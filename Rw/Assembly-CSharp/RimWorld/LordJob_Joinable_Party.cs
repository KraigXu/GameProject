using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000780 RID: 1920
	public class LordJob_Joinable_Party : LordJob_Joinable_Gathering
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x00117D27 File Offset: 0x00115F27
		protected virtual ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x00117D2E File Offset: 0x00115F2E
		protected virtual TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003236 RID: 12854 RVA: 0x00117D27 File Offset: 0x00115F27
		protected virtual ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.AttendedParty;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x00117D2E File Offset: 0x00115F2E
		protected virtual TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.AttendedParty;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003238 RID: 12856 RVA: 0x00117D35 File Offset: 0x00115F35
		public int DurationTicks
		{
			get
			{
				return this.durationTicks;
			}
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x00117D3D File Offset: 0x00115F3D
		public LordJob_Joinable_Party()
		{
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x00117D45 File Offset: 0x00115F45
		public LordJob_Joinable_Party(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
			this.durationTicks = Rand.RangeInclusive(5000, 15000);
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x00117D65 File Offset: 0x00115F65
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingParty".Translate();
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x00117D76 File Offset: 0x00115F76
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Party(spot, gatheringDef, 3.5E-05f);
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x00117D84 File Offset: 0x00115F84
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
			transition.AddPreAction(new TransitionAction_Custom(delegate
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition.AddPreAction(new TransitionAction_Message(this.gatheringDef.calledOffMessage, MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = this.GetTimeoutTrigger();
			Transition transition2 = new Transition(party, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(delegate
			{
				this.ApplyOutcome((LordToil_Party)party);
			}));
			transition2.AddPreAction(new TransitionAction_Message(this.gatheringDef.finishedMessage, MessageTypeDefOf.SituationResolved, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x00117EE9 File Offset: 0x001160E9
		protected virtual Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassed(this.durationTicks);
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x00117EF8 File Offset: 0x001160F8
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

		// Token: 0x06003240 RID: 12864 RVA: 0x0011802B File Offset: 0x0011622B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.gatheringDef == null)
			{
				this.gatheringDef = GatheringDefOf.Party;
			}
		}

		// Token: 0x04001B47 RID: 6983
		private int durationTicks;
	}
}
