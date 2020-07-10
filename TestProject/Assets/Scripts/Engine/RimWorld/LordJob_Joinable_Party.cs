using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_Joinable_Party : LordJob_Joinable_Gathering
	{
		
		
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		
		
		protected virtual ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		
		
		protected virtual TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		
		
		protected virtual ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		
		
		protected virtual TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		
		
		public int DurationTicks
		{
			get
			{
				return this.durationTicks;
			}
		}

		
		public LordJob_Joinable_Party()
		{
		}

		
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
			this.durationTicks = Rand.RangeInclusive(5000, 15000);
		}

		
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingParty".Translate();
		}

		
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Party(spot, gatheringDef, 3.5E-05f);
		}

		
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil party = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(party);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(party, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.ShouldBeCalledOff(), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
			transition.AddPreAction(new TransitionAction_Custom(CallApplyOutcome));
			transition.AddPreAction(new TransitionAction_Message(this.gatheringDef.calledOffMessage, MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = this.GetTimeoutTrigger();
			Transition transition2 = new Transition(party, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(CallApplyOutcome));
			transition2.AddPreAction(new TransitionAction_Message(this.gatheringDef.finishedMessage, MessageTypeDefOf.SituationResolved, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition2, false);

			void CallApplyOutcome()
            {
				this.ApplyOutcome((LordToil_Party)party);
			}

			return stateGraph;
		}

		
		protected virtual Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassed(this.durationTicks);
		}

		
		private void ApplyOutcome(LordToil_Party toil)
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			LordToilData_Party lordToilData_Party = (LordToilData_Party)toil.data;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				bool flag = pawn == this.organizer;
				int num;
				if (lordToilData_Party.presentForTicks.TryGetValue(pawn, out num) && num > 0)
				{
					if (ownedPawns[i].needs.mood != null)
					{
						ThoughtDef thoughtDef = flag ? this.OrganizerThought : this.AttendeeThought;
						float num2 = 0.5f / thoughtDef.stages[0].baseMoodEffect;
						float moodPowerFactor = Mathf.Min((float)num / (float)this.durationTicks + num2, 1f);
						Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
						thought_Memory.moodPowerFactor = moodPowerFactor;
						ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					}
					TaleRecorder.RecordTale(flag ? this.OrganizerTale : this.AttendeeTale, new object[]
					{
						ownedPawns[i],
						this.organizer
					});
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.gatheringDef == null)
			{
				this.gatheringDef = GatheringDefOf.Party;
			}
		}

		
		private int durationTicks;
	}
}
