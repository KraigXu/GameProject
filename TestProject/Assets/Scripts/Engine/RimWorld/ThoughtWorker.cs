using System;
using Verse;

namespace RimWorld
{
	
	public abstract class ThoughtWorker
	{
		
		public virtual string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Named("PAWN"));
		}

		
		public virtual string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Named("PAWN"));
		}

		
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		
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

		
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		
		public virtual float MoodMultiplier(Pawn p)
		{
			return 1f;
		}

		
		public ThoughtDef def;
	}
}
