using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F8 RID: 1784
	public abstract class JoyGiver
	{
		// Token: 0x06002F39 RID: 12089 RVA: 0x001098A9 File Offset: 0x00107AA9
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x001098B8 File Offset: 0x00107AB8
		protected virtual void GetSearchSet(Pawn pawn, List<Thing> outCandidates)
		{
			outCandidates.Clear();
			if (this.def.thingDefs == null)
			{
				return;
			}
			if (this.def.thingDefs.Count == 1)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]));
				return;
			}
			for (int i = 0; i < this.def.thingDefs.Count; i++)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
			}
		}

		// Token: 0x06002F3B RID: 12091
		public abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06002F3C RID: 12092 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatherSpot)
		{
			return null;
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x00109956 File Offset: 0x00107B56
		public virtual bool CanBeGivenTo(Pawn pawn)
		{
			return this.MissingRequiredCapacity(pawn) == null && this.def.joyKind.PawnCanDo(pawn);
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x00109974 File Offset: 0x00107B74
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04001AAD RID: 6829
		public JoyGiverDef def;
	}
}
