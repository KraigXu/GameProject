using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Hediff_Addiction : HediffWithComps
	{
		
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x0016148C File Offset: 0x0015F68C
		public Need_Chemical Need
		{
			get
			{
				if (this.pawn.Dead)
				{
					return null;
				}
				List<Need> allNeeds = this.pawn.needs.AllNeeds;
				for (int i = 0; i < allNeeds.Count; i++)
				{
					if (allNeeds[i].def == this.def.causesNeed)
					{
						return (Need_Chemical)allNeeds[i];
					}
				}
				return null;
			}
		}

		
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x001614F4 File Offset: 0x0015F6F4
		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].addictionHediff == this.def)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			}
		}

		
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x00161538 File Offset: 0x0015F738
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				string text = (1f - this.Severity).ToStringPercent("F0");
				if (this.def.CompProps<HediffCompProperties_SeverityPerDay>() == null)
				{
					return labelInBrackets;
				}
				if (!labelInBrackets.NullOrEmpty())
				{
					return labelInBrackets + " " + text;
				}
				return text;
			}
		}

		
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x00161588 File Offset: 0x0015F788
		public override string TipStringExtra
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need != null)
				{
					return "CreatesNeed".Translate() + ": " + need.LabelCap + " (" + need.CurLevelPercentage.ToStringPercent("F0") + ")";
				}
				return null;
			}
		}

		
		// (get) Token: 0x06004225 RID: 16933 RVA: 0x001615F0 File Offset: 0x0015F7F0
		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need == null || need.CurCategory != DrugDesireCategory.Withdrawal)
				{
					return 0;
				}
				return 1;
			}
		}

		
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		
		private const int DefaultStageIndex = 0;

		
		private const int WithdrawalStageIndex = 1;
	}
}
