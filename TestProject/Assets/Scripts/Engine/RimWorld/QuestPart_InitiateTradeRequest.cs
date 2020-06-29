using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_InitiateTradeRequest : QuestPart
	{
		
		// (get) Token: 0x06003970 RID: 14704 RVA: 0x00131755 File Offset: 0x0012F955
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

		
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x00131765 File Offset: 0x0012F965
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

		
		// (get) Token: 0x06003972 RID: 14706 RVA: 0x00131775 File Offset: 0x0012F975
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.n__2())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.requestedThingDef, -1);
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null)
				{
					if (component.ActiveRequest)
					{
						Log.Error("Settlement " + this.settlement.Label + " already has an active trade request.", false);
						return;
					}
					component.requestThingDef = this.requestedThingDef;
					component.requestCount = this.requestedCount;
					component.expiration = Find.TickManager.TicksGame + this.requestDuration;
				}
			}
		}

		
		public override void Cleanup()
		{
			base.Cleanup();
			if (!this.keepAfterQuestEnds)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<ThingDef>(ref this.requestedThingDef, "requestedThingDef");
			Scribe_Values.Look<int>(ref this.requestedCount, "requestedCount", 0, false);
			Scribe_Values.Look<int>(ref this.requestDuration, "requestDuration", 0, false);
			Scribe_Values.Look<bool>(ref this.keepAfterQuestEnds, "keepAfterQuestEnds", false, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && !component.ActiveRequest && x.Faction != Faction.OfPlayer;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
			this.requestedThingDef = ThingDefOf.Silver;
			this.requestedCount = 100;
			this.requestDuration = 60000;
		}

		
		public string inSignal;

		
		public Settlement settlement;

		
		public ThingDef requestedThingDef;

		
		public int requestedCount;

		
		public int requestDuration;

		
		public bool keepAfterQuestEnds;
	}
}
