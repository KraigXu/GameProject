using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000772 RID: 1906
	public abstract class LordJob_MechanoidDefendBase : LordJob
	{
		// Token: 0x060031CA RID: 12746 RVA: 0x00115618 File Offset: 0x00113818
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.isMechCluster && !this.mechClusterDefeated && !MechClusterUtility.AnyThreatBuilding(this.things))
			{
				this.OnDefeat();
			}
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x00115643 File Offset: 0x00113843
		public override void Notify_LordDestroyed()
		{
			if (this.isMechCluster && !this.mechClusterDefeated)
			{
				this.OnDefeat();
			}
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x0011565C File Offset: 0x0011385C
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

		// Token: 0x060031CD RID: 12749 RVA: 0x001157E0 File Offset: 0x001139E0
		public void AddThingToNotifyOnDefeat(Thing t)
		{
			this.thingsToNotifyOnDefeat.AddDistinct(t);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x001157F0 File Offset: 0x001139F0
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

		// Token: 0x04001B27 RID: 6951
		public List<Thing> things = new List<Thing>();

		// Token: 0x04001B28 RID: 6952
		protected List<Thing> thingsToNotifyOnDefeat = new List<Thing>();

		// Token: 0x04001B29 RID: 6953
		protected IntVec3 defSpot;

		// Token: 0x04001B2A RID: 6954
		protected Faction faction;

		// Token: 0x04001B2B RID: 6955
		protected float defendRadius;

		// Token: 0x04001B2C RID: 6956
		protected bool canAssaultColony;

		// Token: 0x04001B2D RID: 6957
		protected bool isMechCluster;

		// Token: 0x04001B2E RID: 6958
		protected bool mechClusterDefeated;
	}
}
