using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000973 RID: 2419
	public class QuestPart_DisableTradeRequest : QuestPart
	{
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x00131033 File Offset: 0x0012F233
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
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

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x00131043 File Offset: 0x0012F243
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__1())
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

		// Token: 0x0600394F RID: 14671 RVA: 0x00131054 File Offset: 0x0012F254
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x00131098 File Offset: 0x0012F298
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x001310C4 File Offset: 0x0012F2C4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && component.ActiveRequest;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
		}

		// Token: 0x040021C6 RID: 8646
		public string inSignal;

		// Token: 0x040021C7 RID: 8647
		public Settlement settlement;
	}
}
