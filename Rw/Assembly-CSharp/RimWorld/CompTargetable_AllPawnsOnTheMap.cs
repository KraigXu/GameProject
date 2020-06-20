using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8C RID: 3468
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x001C3453 File Offset: 0x001C1653
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x001C347A File Offset: 0x001C167A
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
