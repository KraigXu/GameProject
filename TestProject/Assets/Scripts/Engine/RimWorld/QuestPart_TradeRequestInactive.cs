using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_TradeRequestInactive : QuestPartActivable
	{
		
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x0012F41A File Offset: 0x0012D61A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.settlement != null)
				{
					yield return this.settlement;
				}
				yield break;
				yield break;
			}
		}

		
		// (get) Token: 0x060038CB RID: 14539 RVA: 0x0012F42A File Offset: 0x0012D62A
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__1())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.settlement.Faction != null)
				{
					yield return this.settlement.Faction;
				}
				yield break;
				yield break;
			}
		}

		
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.settlement == null || !this.settlement.Spawned)
			{
				base.Complete();
				return;
			}
			TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
			if (component == null || !component.ActiveRequest)
			{
				base.Complete(this.settlement.Named("SUBJECT"));
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.settlement = Find.WorldObjects.Settlements.FirstOrDefault<Settlement>();
		}

		
		public Settlement settlement;
	}
}
