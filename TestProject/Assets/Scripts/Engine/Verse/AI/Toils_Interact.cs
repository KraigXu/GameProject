using System;

namespace Verse.AI
{
	// Token: 0x02000532 RID: 1330
	internal class Toils_Interact
	{
		// Token: 0x06002621 RID: 9761 RVA: 0x000E1384 File Offset: 0x000DF584
		public static Toil DestroyThing(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (!thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			};
			return toil;
		}
	}
}
