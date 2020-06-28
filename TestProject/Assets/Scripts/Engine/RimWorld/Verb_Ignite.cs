using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001030 RID: 4144
	public class Verb_Ignite : Verb
	{
		// Token: 0x06006321 RID: 25377 RVA: 0x00227283 File Offset: 0x00225483
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x06006322 RID: 25378 RVA: 0x00227298 File Offset: 0x00225498
		protected override bool TryCastShot()
		{
			Thing thing = this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			FireUtility.TryStartFireIn(thing.OccupiedRect().ClosestCellTo(casterPawn.Position), casterPawn.Map, 0.3f);
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			return true;
		}
	}
}
