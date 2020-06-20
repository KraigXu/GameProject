using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000056 RID: 86
	public class PriorityWork : IExposable
	{
		// Token: 0x170000A4 RID: 164
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00013764 File Offset: 0x00011964
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001376C File Offset: 0x0001196C
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00013774 File Offset: 0x00011974
		public PriorityWork()
		{
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00013797 File Offset: 0x00011997
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000137C4 File Offset: 0x000119C4
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001380D File Offset: 0x00011A0D
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001382D File Offset: 0x00011A2D
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00013848 File Offset: 0x00011A48
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00013861 File Offset: 0x00011A61
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

		// Token: 0x04000114 RID: 276
		private Pawn pawn;

		// Token: 0x04000115 RID: 277
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x04000116 RID: 278
		private WorkGiverDef prioritizedWorkGiver;

		// Token: 0x04000117 RID: 279
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x04000118 RID: 280
		private const int Timeout = 30000;
	}
}
