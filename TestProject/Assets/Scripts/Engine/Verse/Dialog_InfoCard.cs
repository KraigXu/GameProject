using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DF RID: 991
	public class Dialog_InfoCard : Window
	{
		// Token: 0x06001D67 RID: 7527 RVA: 0x000B4961 File Offset: 0x000B2B61
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x000B498D File Offset: 0x000B2B8D
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x000B49B9 File Offset: 0x000B2BB9
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000B49E8 File Offset: 0x000B2BE8
		public static void PushCurrentToHistoryAndClose()
		{
			Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
			if (dialog_InfoCard == null)
			{
				return;
			}
			Dialog_InfoCard.history.Add(new Dialog_InfoCard.Hyperlink(dialog_InfoCard, StatsReportUtility.SelectedStatIndex));
			Find.WindowStack.TryRemove(dialog_InfoCard, false);
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x000B4A26 File Offset: 0x000B2C26
		private Def Def
		{
			get
			{
				if (this.thing != null)
				{
					return this.thing.def;
				}
				if (this.worldObject != null)
				{
					return this.worldObject.def;
				}
				return this.def;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x000B4A56 File Offset: 0x000B2C56
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x000B4A63 File Offset: 0x000B2C63
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000B4A74 File Offset: 0x000B2C74
		public Dialog_InfoCard(Thing thing)
		{
			this.thing = thing;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000B4A90 File Offset: 0x000B2C90
		public Dialog_InfoCard(Def onlyDef)
		{
			this.def = onlyDef;
			this.Setup();
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000B4AA5 File Offset: 0x000B2CA5
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.Setup();
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000B4AC1 File Offset: 0x000B2CC1
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000B4ADD File Offset: 0x000B2CDD
		public Dialog_InfoCard(Faction faction)
		{
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000B4AF2 File Offset: 0x000B2CF2
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000B4B07 File Offset: 0x000B2D07
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000B4B1C File Offset: 0x000B2D1C
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.soundAppear = SoundDefOf.InfoCard_Open;
			this.soundClose = SoundDefOf.InfoCard_Close;
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000B4B74 File Offset: 0x000B2D74
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true);
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000B4BE4 File Offset: 0x000B2DE4
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			rect.x += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect.x + 9f, rect.y, 34f, 34f);
			if (this.thing != null)
			{
				Widgets.ThingIcon(rect2, this.thing, 1f);
			}
			else
			{
				Widgets.DefIcon(rect2, this.def, this.stuff, 1f, true);
			}
			Rect rect3 = new Rect(inRect);
			rect3.yMin = rect.yMax;
			rect3.yMax -= 38f;
			Rect rect4 = rect3;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), delegate
			{
				this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			}, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), delegate
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Character;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), delegate
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Health;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Health);
				list.Add(item3);
				TabRecord item4 = new TabRecord("TabRecords".Translate(), delegate
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Records;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Records);
				list.Add(item4);
			}
			if (list.Count > 1)
			{
				rect4.yMin += 45f;
				TabDrawer.DrawTabs(rect4, list, 200f);
			}
			this.FillCard(rect4.ContractedBy(18f));
			if (this.def != null && this.def is BuildableDef)
			{
				IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor((BuildableDef)this.def, TechLevel.Undefined);
				if (enumerable.Count<ThingDef>() > 0 && Dialog_InfoCard.ShowMaterialsButton(inRect, Dialog_InfoCard.history.Count > 0))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (ThingDef thingDef in enumerable)
					{
						ThingDef localStuff = thingDef;
						list2.Add(new FloatMenuOption(thingDef.label, delegate
						{
							this.stuff = localStuff;
							this.Setup();
						}, thingDef, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (Dialog_InfoCard.history.Count > 0 && Widgets.BackButtonFor(inRect))
			{
				Dialog_InfoCard.Hyperlink hyperlink = Dialog_InfoCard.history[Dialog_InfoCard.history.Count - 1];
				Dialog_InfoCard.history.RemoveAt(Dialog_InfoCard.history.Count - 1);
				Find.WindowStack.TryRemove(typeof(Dialog_InfoCard), false);
				hyperlink.OpenDialog();
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000B4F34 File Offset: 0x000B3134
		protected void FillCard(Rect cardRect)
		{
			if (this.tab == Dialog_InfoCard.InfoCardTab.Stats)
			{
				if (this.thing != null)
				{
					Thing innerThing = this.thing;
					MinifiedThing minifiedThing = this.thing as MinifiedThing;
					if (minifiedThing != null)
					{
						innerThing = minifiedThing.InnerThing;
					}
					StatsReportUtility.DrawStatsReport(cardRect, innerThing);
				}
				else if (this.titleDef != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.titleDef, this.faction);
				}
				else if (this.faction != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.faction);
				}
				else if (this.worldObject != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.worldObject);
				}
				else if (this.def is AbilityDef)
				{
					StatsReportUtility.DrawStatsReport(cardRect, (AbilityDef)this.def);
				}
				else
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.def, this.stuff);
				}
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect));
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Health)
			{
				cardRect.yMin += 8f;
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, false, null);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			if (this.executeAfterFillCardOnce != null)
			{
				this.executeAfterFillCardOnce();
				this.executeAfterFillCardOnce = null;
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000B5094 File Offset: 0x000B3294
		private string GetTitle()
		{
			if (this.thing != null)
			{
				return this.thing.LabelCapNoCount;
			}
			if (this.worldObject != null)
			{
				return this.worldObject.LabelCap;
			}
			ThingDef thingDef = this.Def as ThingDef;
			if (thingDef != null)
			{
				return GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
			}
			AbilityDef abilityDef = this.Def as AbilityDef;
			if (abilityDef != null)
			{
				return abilityDef.LabelCap;
			}
			if (this.titleDef != null)
			{
				return this.titleDef.GetLabelCapForBothGenders();
			}
			if (this.faction != null)
			{
				return this.faction.Name;
			}
			return this.Def.LabelCap;
		}

		// Token: 0x040011D6 RID: 4566
		private const float ShowMaterialsButtonWidth = 200f;

		// Token: 0x040011D7 RID: 4567
		private const float ShowMaterialsButtonHeight = 40f;

		// Token: 0x040011D8 RID: 4568
		private const float ShowMaterialsMargin = 16f;

		// Token: 0x040011D9 RID: 4569
		private Action executeAfterFillCardOnce;

		// Token: 0x040011DA RID: 4570
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		// Token: 0x040011DB RID: 4571
		private Thing thing;

		// Token: 0x040011DC RID: 4572
		private ThingDef stuff;

		// Token: 0x040011DD RID: 4573
		private Def def;

		// Token: 0x040011DE RID: 4574
		private WorldObject worldObject;

		// Token: 0x040011DF RID: 4575
		private RoyalTitleDef titleDef;

		// Token: 0x040011E0 RID: 4576
		private Faction faction;

		// Token: 0x040011E1 RID: 4577
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x0200164F RID: 5711
		private enum InfoCardTab : byte
		{
			// Token: 0x040055A8 RID: 21928
			Stats,
			// Token: 0x040055A9 RID: 21929
			Character,
			// Token: 0x040055AA RID: 21930
			Health,
			// Token: 0x040055AB RID: 21931
			Records
		}

		// Token: 0x02001650 RID: 5712
		public struct Hyperlink
		{
			// Token: 0x170014FB RID: 5371
			// (get) Token: 0x0600845B RID: 33883 RVA: 0x002AFD70 File Offset: 0x002ADF70
			public string Label
			{
				get
				{
					string result = null;
					if (this.worldObject != null)
					{
						result = this.worldObject.Label;
					}
					else if (this.def != null && this.def is ThingDef && this.stuff != null)
					{
						result = (this.def as ThingDef).label;
					}
					else if (this.def != null)
					{
						result = this.def.label;
					}
					else if (this.thing != null)
					{
						result = this.thing.Label;
					}
					else if (this.titleDef != null)
					{
						result = this.titleDef.GetLabelCapForBothGenders();
					}
					return result;
				}
			}

			// Token: 0x0600845C RID: 33884 RVA: 0x002AFE08 File Offset: 0x002AE008
			public Hyperlink(Dialog_InfoCard infoCard, int statIndex = -1)
			{
				this.def = infoCard.def;
				this.thing = infoCard.thing;
				this.stuff = infoCard.stuff;
				this.worldObject = infoCard.worldObject;
				this.titleDef = infoCard.titleDef;
				this.faction = infoCard.faction;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x0600845D RID: 33885 RVA: 0x002AFE64 File Offset: 0x002AE064
			public Hyperlink(Def def, int statIndex = -1)
			{
				this.def = def;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x0600845E RID: 33886 RVA: 0x002AFE97 File Offset: 0x002AE097
			public Hyperlink(RoyalTitleDef titleDef, Faction faction, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = titleDef;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x0600845F RID: 33887 RVA: 0x002AFECA File Offset: 0x002AE0CA
			public Hyperlink(Thing thing, int statIndex = -1)
			{
				this.thing = thing;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
			}

			// Token: 0x06008460 RID: 33888 RVA: 0x002AFF00 File Offset: 0x002AE100
			public void OpenDialog()
			{
				Dialog_InfoCard dialog_InfoCard = null;
				if (this.def == null && this.thing == null && this.worldObject == null && this.titleDef == null)
				{
					dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
				}
				else
				{
					Dialog_InfoCard.PushCurrentToHistoryAndClose();
					if (this.worldObject != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.worldObject);
					}
					else if (this.def != null && this.def is ThingDef && (this.stuff != null || GenStuff.DefaultStuffFor((ThingDef)this.def) != null))
					{
						dialog_InfoCard = new Dialog_InfoCard(this.def as ThingDef, this.stuff ?? GenStuff.DefaultStuffFor((ThingDef)this.def));
					}
					else if (this.def != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.def);
					}
					else if (this.thing != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.thing);
					}
					else if (this.titleDef != null)
					{
						dialog_InfoCard = new Dialog_InfoCard(this.titleDef, this.faction);
					}
				}
				if (dialog_InfoCard == null)
				{
					return;
				}
				int localSelectedStatIndex = this.selectedStatIndex;
				if (this.selectedStatIndex >= 0)
				{
					dialog_InfoCard.executeAfterFillCardOnce = delegate
					{
						StatsReportUtility.SelectEntry(localSelectedStatIndex);
					};
				}
				Find.WindowStack.Add(dialog_InfoCard);
			}

			// Token: 0x040055AC RID: 21932
			public Thing thing;

			// Token: 0x040055AD RID: 21933
			public ThingDef stuff;

			// Token: 0x040055AE RID: 21934
			public Def def;

			// Token: 0x040055AF RID: 21935
			public WorldObject worldObject;

			// Token: 0x040055B0 RID: 21936
			public RoyalTitleDef titleDef;

			// Token: 0x040055B1 RID: 21937
			public Faction faction;

			// Token: 0x040055B2 RID: 21938
			public int selectedStatIndex;
		}
	}
}
