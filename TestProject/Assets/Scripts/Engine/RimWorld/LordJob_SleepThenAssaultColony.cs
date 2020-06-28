using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000776 RID: 1910
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000F4066 File Offset: 0x000F2266
		public LordJob_SleepThenAssaultColony()
		{
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x001160E2 File Offset: 0x001142E2
		public LordJob_SleepThenAssaultColony(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x001160F4 File Offset: 0x001142F4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					vector += this.lord.ownedPawns[i].Position.ToVector3();
				}
				vector /= (float)this.lord.ownedPawns.Count;
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x001161D4 File Offset: 0x001143D4
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04001B37 RID: 6967
		private Faction faction;
	}
}
