using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Thought_PsychicHarmonizer : Thought_Memory
	{
		
		// (get) Token: 0x0600642A RID: 25642 RVA: 0x0022B3DC File Offset: 0x002295DC
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.harmonizer.pawn.Named("HARMONIZER")).CapitalizeFirst();
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Hediff>(ref this.harmonizer, "harmonizer", false);
		}

		
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			float num = base.MoodOffset();
			float num2 = Mathf.Lerp(-1f, 1f, this.harmonizer.pawn.needs.mood.CurLevel);
			float statValue = this.harmonizer.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
			return num * num2 * statValue;
		}

		
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		
		public override bool GroupsWith(Thought other)
		{
			Thought_PsychicHarmonizer thought_PsychicHarmonizer = other as Thought_PsychicHarmonizer;
			return thought_PsychicHarmonizer != null && base.GroupsWith(other) && thought_PsychicHarmonizer.harmonizer == this.harmonizer;
		}

		
		// (get) Token: 0x0600642F RID: 25647 RVA: 0x0022B4E8 File Offset: 0x002296E8
		public override bool ShouldDiscard
		{
			get
			{
				Pawn pawn = this.harmonizer.pawn;
				return pawn.health.Dead || pawn.needs == null || pawn.needs.mood == null || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false) || ((pawn.Spawned || this.pawn.Spawned || pawn.GetCaravan() != this.pawn.GetCaravan()) && (!pawn.Spawned || !this.pawn.Spawned || pawn.Map != this.pawn.Map || pawn.Position.DistanceTo(this.pawn.Position) > this.harmonizer.TryGetComp<HediffComp_PsychicHarmonizer>().Props.range));
			}
		}

		
		public Hediff harmonizer;
	}
}
