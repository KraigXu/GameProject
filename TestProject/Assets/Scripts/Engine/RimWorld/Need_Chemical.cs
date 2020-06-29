using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Need_Chemical : Need
	{
		
		// (get) Token: 0x06004546 RID: 17734 RVA: 0x0010E022 File Offset: 0x0010C222
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		
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

		
		// (get) Token: 0x0600454B RID: 17739 RVA: 0x00176A4A File Offset: 0x00174C4A
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
		}

		
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}

		
		private const float ThreshDesire = 0.01f;

		
		private const float ThreshSatisfied = 0.1f;
	}
}
