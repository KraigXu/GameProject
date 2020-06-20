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
	// Token: 0x02000E89 RID: 3721
	[StaticConstructorOnStartup]
	public class Page_SelectScenario : Page
	{
		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06005AAF RID: 23215 RVA: 0x001EDE35 File Offset: 0x001EC035
		public override string PageTitle
		{
			get
			{
				return "ChooseScenario".Translate();
			}
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x001EDE46 File Offset: 0x001EC046
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
			ScenarioLister.MarkDirty();
			this.EnsureValidSelection();
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x001EDE64 File Offset: 0x001EC064
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			GUI.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoScenarioSelectionList(rect2);
			ScenarioUI.DrawScenarioInfo(new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded(), this.curScen, ref this.infoScrollPosition);
			GUI.EndGroup();
			base.DoBottomButtons(rect, null, "ScenarioEditor".Translate(), new Action(this.GoToScenarioEditor), true, true);
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x001EDF2F File Offset: 0x001EC12F
		private bool CanEditScenario(Scenario scen)
		{
			return scen.Category == ScenarioCategory.CustomLocal || scen.CanToUploadToWorkshop();
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x001EDF48 File Offset: 0x001EC148
		private void GoToScenarioEditor()
		{
			Page_ScenarioEditor page_ScenarioEditor = new Page_ScenarioEditor(this.CanEditScenario(this.curScen) ? this.curScen : this.curScen.CopyForEditing());
			page_ScenarioEditor.prev = this;
			Find.WindowStack.Add(page_ScenarioEditor);
			this.Close(true);
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x001EDF98 File Offset: 0x001EC198
		private void DoScenarioSelectionList(Rect rect)
		{
			rect.xMax += 2f;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f - 2f, this.totalScenarioListHeight + 250f);
			Widgets.BeginScrollView(rect, ref this.scenariosScrollPosition, rect2, true);
			Rect rect3 = rect2.AtZero();
			rect3.height = 999999f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect2.width;
			listing_Standard.Begin(rect3);
			Text.Font = GameFont.Small;
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosCustom".Translate(), -1f, null);
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.CustomLocal));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosSteamWorkshop".Translate(), -1f, null);
			if (listing_Standard.ButtonText("OpenSteamWorkshop".Translate(), null))
			{
				SteamUtility.OpenSteamWorkshopPage();
			}
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.SteamWorkshop));
			listing_Standard.End();
			this.totalScenarioListHeight = listing_Standard.CurHeight;
			Widgets.EndScrollView();
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x001EE0D4 File Offset: 0x001EC2D4
		private void ListScenariosOnListing(Listing_Standard listing, IEnumerable<Scenario> scenarios)
		{
			bool flag = false;
			foreach (Scenario scenario in scenarios)
			{
				if (scenario.showInUI)
				{
					if (flag)
					{
						listing.Gap(12f);
					}
					Scenario scen = scenario;
					Rect rect = listing.GetRect(62f);
					this.DoScenarioListEntry(rect, scen);
					flag = true;
				}
			}
			if (!flag)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				listing.Label("(" + "NoneLower".Translate() + ")", -1f, null);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x001EE1A0 File Offset: 0x001EC3A0
		private void DoScenarioListEntry(Rect rect, Scenario scen)
		{
			bool flag = this.curScen == scen;
			Widgets.DrawOptionBackground(rect, flag);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = rect.ContractedBy(4f);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.height = Text.CalcHeight(scen.name, rect3.width);
			Widgets.Label(rect3, scen.name);
			Text.Font = GameFont.Tiny;
			Rect rect4 = rect2;
			rect4.yMin = rect3.yMax;
			Widgets.Label(rect4, scen.GetSummary());
			if (scen.enabled)
			{
				WidgetRow widgetRow = new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenDown, 99999f, 4f);
				if (scen.Category == ScenarioCategory.CustomLocal && widgetRow.ButtonIcon(TexButton.DeleteX, "Delete".Translate(), new Color?(GenUI.SubtleMouseoverColor), true))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(scen.File.Name), delegate
					{
						scen.File.Delete();
						ScenarioLister.MarkDirty();
					}, true, null));
				}
				if (scen.Category == ScenarioCategory.SteamWorkshop && widgetRow.ButtonIcon(TexButton.DeleteX, "Unsubscribe".Translate(), new Color?(GenUI.SubtleMouseoverColor), true))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribe".Translate(scen.File.Name), delegate
					{
						scen.enabled = false;
						if (this.curScen == scen)
						{
							this.curScen = null;
							this.EnsureValidSelection();
						}
						Workshop.Unsubscribe(scen);
					}, true, null));
				}
				if (scen.GetPublishedFileId() != PublishedFileId_t.Invalid)
				{
					if (widgetRow.ButtonIcon(ContentSource.SteamWorkshop.GetIcon(), "WorkshopPage".Translate(), null, true))
					{
						SteamUtility.OpenWorkshopPage(scen.GetPublishedFileId());
					}
					if (scen.CanToUploadToWorkshop())
					{
						widgetRow.Icon(Page_SelectScenario.CanUploadIcon, "CanBeUpdatedOnWorkshop".Translate());
					}
				}
				if (!flag && Widgets.ButtonInvisible(rect, true))
				{
					this.curScen = scen;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x001EE3F5 File Offset: 0x001EC5F5
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
			return true;
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x001EE418 File Offset: 0x001EC618
		public static void BeginScenarioConfiguration(Scenario scen, Page originPage)
		{
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = scen;
			Current.Game.Scenario.PreConfigure();
			Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
			if (firstConfigPage == null)
			{
				PageUtility.InitGameStart();
				return;
			}
			originPage.next = firstConfigPage;
			firstConfigPage.prev = originPage;
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x001EE47F File Offset: 0x001EC67F
		private void EnsureValidSelection()
		{
			if (this.curScen == null || !ScenarioLister.ScenarioIsListedAnywhere(this.curScen))
			{
				this.curScen = ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef).FirstOrDefault<Scenario>();
			}
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x001EE4A8 File Offset: 0x001EC6A8
		internal void Notify_ScenarioListChanged()
		{
			PublishedFileId_t selModId = this.curScen.GetPublishedFileId();
			this.curScen = ScenarioLister.AllScenarios().FirstOrDefault((Scenario sc) => sc.GetPublishedFileId() == selModId);
			this.EnsureValidSelection();
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x001EE4EE File Offset: 0x001EC6EE
		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.curScen != null && this.curScen.GetPublishedFileId() == pfid)
			{
				this.curScen = null;
			}
			this.EnsureValidSelection();
		}

		// Token: 0x04003176 RID: 12662
		private Scenario curScen;

		// Token: 0x04003177 RID: 12663
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x04003178 RID: 12664
		private const float ScenarioEntryHeight = 62f;

		// Token: 0x04003179 RID: 12665
		private static readonly Texture2D CanUploadIcon = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/CanUpload", true);

		// Token: 0x0400317A RID: 12666
		private Vector2 scenariosScrollPosition = Vector2.zero;

		// Token: 0x0400317B RID: 12667
		private float totalScenarioListHeight;
	}
}
