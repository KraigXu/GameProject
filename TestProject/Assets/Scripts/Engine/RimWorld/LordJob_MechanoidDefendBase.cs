using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	
	public abstract class LordJob_MechanoidDefendBase : LordJob
	{
		
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.isMechCluster && !this.mechClusterDefeated && !MechClusterUtility.AnyThreatBuilding(this.things))
			{
				this.OnDefeat();
			}
		}

		
		public override void Notify_LordDestroyed()
		{
			if (this.isMechCluster && !this.mechClusterDefeated)
			{
				this.OnDefeat();
			}
		}

		
		private void OnDefeat()
		{
			foreach (Thing thing in this.things)
			{
				thing.SetFaction(null, null);
				CompSendSignalOnPawnProximity compSendSignalOnPawnProximity = thing.TryGetComp<CompSendSignalOnPawnProximity>();
				if (compSendSignalOnPawnProximity != null)
				{
					compSendSignalOnPawnProximity.Expire();
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null)
				{
					compSendSignalOnCountdown.ticksLeft = 0;
				}
				ThingWithComps thingWithComps;
				if ((thingWithComps = (thing as ThingWithComps)) != null)
				{
					thingWithComps.BroadcastCompSignal("MechClusterDefeated");
				}
			}
			this.lord.Notify_MechClusterDefeated();
			for (int i = 0; i < this.thingsToNotifyOnDefeat.Count; i++)
			{
				this.thingsToNotifyOnDefeat[i].Notify_LordDestroyed();
			}
			this.mechClusterDefeated = true;
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DefeatedMechCluster, null);
					}
				}
			}
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "AllEnemiesDefeated");
			Messages.Message("MessageMechClusterDefeated".Translate(), new LookTargets(this.defSpot, base.Map), MessageTypeDefOf.PositiveEvent, true);
			SoundDefOf.MechClusterDefeated.PlayOneShotOnCamera(base.Map);
		}

		
		public void AddThingToNotifyOnDefeat(Thing t)
		{
			this.thingsToNotifyOnDefeat.AddDistinct(t);
		}

		
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.canAssaultColony, "canAssaultColony", false, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToNotifyOnDefeat, "thingsToNotifyOnDefeat", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isMechCluster, "isMechCluster", false, false);
			Scribe_Values.Look<bool>(ref this.mechClusterDefeated, "mechClusterDefeated", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				this.thingsToNotifyOnDefeat.RemoveAll((Thing x) => x.DestroyedOrNull());
			}
		}

		
		public List<Thing> things = new List<Thing>();

		
		protected List<Thing> thingsToNotifyOnDefeat = new List<Thing>();

		
		protected IntVec3 defSpot;

		
		protected Faction faction;

		
		protected float defendRadius;

		
		protected bool canAssaultColony;

		
		protected bool isMechCluster;

		
		protected bool mechClusterDefeated;
	}
}
