using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	
	public class PriorityWork : IExposable
	{
		
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00013734 File Offset: 0x00011934
		public bool IsPrioritized
		{
			get
			{
				if (this.prioritizedCell.IsValid)
				{
					if (Find.TickManager.TicksGame < this.prioritizeTick + 30000)
					{
						return true;
					}
					this.Clear();
				}
				return false;
			}
		}

		
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00013764 File Offset: 0x00011964
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001376C File Offset: 0x0001196C
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		
		public PriorityWork()
		{
		}

		
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		
		public IEnumerable<Gizmo> GetGizmos()
		{
			if ((this.IsPrioritized || (this.pawn.CurJob != null && this.pawn.CurJob.playerForced) || this.pawn.jobs.jobQueue.AnyPlayerForced) && !this.pawn.Drafted)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandClearPrioritizedWork".Translate(),
					defaultDesc = "CommandClearPrioritizedWorkDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate
					{
						this.ClearPrioritizedWorkAndJobQueue();
						if (this.pawn.CurJob.playerForced)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					},
					hotKey = KeyBindingDefOf.Designator_Cancel,
					groupKey = 6165612
				};
			}
			yield break;
		}

		
		private Pawn pawn;

		
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		
		private WorkGiverDef prioritizedWorkGiver;

		
		private int prioritizeTick = Find.TickManager.TicksGame;

		
		private const int Timeout = 30000;
	}
}
