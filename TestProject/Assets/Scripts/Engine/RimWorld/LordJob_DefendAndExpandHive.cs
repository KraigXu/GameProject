﻿using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_DefendAndExpandHive : LordJob
	{
		
		
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		
		
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		
		public LordJob_DefendAndExpandHive()
		{
		}

		
		public LordJob_DefendAndExpandHive(SpawnedPawnParams parms)
		{
			this.aggressive = parms.aggressive;
		}

		
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendAndExpandHive lordToil_DefendAndExpandHive = new LordToil_DefendAndExpandHive();
			lordToil_DefendAndExpandHive.distToHiveToAttack = 10f;
			stateGraph.StartingToil = lordToil_DefendAndExpandHive;
			LordToil_DefendHiveAggressively lordToil_DefendHiveAggressively = new LordToil_DefendHiveAggressively();
			lordToil_DefendHiveAggressively.distToHiveToAttack = 40f;
			stateGraph.AddToil(lordToil_DefendHiveAggressively);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendAndExpandHive, this.aggressive ? lordToil_AssaultColony : lordToil_DefendHiveAggressively, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
			transition.AddTrigger(new Trigger_PawnLostViolently(false));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoAttackedByEnemy));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoBurnedBadly));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoDestroyedNonRoofCollapse));
			transition.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendAndExpandHive, lordToil_AssaultColony, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendHiveAggressively, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_DefendAndExpandHive, lordToil_DefendAndExpandHive, true, true);
			transition4.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_DefendHiveAggressively, lordToil_DefendHiveAggressively, true, true);
			transition5.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_AssaultColony, lordToil_DefendAndExpandHive, false, true);
			transition6.AddSource(lordToil_DefendHiveAggressively);
			transition6.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1200, new string[]
			{
				Hive.MemoAttackedByEnemy,
				Hive.MemoBurnedBadly,
				Hive.MemoDestroyedNonRoofCollapse,
				Hive.MemoDeSpawned,
				HediffGiver_Heat.MemoPawnBurnedByAir
			}));
			transition6.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
		}

		
		private bool aggressive;
	}
}
