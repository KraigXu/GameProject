using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D03 RID: 3331
	public class CompEggLayer : ThingComp
	{
		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x001B2D50 File Offset: 0x001B0F50
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x001B2D9C File Offset: 0x001B0F9C
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x060050FB RID: 20731 RVA: 0x001B2DB8 File Offset: 0x001B0FB8
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x060050FC RID: 20732 RVA: 0x001B2DD0 File Offset: 0x001B0FD0
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.Props.eggProgressUnfertilizedMax < 1f && this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x001B2E04 File Offset: 0x001B1004
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x001B2E14 File Offset: 0x001B1014
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x001B2E60 File Offset: 0x001B1060
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (this.Props.eggLayIntervalDays * 60000f);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.eggProgress += num;
				if (this.eggProgress > 1f)
				{
					this.eggProgress = 1f;
				}
				if (this.ProgressStoppedBecauseUnfertilized)
				{
					this.eggProgress = this.Props.eggProgressUnfertilizedMax;
				}
			}
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x001B2EE4 File Offset: 0x001B10E4
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x001B2F00 File Offset: 0x001B1100
		public virtual Thing ProduceEgg()
		{
			if (!this.Active)
			{
				Log.Error("LayEgg while not Active: " + this.parent, false);
			}
			this.eggProgress = 0f;
			int randomInRange = this.Props.eggCountRange.RandomInRange;
			if (randomInRange == 0)
			{
				return null;
			}
			Thing thing;
			if (this.fertilizationCount > 0)
			{
				thing = ThingMaker.MakeThing(this.Props.eggFertilizedDef, null);
				this.fertilizationCount = Mathf.Max(0, this.fertilizationCount - randomInRange);
			}
			else
			{
				thing = ThingMaker.MakeThing(this.Props.eggUnfertilizedDef, null);
			}
			thing.stackCount = randomInRange;
			CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
			if (compHatcher != null)
			{
				compHatcher.hatcheeFaction = this.parent.Faction;
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					compHatcher.hatcheeParent = pawn;
				}
				if (this.fertilizedBy != null)
				{
					compHatcher.otherParent = this.fertilizedBy;
				}
			}
			return thing;
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x001B2FDC File Offset: 0x001B11DC
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			string text = "EggProgress".Translate() + ": " + this.eggProgress.ToStringPercent();
			if (this.fertilizationCount > 0)
			{
				text += "\n" + "Fertilized".Translate();
			}
			else if (this.ProgressStoppedBecauseUnfertilized)
			{
				text += "\n" + "ProgressStoppedUntilFertilized".Translate();
			}
			return text;
		}

		// Token: 0x04002CEE RID: 11502
		private float eggProgress;

		// Token: 0x04002CEF RID: 11503
		private int fertilizationCount;

		// Token: 0x04002CF0 RID: 11504
		private Pawn fertilizedBy;
	}
}
