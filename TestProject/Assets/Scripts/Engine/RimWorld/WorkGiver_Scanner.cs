using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071B RID: 1819
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x0010E01A File Offset: 0x0010C21A
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x0010E022 File Offset: 0x0010C222
		public virtual int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x0010E025 File Offset: 0x0010C225
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			yield break;
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x0010E02E File Offset: 0x0010C22E
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x0010E036 File Offset: 0x0010C236
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x0010E044 File Offset: 0x0010C244
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x0010E052 File Offset: 0x0010C252
		public virtual string PostProcessedGerund(Job job)
		{
			return this.def.gerund;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x0010E05F File Offset: 0x0010C25F
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}
	}
}
