using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0D RID: 3085
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x0018EB4F File Offset: 0x0018CD4F
		private float MaxSeverity
		{
			get
			{
				if (this.hediff.lethalSeverity <= 0f)
				{
					return 1f;
				}
				return this.hediff.lethalSeverity * 0.99f;
			}
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x0018EB7C File Offset: 0x0018CD7C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.hediff.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<HediffDef>(this.PossibleHediffs(), (HediffDef hd) => hd.LabelCap, (HediffDef hd) => delegate
				{
					this.hediff = hd;
					if (this.severityRange.max > this.MaxSeverity)
					{
						this.severityRange.max = this.MaxSeverity;
					}
					if (this.severityRange.min > this.MaxSeverity)
					{
						this.severityRange.min = this.MaxSeverity;
					}
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.severityRange, 0f, this.MaxSeverity, "ConfigurableSeverity", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x0018EC63 File Offset: 0x0018CE63
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x0018EC90 File Offset: 0x0018CE90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x0018ECD0 File Offset: 0x0018CED0
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x0018ED24 File Offset: 0x0018CF24
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x0018ED98 File Offset: 0x0018CF98
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_ForcedHediff.chance);
				return true;
			}
			return false;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x0018EDD8 File Offset: 0x0018CFD8
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			if (!base.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				return false;
			}
			if (this.hideOffMap)
			{
				if (!req.AllowDead && pawn.health.WouldDieAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
				if (!req.AllowDowned && pawn.health.WouldBeDownedAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x0018EE4F File Offset: 0x0018D04F
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x0018EE4F File Offset: 0x0018D04F
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x0018EE58 File Offset: 0x0018D058
		private void AddHediff(Pawn p)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x040029E7 RID: 10727
		private HediffDef hediff;

		// Token: 0x040029E8 RID: 10728
		private FloatRange severityRange;
	}
}
