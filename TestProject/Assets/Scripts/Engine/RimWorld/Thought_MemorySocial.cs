using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		
		// (get) Token: 0x060047CB RID: 18379 RVA: 0x00185705 File Offset: 0x00183905
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x00185724 File Offset: 0x00183924
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x00185740 File Offset: 0x00183940
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		
		// (get) Token: 0x060047CE RID: 18382 RVA: 0x00185756 File Offset: 0x00183956
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return this.opinionOffset * this.AgeFactor;
		}

		
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		
		public float opinionOffset;
	}
}
