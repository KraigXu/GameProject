using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B77 RID: 2935
	public class Pawn_DraftController : IExposable
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x060044B1 RID: 17585 RVA: 0x0017333F File Offset: 0x0017153F
		// (set) Token: 0x060044B2 RID: 17586 RVA: 0x00173348 File Offset: 0x00171548
		public bool Drafted
		{
			get
			{
				return this.draftedInt;
			}
			set
			{
				if (value == this.draftedInt)
				{
					return;
				}
				this.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
				this.fireAtWillInt = true;
				this.draftedInt = value;
				if (!value && this.pawn.Spawned)
				{
					this.pawn.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this.pawn);
				}
				this.pawn.jobs.ClearQueuedJobs(true);
				if (this.pawn.jobs.curJob != null && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				if (this.draftedInt)
				{
					Lord lord = this.pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.Drafted, null);
					}
					this.autoUndrafter.Notify_Drafted();
				}
				else if (this.pawn.playerSettings != null)
				{
					this.pawn.playerSettings.animalsReleased = false;
				}
				foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					pawn.jobs.Notify_MasterDraftedOrUndrafted();
				}
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x001734A4 File Offset: 0x001716A4
		// (set) Token: 0x060044B4 RID: 17588 RVA: 0x001734AC File Offset: 0x001716AC
		public bool FireAtWill
		{
			get
			{
				return this.fireAtWillInt;
			}
			set
			{
				this.fireAtWillInt = value;
				if (!this.fireAtWillInt && this.pawn.stances.curStance is Stance_Warmup)
				{
					this.pawn.stances.CancelBusyStanceSoft();
				}
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x001734E4 File Offset: 0x001716E4
		public Pawn_DraftController(Pawn pawn)
		{
			this.pawn = pawn;
			this.autoUndrafter = new AutoUndrafter(pawn);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x00173508 File Offset: 0x00171708
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.draftedInt, "drafted", false, false);
			Scribe_Values.Look<bool>(ref this.fireAtWillInt, "fireAtWill", true, false);
			Scribe_Deep.Look<AutoUndrafter>(ref this.autoUndrafter, "autoUndrafter", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x00173558 File Offset: 0x00171758
		public void DraftControllerTick()
		{
			this.autoUndrafter.AutoUndraftTick();
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x00173565 File Offset: 0x00171765
		internal IEnumerable<Gizmo> GetGizmos()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_ColonistDraft;
			command_Toggle.isActive = (() => this.Drafted);
			command_Toggle.toggleAction = delegate
			{
				this.Drafted = !this.Drafted;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
				if (this.Drafted)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
				}
			};
			command_Toggle.defaultDesc = "CommandToggleDraftDesc".Translate();
			command_Toggle.icon = TexCommand.Draft;
			command_Toggle.turnOnSound = SoundDefOf.DraftOn;
			command_Toggle.turnOffSound = SoundDefOf.DraftOff;
			if (!this.Drafted)
			{
				command_Toggle.defaultLabel = "CommandDraftLabel".Translate();
			}
			if (this.pawn.Downed)
			{
				command_Toggle.Disable("IsIncapped".Translate(this.pawn.LabelShort, this.pawn));
			}
			if (!this.Drafted)
			{
				command_Toggle.tutorTag = "Draft";
			}
			else
			{
				command_Toggle.tutorTag = "Undraft";
			}
			yield return command_Toggle;
			if (this.Drafted && this.pawn.equipment.Primary != null && this.pawn.equipment.Primary.def.IsRangedWeapon)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Misc6,
					isActive = (() => this.FireAtWill),
					toggleAction = delegate
					{
						this.FireAtWill = !this.FireAtWill;
					},
					icon = TexCommand.FireAtWill,
					defaultLabel = "CommandFireAtWillLabel".Translate(),
					defaultDesc = "CommandFireAtWillDesc".Translate(),
					tutorTag = "FireAtWillToggle"
				};
			}
			yield break;
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x00173575 File Offset: 0x00171775
		internal void Notify_PrimaryWeaponChanged()
		{
			this.fireAtWillInt = true;
		}

		// Token: 0x0400273C RID: 10044
		public Pawn pawn;

		// Token: 0x0400273D RID: 10045
		private bool draftedInt;

		// Token: 0x0400273E RID: 10046
		private bool fireAtWillInt = true;

		// Token: 0x0400273F RID: 10047
		private AutoUndrafter autoUndrafter;
	}
}
