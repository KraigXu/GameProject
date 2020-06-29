using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompTargetable_SinglePawn : CompTargetable
	{
		
		// (get) Token: 0x0600548A RID: 21642 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
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
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
