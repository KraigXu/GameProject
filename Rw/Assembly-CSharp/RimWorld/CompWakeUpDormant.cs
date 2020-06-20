using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D7B RID: 3451
	public class CompWakeUpDormant : ThingComp
	{
		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005412 RID: 21522 RVA: 0x001C1284 File Offset: 0x001BF484
		private CompProperties_WakeUpDormant Props
		{
			get
			{
				return (CompProperties_WakeUpDormant)this.props;
			}
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x001C1291 File Offset: 0x001BF491
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpIfColonistClose = this.Props.wakeUpIfAnyColonistClose;
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x001C12AB File Offset: 0x001BF4AB
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x001C12CC File Offset: 0x001BF4CC
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

		// Token: 0x06005416 RID: 21526 RVA: 0x001C1448 File Offset: 0x001BF648
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

		// Token: 0x06005417 RID: 21527 RVA: 0x001C155A File Offset: 0x001BF75A
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.wakeUpOnDamage && totalDamageDealt > 0f && dinfo.Def.ExternalViolenceFor(this.parent))
			{
				this.Activate(true, false);
			}
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x001C158D File Offset: 0x001BF78D
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
			{
				return;
			}
			this.sentSignal = true;
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x001C15A9 File Offset: 0x001BF7A9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.wakeUpIfColonistClose, "wakeUpIfColonistClose", false, false);
			Scribe_Values.Look<bool>(ref this.sentSignal, "sentSignal", false, false);
		}

		// Token: 0x04002E5D RID: 11869
		public bool wakeUpIfColonistClose;

		// Token: 0x04002E5E RID: 11870
		private bool sentSignal;
	}
}
