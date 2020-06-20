using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8E RID: 3470
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x0600548A RID: 21642 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x001C3505 File Offset: 0x001C1705
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x001C352C File Offset: 0x001C172C
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
