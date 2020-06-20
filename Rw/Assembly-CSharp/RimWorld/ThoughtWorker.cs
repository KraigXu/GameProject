using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FA RID: 2042
	public abstract class ThoughtWorker
	{
		// Token: 0x060033F4 RID: 13300 RVA: 0x0011E45A File Offset: 0x0011C65A
		public virtual string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Named("PAWN"));
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x0011E45A File Offset: 0x0011C65A
		public virtual string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Named("PAWN"));
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x0011E472 File Offset: 0x0011C672
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0011E481 File Offset: 0x0011C681
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x0011E491 File Offset: 0x0011C691
		private ThoughtState PostProcessedState(ThoughtState state)
		{
			if (this.def.invert)
			{
				if (state.Active)
				{
					state = ThoughtState.Inactive;
				}
				else
				{
					state = ThoughtState.ActiveAtStage(0);
				}
			}
			return state;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0011E4BB File Offset: 0x0011C6BB
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x0011E4D7 File Offset: 0x0011C6D7
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float MoodMultiplier(Pawn p)
		{
			return 1f;
		}

		// Token: 0x04001BB1 RID: 7089
		public ThoughtDef def;
	}
}
