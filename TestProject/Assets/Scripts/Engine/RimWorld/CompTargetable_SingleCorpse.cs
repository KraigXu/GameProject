using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		
		// (get) Token: 0x06005485 RID: 21637 RVA: 0x0001028D File Offset: 0x0000E48D
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
				canTargetPawns = false,
				canTargetBuildings = false,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = ((TargetInfo x) => x.Thing is Corpse && base.BaseTargetValidator(x.Thing))
			};
		}

		
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
