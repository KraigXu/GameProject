using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x0010E01A File Offset: 0x0010C21A
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x0010E022 File Offset: 0x0010C222
		public virtual int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return -1;
			}
		}

		
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			yield break;
		}

		
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		
		public virtual string PostProcessedGerund(Job job)
		{
			return this.def.gerund;
		}

		
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}
	}
}
