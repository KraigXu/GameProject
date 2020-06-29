using System;
using RimWorld;

namespace Verse
{
	
	public abstract class DeathActionWorker
	{
		
		
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		
		
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		
		public abstract void PawnDied(Corpse corpse);
	}
}
