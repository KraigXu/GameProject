﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		
		
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

		
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		
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

		
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		
		private void AddHediff(Pawn p)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		
		private HediffDef hediff;

		
		private FloatRange severityRange;
	}
}
