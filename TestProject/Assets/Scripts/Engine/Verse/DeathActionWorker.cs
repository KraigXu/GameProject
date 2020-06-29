using System;
using RimWorld;

namespace Verse
{
	
	public abstract class DeathActionWorker
	{
		
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001907E File Offset: 0x0001727E
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00010306 File Offset: 0x0000E506
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
