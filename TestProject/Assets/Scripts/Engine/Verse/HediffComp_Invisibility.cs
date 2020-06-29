using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_Invisibility : HediffComp
	{
		
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.UpdateTarget();
		}

		
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		
		private void UpdateTarget()
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			PortraitsCache.SetDirty(pawn);
		}
	}
}
