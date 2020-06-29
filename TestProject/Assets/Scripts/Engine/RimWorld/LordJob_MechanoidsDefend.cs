using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_MechanoidsDefend : LordJob_MechanoidDefendBase
	{
		
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		
		public LordJob_MechanoidsDefend()
		{
		}

		
		public LordJob_MechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			this.things.AddRange(things);
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		
		public LordJob_MechanoidsDefend(SpawnedPawnParams parms)
		{
			this.things.Add(parms.spawnerThing);
			this.faction = parms.spawnerThing.Faction;
			this.defendRadius = parms.defendRadius;
			this.defSpot = parms.defSpot;
			this.canAssaultColony = false;
		}

		
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!this.defSpot.IsValid)
			{
				Log.Warning("LordJob_MechanoidsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.", false);
				stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph());
				return stateGraph;
			}
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, this.defendRadius);
			stateGraph.StartingToil = lordToil_DefendPoint;
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false);
			stateGraph.AddToil(lordToil_AssaultColony);
			if (this.canAssaultColony)
			{
				LordToil_AssaultColony lordToil_AssaultColony2 = new LordToil_AssaultColony(false);
				stateGraph.AddToil(lordToil_AssaultColony2);
				Transition transition = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition.AddSource(lordToil_AssaultColony2);
				transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
				transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
				transition2.AddTrigger(new Trigger_PawnLostViolently(true));
				transition2.AddTrigger(new Trigger_Memo(LordJob_MechanoidsDefend.MemoDamaged));
				transition2.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition2, false);
				Transition transition3 = new Transition(lordToil_AssaultColony2, lordToil_DefendPoint, false, true);
				transition3.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1380, new string[]
				{
					LordJob_MechanoidsDefend.MemoDamaged
				}));
				transition3.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
				stateGraph.AddTransition(transition3, false);
				Transition transition4 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition4.AddSource(lordToil_AssaultColony2);
				transition4.AddTrigger(new Trigger_AnyThingDamageTaken(this.things, 0.5f));
				transition4.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
				stateGraph.AddTransition(transition4, false);
			}
			Transition transition5 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
			transition5.AddTrigger(new Trigger_ChanceOnSignal(TriggerSignalType.MechClusterDefeated, 1f));
			stateGraph.AddTransition(transition5, false);
			if (!this.isMechCluster)
			{
				Transition transition6 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition6.AddTrigger(new Trigger_AnyThingDamageTaken(this.things, 1f));
				stateGraph.AddTransition(transition6, false);
			}
			return stateGraph;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shipPart, "shipPart", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				if (this.shipPart != null)
				{
					if (this.things == null)
					{
						this.things = new List<Thing>();
					}
					this.things.Add(this.shipPart);
					this.shipPart = null;
				}
			}
		}

		
		private Thing shipPart;

		
		public static readonly string MemoDamaged = "ShipPartDamaged";
	}
}
