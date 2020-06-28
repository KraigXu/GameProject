using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000E88 RID: 3720
	public class Page_ScenarioEditor : Page
	{
		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x001ED7CA File Offset: 0x001EB9CA
		public override string PageTitle
		{
			get
			{
				return "ScenarioEditor".Translate();
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x001ED7DB File Offset: 0x001EB9DB
		public Scenario EditingScenario
		{
			get
			{
				return this.curScen;
			}
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x001ED7E3 File Offset: 0x001EB9E3
		public Page_ScenarioEditor(Scenario scen)
		{
			if (scen != null)
			{
				this.curScen = scen;
				this.seedIsValid = false;
				return;
			}
			this.RandomizeSeedAndScenario();
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x001ED815 File Offset: 0x001EBA15
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x001ED828 File Offset: 0x001EBA28
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			GUI.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoConfigControls(rect2);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded();
			if (!this.editMode)
			{
				ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			}
			else
			{
				ScenarioUI.DrawScenarioEditInterface(rect3, this.curScen, ref this.infoScrollPosition);
			}
			GUI.EndGroup();
			base.DoBottomButtons(rect, null, null, null, true, true);
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x001ED8F8 File Offset: 0x001EBAF8
		private void RandomizeSeedAndScenario()
		{
			this.seed = GenText.RandomSeedString();
			this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x001ED918 File Offset: 0x001EBB18
		private void DoConfigControls(Rect rect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(rect);
			if (listing_Standard.ButtonText("Load".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Load(delegate(Scenario loadedScen)
				{
					this.curScen = loadedScen;
					this.seedIsValid = false;
				}));
			}
			if (listing_Standard.ButtonText("Save".Translate(), null) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen));
			}
			if (listing_Standard.ButtonText("RandomizeSeed".Translate(), null))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.RandomizeSeedAndScenario();
				this.seedIsValid = true;
			}
			if (this.seedIsValid)
			{
				listing_Standard.Label("Seed".Translate().CapitalizeFirst(), -1f, null);
				string a = listing_Standard.TextEntry(this.seed, 1);
				if (a != this.seed)
				{
					this.seed = a;
					this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
				}
			}
			else
			{
				listing_Standard.Gap(Text.LineHeight + Text.LineHeight + 2f);
			}
			listing_Standard.CheckboxLabeled("EditMode".Translate().CapitalizeFirst(), ref this.editMode, null);
			if (this.editMode)
			{
				this.seedIsValid = false;
				if (listing_Standard.ButtonText("AddPart".Translate(), null))
				{
					this.OpenAddScenPartMenu();
				}
				if (SteamManager.Initialized && (this.curScen.Category == ScenarioCategory.CustomLocal || this.curScen.Category == ScenarioCategory.SteamWorkshop) && listing_Standard.ButtonText(Workshop.UploadButtonLabel(this.curScen.GetPublishedFileId()), null) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
				{
					AcceptanceReport acceptanceReport = this.curScen.TryUploadReport();
					if (!acceptanceReport.Accepted)
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								Workshop.Upload(this.curScen);
							}, true, null));
						}, true, null));
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x001EDB54 File Offset: 0x001EBD54
		private static bool CheckAllPartsCompatible(Scenario scen)
		{
			foreach (ScenPart scenPart in scen.AllParts)
			{
				int num = 0;
				foreach (ScenPart scenPart2 in scen.AllParts)
				{
					if (scenPart2.def == scenPart.def)
					{
						num++;
					}
					if (num > scenPart.def.maxUses)
					{
						Messages.Message("TooMany".Translate(scenPart.def.maxUses) + ": " + scenPart.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
					if (scenPart != scenPart2 && !scenPart.CanCoexistWith(scenPart2))
					{
						Messages.Message("Incompatible".Translate() + ": " + scenPart.def.label + ", " + scenPart2.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x001EDCCC File Offset: 0x001EBECC
		private void OpenAddScenPartMenu()
		{
			FloatMenuUtility.MakeMenu<ScenPartDef>(from p in ScenarioMaker.AddableParts(this.curScen)
			where p.category != ScenPartCategory.Fixed
			orderby p.label
			select p, (ScenPartDef p) => p.LabelCap, (ScenPartDef p) => delegate
			{
				this.AddScenPart(p);
			});
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x001EDD5C File Offset: 0x001EBF5C
		private void AddScenPart(ScenPartDef def)
		{
			ScenPart scenPart = ScenarioMaker.MakeScenPart(def);
			scenPart.Randomize();
			this.curScen.parts.Add(scenPart);
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x001EDD87 File Offset: 0x001EBF87
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
			if (!Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				return false;
			}
			Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
			return true;
		}

		// Token: 0x04003171 RID: 12657
		private Scenario curScen;

		// Token: 0x04003172 RID: 12658
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x04003173 RID: 12659
		private string seed;

		// Token: 0x04003174 RID: 12660
		private bool seedIsValid = true;

		// Token: 0x04003175 RID: 12661
		private bool editMode;
	}
}
