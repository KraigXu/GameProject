using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	
	public class Page_ModsConfig : Page
	{
		
		public Page_ModsConfig()
		{
			this.doCloseButton = true;
			this.closeOnCancel = true;
		}

		
		public override void PreOpen()
		{
			base.PreOpen();
			ModLister.RebuildModList();
			this.modsInListOrderDirty = true;
			this.selectedMod = this.ModsInListOrder().FirstOrDefault<ModMetaData>();
			this.activeModsWhenOpenedHash = ModLister.InstalledModsListHash(true);
			this.RecacheSelectedModRequirements();
		}

		
		private List<ModMetaData> ModsInListOrder()
		{
			if (this.modsInListOrderDirty)
			{
				Page_ModsConfig.modsInListOrderCached.Clear();
				Page_ModsConfig.modsInListOrderCached.AddRange(ModsConfig.ActiveModsInLoadOrder);
				Page_ModsConfig.modsInListOrderCached.AddRange(from x in ModLister.AllInstalledMods
				where !x.Active
				select x into m
				orderby m.VersionCompatible descending
				select m);
				this.modsInListOrderDirty = false;
			}
			return Page_ModsConfig.modsInListOrderCached;
		}

		
		public override void DoWindowContents(Rect rect)
		{
			Rect mainRect = base.GetMainRect(rect, 0f, true);
			GUI.BeginGroup(mainRect);
			Text.Font = GameFont.Small;
			float num = 0f;
			if (Widgets.ButtonText(new Rect(17f, num, 316f, 30f), "OpenSteamWorkshop".Translate(), true, true, true))
			{
				SteamUtility.OpenSteamWorkshopPage();
			}
			num += 30f;
			if (Widgets.ButtonText(new Rect(17f, num, 316f, 30f), "GetModsFromForum".Translate(), true, true, true))
			{
				Application.OpenURL("http://rimworldgame.com/getmods");
			}
			num += 30f;
			num += 17f;
			this.filter = Widgets.TextField(new Rect(0f, num, 350f, 30f), this.filter);
			num += 30f;
			num += 10f;
			float num2 = 47f;
			Rect rect2 = new Rect(0f, num, 350f, mainRect.height - num - num2);
			Widgets.DrawMenuSection(rect2);
			float height = (float)ModLister.AllInstalledMods.Count<ModMetaData>() * 26f + 8f;
			Rect rect3 = new Rect(0f, 0f, rect2.width - 16f, height);
			Widgets.BeginScrollView(rect2, ref this.modListScrollPosition, rect3, true);
			Rect rect4 = rect3.ContractedBy(4f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect4.width;
			float num3 = this.modListScrollPosition.y - 26f;
			float num4 = this.modListScrollPosition.y + rect2.height;
			listing_Standard.Begin(rect4);
			int num5 = ReorderableWidget.NewGroup(delegate(int from, int to)
			{
				ModsConfig.Reorder(from, to);
				this.modsInListOrderDirty = true;
			}, ReorderableDirection.Vertical, -1f, null);
			int num6 = 0;
			foreach (ModMetaData modMetaData in this.ModsInListOrder())
			{
				float num7 = (float)num6 * 26f;
				bool active = modMetaData.Active;
				Rect rect5 = new Rect(0f, (float)num6 * 26f, listing_Standard.ColumnWidth, 26f);
				if (active)
				{
					ReorderableWidget.Reorderable(num5, rect5, false);
				}
				if (num7 >= num3 && num7 <= num4)
				{
					this.DoModRow(rect5, modMetaData, num6, num5);
				}
				num6++;
			}
			int downloadingItemsCount = WorkshopItems.DownloadingItemsCount;
			for (int i = 0; i < downloadingItemsCount; i++)
			{
				this.DoModRowDownloading(listing_Standard, num6);
				num6++;
			}
			listing_Standard.End();
			Widgets.EndScrollView();
			num += rect2.height;
			num += 10f;
			if (Widgets.ButtonText(new Rect(17f, num, 316f, 30f), "ResolveModOrder".Translate(), true, true, true))
			{
				ModsConfig.TrySortMods();
				this.modsInListOrderDirty = true;
			}
			Rect position = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height);
			GUI.BeginGroup(position);
			if (this.selectedMod != null)
			{
				Text.Font = GameFont.Medium;
				Rect rect6 = new Rect(0f, 0f, position.width, 40f);
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(rect6, this.selectedMod.Name.Truncate(rect6.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				Rect position2 = new Rect(0f, rect6.yMax, 0f, 20f);
				if (this.selectedMod.PreviewImage != null)
				{
					position2.width = Mathf.Min((float)this.selectedMod.PreviewImage.width, position.width);
					position2.height = (float)this.selectedMod.PreviewImage.height * (position2.width / (float)this.selectedMod.PreviewImage.width);
					float num8 = Mathf.Ceil(position.height * 0.37f);
					if (position2.height > num8)
					{
						float height2 = position2.height;
						position2.height = num8;
						position2.width *= position2.height / height2;
					}
					if (position2.height > 300f)
					{
						position2.width *= 300f / position2.height;
						position2.height = 300f;
					}
					position2.x = position.width / 2f - position2.width / 2f;
					GUI.DrawTexture(position2, this.selectedMod.PreviewImage, ScaleMode.ScaleToFit);
				}
				float num9 = position2.yMax + 10f;
				Text.Font = GameFont.Small;
				float num10 = num9;
				if (!this.selectedMod.Author.NullOrEmpty())
				{
					Widgets.Label(new Rect(0f, num10, position.width / 2f, Text.LineHeight), "Author".Translate() + ": " + this.selectedMod.Author);
					num10 += Text.LineHeight;
				}
				if (!this.selectedMod.PackageId.NullOrEmpty())
				{
					GUI.color = Color.gray;
					Widgets.Label(new Rect(0f, num10, position.width / 2f, Text.LineHeight), "ModPackageId".Translate() + ": " + this.selectedMod.PackageIdPlayerFacing);
					num10 += Text.LineHeight;
					GUI.color = Color.white;
				}
				float num11 = num9;
				WidgetRow widgetRow = new WidgetRow(position.width, num11, UIDirection.LeftThenUp, 99999f, 4f);
				if (SteamManager.Initialized && this.selectedMod.OnSteamWorkshop)
				{
					if (widgetRow.ButtonText("Unsubscribe".Translate(), null, true, true))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribe".Translate(this.selectedMod.Name), delegate
						{
							this.selectedMod.enabled = false;
							Workshop.Unsubscribe(this.selectedMod);
							this.Notify_SteamItemUnsubscribed(this.selectedMod.GetPublishedFileId());
						}, true, null));
					}
					if (widgetRow.ButtonText("WorkshopPage".Translate(), null, true, true))
					{
						SteamUtility.OpenWorkshopPage(this.selectedMod.GetPublishedFileId());
					}
					num11 += 25f;
				}
				if (!this.selectedMod.IsCoreMod)
				{
					Text.Anchor = TextAnchor.UpperRight;
					Rect rect7 = new Rect(position.width - 300f, num11, 300f, Text.LineHeight);
					if (!this.selectedMod.VersionCompatible)
					{
						GUI.color = Color.red;
					}
					Widgets.Label(rect7, "ModTargetVersion".Translate() + ": " + this.selectedMod.SupportedVersionsReadOnly.Select(delegate(System.Version v)
					{
						string text = VersionControl.IsCompatible(v) ? "<color=green>" : "<color=red>";
						string text2 = "</color>";
						if (v.Build > 0)
						{
							return string.Format("{0}{1}.{2}.{3}{4}", new object[]
							{
								text,
								v.Major.ToString(),
								v.Minor.ToString(),
								v.Build.ToString(),
								text2
							});
						}
						return string.Format("{0}{1}.{2}{3}", new object[]
						{
							text,
							v.Major.ToString(),
							v.Minor.ToString(),
							text2
						});
					}).ToCommaList(false));
					GUI.color = Color.white;
					num11 += Text.LineHeight;
				}
				if (this.anyReqsCached)
				{
					Text.Anchor = TextAnchor.MiddleRight;
					TaggedString taggedString = "ModDisplayFulfilledRequirements".Translate();
					float num12 = Text.CalcSize(taggedString).x + 24f + 4f;
					Rect rect8 = new Rect(position.width - num12, num11, num12, 24f);
					bool flag = this.displayFullfilledRequirements;
					Widgets.CheckboxLabeled(rect8, taggedString, ref this.displayFullfilledRequirements, false, null, null, false);
					if (flag != this.displayFullfilledRequirements)
					{
						this.RecacheSelectedModRequirements();
					}
					num11 += 34f;
				}
				Text.Anchor = TextAnchor.UpperLeft;
				float num13 = Mathf.Max(num10, num11) + (this.anyReqsCached ? 10f : 17f);
				Rect outRect = new Rect(0f, num13, position.width, position.height - num13 - 40f);
				float width = outRect.width - 16f;
				float num14 = Text.CalcHeight(this.selectedMod.Description, width);
				num14 = Mathf.Min(num14 * 1.25f, num14 + 200f);
				Rect viewRect = new Rect(0f, 0f, width, num14 + this.modRequirementsHeightCached + (this.anyReqsInfoToShowCached ? 10f : 0f));
				float num15 = (viewRect.height > outRect.height) ? 16f : 0f;
				Widgets.BeginScrollView(outRect, ref this.modDescriptionScrollPosition, viewRect, true);
				float num16 = 0f;
				if (this.anyReqsInfoToShowCached)
				{
					num16 = this.DoRequirementSection(position.width - num15);
					num16 += 10f;
				}
				Widgets.Label(new Rect(0f, num16, viewRect.width - num15, viewRect.height - num16), this.selectedMod.Description);
				Widgets.EndScrollView();
				if (Prefs.DevMode && SteamManager.Initialized && this.selectedMod.CanToUploadToWorkshop() && Widgets.ButtonText(new Rect(0f, position.yMax - 40f, 200f, 40f), Workshop.UploadButtonLabel(this.selectedMod.GetPublishedFileId()), true, true, true))
				{
					ModLoadFolders loadFolders = this.selectedMod.loadFolders;
					List<string> list = (loadFolders != null) ? loadFolders.GetIssueList(this.selectedMod) : null;
					if (this.selectedMod.HadIncorrectlyFormattedVersionInMetadata)
					{
						Messages.Message("MessageModNeedsWellFormattedTargetVersion".Translate(VersionControl.CurrentMajor + "." + VersionControl.CurrentMinor), MessageTypeDefOf.RejectInput, false);
					}
					else if (this.selectedMod.HadIncorrectlyFormattedPackageId)
					{
						Find.WindowStack.Add(new Dialog_MessageBox("MessageModNeedsWellFormattedPackageId".Translate(), null, null, null, null, null, false, null, null));
					}
					else if (!list.NullOrEmpty<string>())
					{
						Find.WindowStack.Add(new Dialog_MessageBox("ModHadLoadFolderIssues".Translate() + "\n" + list.ToLineList("  - "), null, null, null, null, null, false, null, null));
					}
					else
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								Workshop.Upload(this.selectedMod);
							}, true, null);
							dialog_MessageBox.buttonAText = "Yes".Translate();
							dialog_MessageBox.buttonBText = "No".Translate();
							dialog_MessageBox.interactionDelay = 6f;
							Find.WindowStack.Add(dialog_MessageBox);
						}, true, null));
					}
				}
				if (!this.selectedMod.Url.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.MiddleLeft;
					float num17 = Mathf.Min(position.width / 2f, Text.CalcSize(this.selectedMod.Url).x);
					if (Widgets.ButtonText(new Rect(position.width - num17, outRect.yMax, num17, position.yMax - outRect.yMax), this.selectedMod.Url.Truncate(num17, null), false, true, true))
					{
						Application.OpenURL(this.selectedMod.Url);
					}
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			GUI.EndGroup();
			GUI.EndGroup();
			Text.Font = GameFont.Tiny;
			TaggedString taggedString2 = "GameVersionIndicator".Translate() + ": " + VersionControl.CurrentVersionString;
			float x = Text.CalcSize(taggedString2).x;
			Widgets.Label(new Rect(0f, rect.height - 15f, x, Text.LineHeight), taggedString2);
			Text.Font = GameFont.Small;
			int num18 = ModLister.InstalledModsListHash(true);
			if (this.activeModsHash == -1 || this.activeModsHash != num18)
			{
				Page_ModsConfig.modWarningsCached = ModsConfig.GetModWarnings();
				this.RecacheSelectedModRequirements();
				this.activeModsHash = num18;
				this.modsInListOrderDirty = true;
			}
		}

		
		private void DoModRow(Rect r, ModMetaData mod, int index, int reorderableGroup)
		{
			bool active = mod.Active;
			Action clickAction = null;
			if (mod.Source == ContentSource.SteamWorkshop)
			{
				clickAction = delegate
				{
					SteamUtility.OpenWorkshopPage(mod.GetPublishedFileId());
				};
			}
			ContentSourceUtility.DrawContentSource(r, mod.Source, clickAction);
			r.xMin += 28f;
			bool flag = mod == this.selectedMod;
			Rect rect = r;
			if (mod.enabled)
			{
				string text = "";
				if (active)
				{
					text += "DragToReorder".Translate() + ".\n";
				}
				if (!mod.VersionCompatible)
				{
					GUI.color = Color.yellow;
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					if (mod.MadeForNewerVersion)
					{
						text += "ModNotMadeForThisVersion_Newer".Translate();
					}
					else
					{
						text += "ModNotMadeForThisVersion".Translate();
					}
				}
				if (active && !Page_ModsConfig.modWarningsCached.NullOrEmpty<string>() && !Page_ModsConfig.modWarningsCached[index].NullOrEmpty())
				{
					GUI.color = Color.red;
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += Page_ModsConfig.modWarningsCached[index];
				}
				GUI.color = this.FilteredColor(GUI.color, mod.Name);
				if (!text.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, new TipSignal(text, mod.GetHashCode() * 3311));
				}
				float num = rect.width - 24f;
				if (active)
				{
					GUI.DrawTexture(new Rect(rect.xMax - 48f + 2f, rect.y, 24f, 24f), TexButton.DragHash);
					num -= 24f;
				}
				Text.Font = GameFont.Small;
				string label = mod.Name.Truncate(num, this.truncatedModNamesCache);
				bool flag2 = active;
				if (Widgets.CheckboxLabeledSelectable(rect, label, ref flag, ref flag2))
				{
					this.selectedMod = mod;
					this.RecacheSelectedModRequirements();
				}
				if (active && !flag2 && mod.IsCoreMod)
				{
					ModMetaData coreMod = mod;
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDisableCoreMod".Translate(), delegate
					{
						coreMod.Active = false;
						this.truncatedModNamesCache.Clear();
					}, false, null));
				}
				else
				{
					if (!active && flag2)
					{
						foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
						{
							if (modMetaData.PackageIdNonUnique.Equals(mod.PackageIdNonUnique, StringComparison.InvariantCultureIgnoreCase))
							{
								Find.WindowStack.Add(new Dialog_MessageBox("MessageModWithPackageIdAlreadyEnabled".Translate(mod.PackageIdPlayerFacing, modMetaData.Name), null, null, null, null, null, false, null, null));
								return;
							}
						}
					}
					if (flag2 != active)
					{
						mod.Active = flag2;
					}
					this.truncatedModNamesCache.Clear();
				}
			}
			else
			{
				GUI.color = this.FilteredColor(Color.gray, mod.Name);
				Widgets.Label(rect, mod.Name);
			}
			GUI.color = Color.white;
		}

		
		private void DoModRowDownloading(Listing_Standard listing, int index)
		{
			Rect rect = new Rect(0f, (float)index * 26f, listing.ColumnWidth, 26f);
			ContentSourceUtility.DrawContentSource(rect, ContentSource.SteamWorkshop, null);
			rect.xMin += 28f;
			Widgets.Label(rect, "Downloading".Translate() + GenText.MarchingEllipsis(0f));
		}

		
		private float DoRequirementSection(float width)
		{
			float num = 0f;
			if (this.visibleReqsCached.Count > 0 || this.anyOrderingIssuesCached)
			{
				object obj = this.anyUnfulfilledReqsCached || this.anyOrderingIssuesCached;
				Rect rect = new Rect(0f, 0f, width, this.modRequirementsHeightCached);
				object obj2 = obj;
				if (obj2 != null)
				{
					Widgets.DrawBoxSolid(rect, Page_ModsConfig.UnmetRequirementBoxBGColor);
				}
				GUI.color = ((obj2 != null) ? Page_ModsConfig.UnmetRequirementBoxOutlineColor : Page_ModsConfig.RequirementBoxOutlineColor);
				Widgets.DrawBox(rect, 1);
				GUI.color = Color.white;
				num += 10f;
				Text.Anchor = TextAnchor.MiddleLeft;
				for (int i = 0; i < this.visibleReqsCached.Count; i++)
				{
					Rect entryRect = new Rect(11f, num, width - 20f - 1f, 26f);
					this.DrawRequirementEntry(this.visibleReqsCached[i], entryRect, ref num);
					if (i < this.visibleReqsCached.Count - 1)
					{
						num += 4f;
					}
				}
				if (this.anyOrderingIssuesCached)
				{
					num += 4f;
					Widgets.Label(new Rect(10f, num, width - 20f, Text.LineHeight * 2f), "ModOrderingWarning".Translate());
					num += Text.LineHeight * 2f;
				}
				num += 10f;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return num;
		}

		
		private void DrawRequirementEntry(ModRequirement entry, Rect entryRect, ref float y)
		{
			Widgets.DrawBoxSolid(entryRect, entry.IsSatisfied ? Page_ModsConfig.RequirementRowColor : (Mouse.IsOver(entryRect) ? Page_ModsConfig.UnmetRequirementRowColorHighlighted : Page_ModsConfig.UnmetRequirementRowColor));
			Rect rect = entryRect;
			rect.x += 4f;
			rect.width = 200f;
			Widgets.Label(rect, entry.RequirementTypeLabel.Truncate(rect.width, this.truncatedStringCache));
			Rect rect2 = entryRect;
			rect2.x = rect.xMax + 4f;
			rect2.width -= rect2.x + 24f;
			Widgets.Label(rect2, entry.displayName.Truncate(rect2.width, this.truncatedStringCache));
			if (Widgets.ButtonInvisible(entryRect, true))
			{
				entry.OnClicked(this);
			}
			GUI.DrawTexture(new Rect
			{
				xMin = entryRect.xMax - 24f - 4f,
				y = entryRect.y + 1f,
				width = 24f,
				height = 24f
			}, entry.StatusIcon);
			TooltipHandler.TipRegion(entryRect, new TipSignal(entry.Tooltip));
			y += 26f;
		}

		
		private void RecacheSelectedModRequirements()
		{
			this.anyReqsCached = false;
			this.anyReqsInfoToShowCached = false;
			this.anyUnfulfilledReqsCached = false;
			this.anyOrderingIssuesCached = false;
			this.visibleReqsCached.Clear();
			if (this.selectedMod != null)
			{
				foreach (ModRequirement modRequirement in (from r in this.selectedMod.GetRequirements()
				orderby r.IsSatisfied, r.RequirementTypeLabel
				select r).ToList<ModRequirement>())
				{
					bool isSatisfied = modRequirement.IsSatisfied;
					if (!isSatisfied || this.displayFullfilledRequirements)
					{
						this.visibleReqsCached.Add(modRequirement);
						if (!isSatisfied)
						{
							this.anyUnfulfilledReqsCached = true;
						}
					}
					this.anyReqsCached = true;
					this.anyReqsInfoToShowCached = true;
				}
				this.anyOrderingIssuesCached = ModsConfig.ModHasAnyOrderingIssues(this.selectedMod);
				if (this.visibleReqsCached.Any<ModRequirement>() || this.anyOrderingIssuesCached)
				{
					this.anyReqsInfoToShowCached = true;
					this.modRequirementsHeightCached = (float)this.visibleReqsCached.Count * 26f + (float)(this.visibleReqsCached.Count - 1) * 4f + 20f + 1f;
					if (this.anyOrderingIssuesCached)
					{
						this.modRequirementsHeightCached += Text.LineHeight * 2f + 4f;
						return;
					}
				}
				else
				{
					this.modRequirementsHeightCached = 0f;
				}
			}
		}

		
		public void Notify_ModsListChanged()
		{
			string selModId = this.selectedMod.PackageId;
			this.selectedMod = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.SamePackageId(selModId, false));
			this.RecacheSelectedModRequirements();
			this.modsInListOrderDirty = true;
		}

		
		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.selectedMod != null && this.selectedMod.FolderName == pfid.ToString())
			{
				this.selectedMod = null;
			}
			this.RecacheSelectedModRequirements();
			this.modsInListOrderDirty = true;
		}

		
		public void SelectMod(ModMetaData mod)
		{
			this.selectedMod = mod;
			this.RecacheSelectedModRequirements();
		}

		
		public override void PostClose()
		{
			ModsConfig.Save();
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				modMetaData.UnsetPreviewImage();
			}
			Resources.UnloadUnusedAssets();
			if (this.activeModsWhenOpenedHash != ModLister.InstalledModsListHash(true))
			{
				ModsConfig.RestartFromChangedMods();
			}
		}

		
		private Color FilteredColor(Color color, string label)
		{
			if (this.filter.NullOrEmpty())
			{
				return color;
			}
			if (label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return color;
			}
			return color * new Color(1f, 1f, 1f, 0.3f);
		}

		
		public ModMetaData selectedMod;

		
		private Vector2 modListScrollPosition = Vector2.zero;

		
		private Vector2 modDescriptionScrollPosition = Vector2.zero;

		
		private int activeModsWhenOpenedHash = -1;

		
		private int activeModsHash = -1;

		
		private bool displayFullfilledRequirements;

		
		protected string filter = "";

		
		private Dictionary<string, string> truncatedModNamesCache = new Dictionary<string, string>();

		
		private static List<string> modWarningsCached = new List<string>();

		
		private List<ModRequirement> visibleReqsCached = new List<ModRequirement>();

		
		private bool anyReqsCached;

		
		private bool anyReqsInfoToShowCached;

		
		private bool anyUnfulfilledReqsCached;

		
		private bool anyOrderingIssuesCached;

		
		private float modRequirementsHeightCached;

		
		private bool modsInListOrderDirty;

		
		private static List<ModMetaData> modsInListOrderCached = new List<ModMetaData>();

		
		private const float ModListAreaWidth = 350f;

		
		private const float ModsListButtonHeight = 30f;

		
		private const float ModsFolderButHeight = 30f;

		
		private const float ButtonsGap = 4f;

		
		private const float UploadRowHeight = 40f;

		
		private const float PreviewMaxHeight = 300f;

		
		private const float VersionWidth = 30f;

		
		private const float ModRowHeight = 26f;

		
		private const float RequirementBoxInnerOffset = 10f;

		
		private static readonly Color RequirementBoxOutlineColor = new Color(0.25f, 0.25f, 0.25f);

		
		private static readonly Color UnmetRequirementBoxOutlineColor = new Color(0.62f, 0.18f, 0.18f);

		
		private static readonly Color UnmetRequirementBoxBGColor = new Color(0.1f, 0.065f, 0.072f);

		
		private static readonly Color RequirementRowColor = new Color(0.13f, 0.13f, 0.13f);

		
		private static readonly Color UnmetRequirementRowColor = new Color(0.23f, 0.15f, 0.15f);

		
		private static readonly Color UnmetRequirementRowColorHighlighted = new Color(0.27f, 0.18f, 0.18f);

		
		private Dictionary<string, string> truncatedStringCache = new Dictionary<string, string>();
	}
}
