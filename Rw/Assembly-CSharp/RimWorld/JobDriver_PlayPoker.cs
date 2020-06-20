using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200064A RID: 1610
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		// Token: 0x06002BF8 RID: 11256 RVA: 0x000FC6DB File Offset: 0x000FA8DB
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
		}
	}
}
