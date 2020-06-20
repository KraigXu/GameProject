using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107C RID: 4220
	public class Thought_PsychicHarmonizer : Thought_Memory
	{
		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x0600642A RID: 25642 RVA: 0x0022B3DC File Offset: 0x002295DC
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.harmonizer.pawn.Named("HARMONIZER")).CapitalizeFirst();
			}
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x0022B41B File Offset: 0x0022961B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Hediff>(ref this.harmonizer, "harmonizer", false);
		}

		// Token: 0x0600642C RID: 25644 RVA: 0x0022B434 File Offset: 0x00229634
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

		// Token: 0x0600642D RID: 25645 RVA: 0x001857E8 File Offset: 0x001839E8
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x0022B4B4 File Offset: 0x002296B4
		public override bool GroupsWith(Thought other)
		{
			Thought_PsychicHarmonizer thought_PsychicHarmonizer = other as Thought_PsychicHarmonizer;
			return thought_PsychicHarmonizer != null && base.GroupsWith(other) && thought_PsychicHarmonizer.harmonizer == this.harmonizer;
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600642F RID: 25647 RVA: 0x0022B4E8 File Offset: 0x002296E8
		public override bool ShouldDiscard
		{
			get
			{
				Pawn pawn = this.harmonizer.pawn;
				return pawn.health.Dead || pawn.needs == null || pawn.needs.mood == null || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false) || ((pawn.Spawned || this.pawn.Spawned || pawn.GetCaravan() != this.pawn.GetCaravan()) && (!pawn.Spawned || !this.pawn.Spawned || pawn.Map != this.pawn.Map || pawn.Position.DistanceTo(this.pawn.Position) > this.harmonizer.TryGetComp<HediffComp_PsychicHarmonizer>().Props.range));
			}
		}

		// Token: 0x04003CF4 RID: 15604
		public Hediff harmonizer;
	}
}
