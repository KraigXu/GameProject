using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0B RID: 3083
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x0018E7F7 File Offset: 0x0018C9F7
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x0018E80E File Offset: 0x0018CA0E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0018E82C File Offset: 0x0018CA2C
		public override string Summary(Scenario scen)
		{
			return this.def.gameCondition.LabelCap + ": " + this.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1") + ")";
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0018E89D File Offset: 0x0018CA9D
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x0018E8BA File Offset: 0x0018CABA
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x0018E8F2 File Offset: 0x0018CAF2
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x0018E912 File Offset: 0x0018CB12
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x0018E936 File Offset: 0x0018CB36
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f));
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x0018E958 File Offset: 0x0018CB58
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		// Token: 0x040029E3 RID: 10723
		private float durationDays;

		// Token: 0x040029E4 RID: 10724
		private string durationDaysBuf;
	}
}
