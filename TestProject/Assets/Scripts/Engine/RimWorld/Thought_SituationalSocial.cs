using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		
		// (get) Token: 0x060047E1 RID: 18401 RVA: 0x00185A8E File Offset: 0x00183C8E
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		
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

		
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		
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

		
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		
		public Pawn otherPawn;
	}
}
