using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000778 RID: 1912
	public class LordJob_StageThenAttack : LordJob
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060031F2 RID: 12786 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_StageThenAttack()
		{
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x00116426 File Offset: 0x00114626
		public LordJob_StageThenAttack(Faction faction, IntVec3 stageLoc, int raidSeed)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.raidSeed = raidSeed;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x00116444 File Offset: 0x00114644
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			int tickLimit = Rand.RangeSeeded(5000, 15000, this.raidSeed);
			Transition transition = new Transition(lordToil_Stage, startingToil, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(tickLimit));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), MessageTypeDefOf.ThreatBig, "MessageRaidersBeginningAssault-" + this.raidSeed, 1f));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition, false);
			stateGraph.transitions.Find((Transition x) => x.triggers.Any((Trigger y) => y is Trigger_BecameNonHostileToPlayer)).AddSource(lordToil_Stage);
			return stateGraph;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0011656C File Offset: 0x0011476C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.raidSeed, "raidSeed", 0, false);
		}

		// Token: 0x04001B38 RID: 6968
		private Faction faction;

		// Token: 0x04001B39 RID: 6969
		private IntVec3 stageLoc;

		// Token: 0x04001B3A RID: 6970
		private int raidSeed;
	}
}
