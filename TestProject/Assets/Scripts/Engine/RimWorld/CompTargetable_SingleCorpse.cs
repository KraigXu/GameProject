using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8D RID: 3469
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06005485 RID: 21637 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x001C34A1 File Offset: 0x001C16A1
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

		// Token: 0x06005487 RID: 21639 RVA: 0x001C34D6 File Offset: 0x001C16D6
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
