﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_RandomRaid : QuestPart
	{
		
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x00136BF8 File Offset: 0x00134DF8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x00136C08 File Offset: 0x00134E08
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__1())
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<FloatRange>(ref this.pointsRange, "pointsRange", default(FloatRange), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.useCurrentThreatPoints, "useCurrentThreatPoints", false, false);
		}

		
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

		
		public string inSignal;

		
		public MapParent mapParent;

		
		public FloatRange pointsRange;

		
		public Faction faction;

		
		public bool useCurrentThreatPoints;
	}
}
