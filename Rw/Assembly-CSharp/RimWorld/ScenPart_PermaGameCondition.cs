using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0C RID: 3084
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06004968 RID: 18792 RVA: 0x0018E998 File Offset: 0x0018CB98
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x0018E9DC File Offset: 0x0018CBDC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.gameCondition.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate
				{
					this.gameCondition = d;
				});
			}
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x0018EA45 File Offset: 0x0018CC45
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x0018EA5D File Offset: 0x0018CC5D
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x0018EA70 File Offset: 0x0018CC70
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x0018EA9B File Offset: 0x0018CC9B
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x0018EAB7 File Offset: 0x0018CCB7
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x0018EAD0 File Offset: 0x0018CCD0
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x0018EAF8 File Offset: 0x0018CCF8
		public override bool CanCoexistWith(ScenPart other)
		{
			if (this.gameCondition == null)
			{
				return true;
			}
			ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
			return scenPart_PermaGameCondition == null || this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition);
		}

		// Token: 0x040029E5 RID: 10725
		private GameConditionDef gameCondition;

		// Token: 0x040029E6 RID: 10726
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
