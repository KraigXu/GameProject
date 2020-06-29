using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	
	public class Dialog_InfoCard : Window
	{
		
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		
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

		
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x000B4A56 File Offset: 0x000B2C56
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x000B4A63 File Offset: 0x000B2C63
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		
		public Dialog_InfoCard(Thing thing)
		{
			this.thing = thing;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		
		public Dialog_InfoCard(Def onlyDef)
		{
			this.def = onlyDef;
			this.Setup();
		}

		
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.Setup();
		}

		
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.Setup();
		}

		
		public Dialog_InfoCard(Faction faction)
		{
			this.faction = faction;
			this.Setup();
		}

		
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		
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

		
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true);
		}

		
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

		
		private const float ShowMaterialsButtonWidth = 200f;

		
		private const float ShowMaterialsButtonHeight = 40f;

		
		private const float ShowMaterialsMargin = 16f;

		
		private Action executeAfterFillCardOnce;

		
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		
		private Thing thing;

		
		private ThingDef stuff;

		
		private Def def;

		
		private WorldObject worldObject;

		
		private RoyalTitleDef titleDef;

		
		private Faction faction;

		
		private Dialog_InfoCard.InfoCardTab tab;

		
		private enum InfoCardTab : byte
		{
			
			Stats,
			
			Character,
			
			Health,
			
			Records
		}

		
		public struct Hyperlink
		{
			
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

			
			public Thing thing;

			
			public ThingDef stuff;

			
			public Def def;

			
			public WorldObject worldObject;

			
			public RoyalTitleDef titleDef;

			
			public Faction faction;

			
			public int selectedStatIndex;
		}
	}
}
