using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8A RID: 2954
	public class Need_Chemical : Need
	{
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004546 RID: 17734 RVA: 0x0010E022 File Offset: 0x0010C222
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004547 RID: 17735 RVA: 0x0017699A File Offset: 0x00174B9A
		public DrugDesireCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.1f)
				{
					return DrugDesireCategory.Satisfied;
				}
				if (this.CurLevel > 0.01f)
				{
					return DrugDesireCategory.Desire;
				}
				return DrugDesireCategory.Withdrawal;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x001769BB File Offset: 0x00174BBB
		// (set) Token: 0x06004549 RID: 17737 RVA: 0x001769C4 File Offset: 0x00174BC4
		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				DrugDesireCategory curCategory = this.CurCategory;
				base.CurLevel = value;
				if (this.CurCategory != curCategory)
				{
					this.CategoryChanged();
				}
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600454A RID: 17738 RVA: 0x001769F0 File Offset: 0x00174BF0
		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.def.causesNeed == this.def)
					{
						return hediff_Addiction;
					}
				}
				return null;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600454B RID: 17739 RVA: 0x00176A4A File Offset: 0x00174C4A
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x00176A5D File Offset: 0x00174C5D
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x00176A81 File Offset: 0x00174C81
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x00176A98 File Offset: 0x00174C98
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x00176ABC File Offset: 0x00174CBC
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}

		// Token: 0x040027A6 RID: 10150
		private const float ThreshDesire = 0.01f;

		// Token: 0x040027A7 RID: 10151
		private const float ThreshSatisfied = 0.1f;
	}
}
