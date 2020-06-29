using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompWakeUpDormant : ThingComp
	{
		
		
		private CompProperties_WakeUpDormant Props
		{
			get
			{
				return (CompProperties_WakeUpDormant)this.props;
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpIfColonistClose = this.Props.wakeUpIfAnyColonistClose;
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		
		public void TickRareWorker()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.wakeUpIfColonistClose)
			{
				int num = GenRadial.NumCellsInRadius(this.Props.anyColonistCloseCheckRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map, false, null, 0, 0))
					{
						foreach (Thing thing in intVec.GetThingList(this.parent.Map))
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null && pawn.IsColonist)
							{
								this.Activate(true, false);
								return;
							}
						}
					}
				}
			}
			if (this.Props.wakeUpOnThingConstructedRadius > 0f)
			{
				if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.wakeUpOnThingConstructedRadius, (Thing t) => t.Faction == Faction.OfPlayer, null, 0, -1, false, RegionType.Set_Passable, false) != null)
				{
					this.Activate(true, false);
				}
			}
		}

		
		public void Activate(bool sendSignal = true, bool silent = false)
		{
			if (sendSignal && !this.sentSignal)
			{
				if (!string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
				{
					if (this.Props.onlyWakeUpSameFaction)
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT"), this.parent.Faction.Named("FACTION")));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT")));
					}
				}
				if (!silent && this.parent.Spawned && this.Props.wakeUpSound != null)
				{
					this.Props.wakeUpSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				this.sentSignal = true;
			}
			CompCanBeDormant compCanBeDormant = this.parent.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				compCanBeDormant.WakeUp();
			}
		}

		
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.wakeUpOnDamage && totalDamageDealt > 0f && dinfo.Def.ExternalViolenceFor(this.parent))
			{
				this.Activate(true, false);
			}
		}

		
		//public override void Notify_SignalReceived(Signal signal)
		//{
		//	if (string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
		//	{
		//		return;
		//	}
		//	this.sentSignal = true;
		//}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.wakeUpIfColonistClose, "wakeUpIfColonistClose", false, false);
			Scribe_Values.Look<bool>(ref this.sentSignal, "sentSignal", false, false);
		}

		
		public bool wakeUpIfColonistClose;

		
		private bool sentSignal;
	}
}
