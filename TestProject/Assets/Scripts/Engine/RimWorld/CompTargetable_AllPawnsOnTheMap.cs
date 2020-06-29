using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			if (this.parent.MapHeld == null)
			{
				yield break;
			}
			TargetingParameters tp = this.GetTargetingParameters();
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (tp.CanTarget(pawn))
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}
	}
}
