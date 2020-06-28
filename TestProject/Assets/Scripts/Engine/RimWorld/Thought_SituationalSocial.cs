using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD5 RID: 3029
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060047E1 RID: 18401 RVA: 0x00185A8E File Offset: 0x00183C8E
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060047E2 RID: 18402 RVA: 0x00185AAC File Offset: 0x00183CAC
		public override string LabelCap
		{
			get
			{
				if (this.def.Worker == null)
				{
					return base.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				return base.LabelCap;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060047E3 RID: 18403 RVA: 0x00185B04 File Offset: 0x00183D04
		public override string LabelCapSocial
		{
			get
			{
				if (base.CurStage.labelSocial != null)
				{
					return base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				return base.LabelCapSocial;
			}
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x00185B5A File Offset: 0x00183D5A
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x00185B64 File Offset: 0x00183D64
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.CurStage.baseOpinionOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true) * this.otherPawn.GetStatValue(this.def.effectMultiplyingStat, true);
			}
			return num;
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x00185BD8 File Offset: 0x00183DD8
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x00185C0A File Offset: 0x00183E0A
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x0400293D RID: 10557
		public Pawn otherPawn;
	}
}
