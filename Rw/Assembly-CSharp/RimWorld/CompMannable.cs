using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D1E RID: 3358
	public class CompMannable : ThingComp
	{
		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x060051B3 RID: 20915 RVA: 0x001B5B55 File Offset: 0x001B3D55
		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x060051B4 RID: 20916 RVA: 0x001B5B80 File Offset: 0x001B3D80
		public Pawn ManningPawn
		{
			get
			{
				if (!this.MannedNow)
				{
					return null;
				}
				return this.lastManPawn;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060051B5 RID: 20917 RVA: 0x001B5B92 File Offset: 0x001B3D92
		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x001B5B9F File Offset: 0x001B3D9F
		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001B5BC9 File Offset: 0x001B3DC9
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (!pawn.RaceProps.ToolUser)
			{
				yield break;
			}
			if (!pawn.CanReserveAndReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
			{
				yield break;
			}
			if (this.Props.manWorkType != WorkTags.None && pawn.WorkTagIsDisabled(this.Props.manWorkType))
			{
				if (this.Props.manWorkType == WorkTags.Violent)
				{
					yield return new FloatMenuOption("CannotManThing".Translate(this.parent.LabelShort, this.parent) + " (" + "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				yield break;
			}
			FloatMenuOption floatMenuOption = new FloatMenuOption("OrderManThing".Translate(this.parent.LabelShort, this.parent), delegate
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, this.parent);
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x04002D25 RID: 11557
		private int lastManTick = -1;

		// Token: 0x04002D26 RID: 11558
		private Pawn lastManPawn;
	}
}
