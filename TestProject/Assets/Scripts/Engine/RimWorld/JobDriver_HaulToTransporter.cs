using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_HaulToTransporter : JobDriver_HaulToContainer
	{
		
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x000FDB19 File Offset: 0x000FBD19
		public CompTransporter Transporter
		{
			get
			{
				if (base.Container == null)
				{
					return null;
				}
				return base.Container.TryGetComp<CompTransporter>();
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			ThingCount thingCount;
			if (this.job.targetA.IsValid)
			{
				thingCount = new ThingCount(this.job.targetA.Thing, this.job.targetA.Thing.stackCount);
			}
			else
			{
				thingCount = LoadTransportersJobUtility.FindThingToLoad(this.pawn, base.Container.TryGetComp<CompTransporter>());
			}
			this.job.targetA = thingCount.Thing;
			this.job.count = thingCount.Count;
			this.initialCount = thingCount.Count;
			this.pawn.Reserve(thingCount.Thing, this.job, 1, -1, null, true);
		}

		
		public int initialCount;
	}
}
