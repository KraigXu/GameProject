using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD2 RID: 3026
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060047CB RID: 18379 RVA: 0x00185705 File Offset: 0x00183905
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x00185724 File Offset: 0x00183924
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x00185740 File Offset: 0x00183940
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060047CE RID: 18382 RVA: 0x00185756 File Offset: 0x00183956
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x00185773 File Offset: 0x00183973
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

		// Token: 0x060047D0 RID: 18384 RVA: 0x001857A9 File Offset: 0x001839A9
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x001857B1 File Offset: 0x001839B1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x001857CF File Offset: 0x001839CF
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x001857E8 File Offset: 0x001839E8
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x001857F0 File Offset: 0x001839F0
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		// Token: 0x04002939 RID: 10553
		public float opinionOffset;
	}
}
