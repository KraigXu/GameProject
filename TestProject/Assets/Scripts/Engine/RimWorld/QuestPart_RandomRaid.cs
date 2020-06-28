using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public class QuestPart_RandomRaid : QuestPart
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x00136BF8 File Offset: 0x00134DF8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x00136C08 File Offset: 0x00134E08
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__1())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00136C18 File Offset: 0x00134E18
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.forced = true;
				incidentParms.quest = this.quest;
				incidentParms.target = this.mapParent.Map;
				incidentParms.points = (this.useCurrentThreatPoints ? StorytellerUtility.DefaultThreatPointsNow(this.mapParent.Map) : this.pointsRange.RandomInRange);
				incidentParms.faction = this.faction;
				IncidentDef incidentDef;
				if (this.faction == null || this.faction.HostileTo(Faction.OfPlayer))
				{
					incidentDef = IncidentDefOf.RaidEnemy;
				}
				else
				{
					incidentDef = IncidentDefOf.RaidFriendly;
				}
				if (incidentDef.Worker.CanFireNow(incidentParms, true))
				{
					incidentDef.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00136D04 File Offset: 0x00134F04
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<FloatRange>(ref this.pointsRange, "pointsRange", default(FloatRange), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.useCurrentThreatPoints, "useCurrentThreatPoints", false, false);
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00136D78 File Offset: 0x00134F78
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.pointsRange = new FloatRange(500f, 1500f);
			}
		}

		// Token: 0x040022A6 RID: 8870
		public string inSignal;

		// Token: 0x040022A7 RID: 8871
		public MapParent mapParent;

		// Token: 0x040022A8 RID: 8872
		public FloatRange pointsRange;

		// Token: 0x040022A9 RID: 8873
		public Faction faction;

		// Token: 0x040022AA RID: 8874
		public bool useCurrentThreatPoints;
	}
}
