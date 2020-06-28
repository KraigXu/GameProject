using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000992 RID: 2450
	public class QuestPart_SpawnThing : QuestPart
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060039F5 RID: 14837 RVA: 0x001339F9 File Offset: 0x00131BF9
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.questLookTarget)
				{
					yield return this.innerSkyfallerThing ?? this.thing;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x00133A0C File Offset: 0x00131C0C
		public override bool IncreasesPopulation
		{
			get
			{
				Pawn pawn = this.thing as Pawn;
				return pawn != null && PawnsArriveQuestPartUtility.IncreasesPopulation(Gen.YieldSingle<Pawn>(pawn), false, false);
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x00133A38 File Offset: 0x00131C38
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent.HasMap)
			{
				IntVec3 center = IntVec3.Invalid;
				if (this.cell.IsValid)
				{
					center = this.cell;
				}
				else
				{
					Thing thing;
					if (this.tryLandInShipLandingZone && !DropCellFinder.TryFindShipLandingArea(this.mapParent.Map, out center, out thing))
					{
						if (thing != null)
						{
							Messages.Message("ShuttleBlocked".Translate("BlockedBy".Translate(thing).CapitalizeFirst()), thing, MessageTypeDefOf.NeutralEvent, true);
						}
						center = DropCellFinder.TryFindSafeLandingSpotCloseToColony(this.mapParent.Map, this.thing.def.Size, this.factionForFindingSpot, 2);
					}
					if (!center.IsValid && (!this.lookForSafeSpot || !DropCellFinder.FindSafeLandingSpot(out center, this.factionForFindingSpot, this.mapParent.Map, 35, 15, 25, new IntVec2?(this.thing.def.size))))
					{
						IntVec3 intVec = DropCellFinder.RandomDropSpot(this.mapParent.Map);
						if (!DropCellFinder.TryFindDropSpotNear(intVec, this.mapParent.Map, out center, false, false, false, new IntVec2?(this.thing.def.size)))
						{
							center = intVec;
						}
					}
				}
				GenPlace.TryPlaceThing(this.thing, center, this.mapParent.Map, ThingPlaceMode.Near, null, null, default(Rot4));
				this.spawned = true;
				Skyfaller skyfaller = this.thing as Skyfaller;
				if (skyfaller != null && skyfaller.innerContainer.Count == 1)
				{
					this.innerSkyfallerThing = skyfaller.innerContainer.First<Thing>();
					return;
				}
				this.innerSkyfallerThing = null;
			}
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00133C05 File Offset: 0x00131E05
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.thing;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x00133C10 File Offset: 0x00131E10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			if (!this.spawned && (this.thing == null || !(this.thing is Pawn)))
			{
				Scribe_Deep.Look<Thing>(ref this.thing, "thing", Array.Empty<object>());
			}
			else
			{
				Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			}
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.lookForSafeSpot, "lookForSafeSpot", false, false);
			Scribe_References.Look<Faction>(ref this.factionForFindingSpot, "factionForFindingSpot", false);
			Scribe_Values.Look<bool>(ref this.questLookTarget, "questLookTarget", true, false);
			Scribe_References.Look<Thing>(ref this.innerSkyfallerThing, "innerSkyfallerThing", false);
			Scribe_Values.Look<bool>(ref this.tryLandInShipLandingZone, "tryLandInShipLandingZone", false, false);
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x00133D10 File Offset: 0x00131F10
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x04002229 RID: 8745
		public string inSignal;

		// Token: 0x0400222A RID: 8746
		public Thing thing;

		// Token: 0x0400222B RID: 8747
		public Faction factionForFindingSpot;

		// Token: 0x0400222C RID: 8748
		public MapParent mapParent;

		// Token: 0x0400222D RID: 8749
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x0400222E RID: 8750
		public bool questLookTarget = true;

		// Token: 0x0400222F RID: 8751
		public bool lookForSafeSpot;

		// Token: 0x04002230 RID: 8752
		public bool tryLandInShipLandingZone;

		// Token: 0x04002231 RID: 8753
		private Thing innerSkyfallerThing;

		// Token: 0x04002232 RID: 8754
		private bool spawned;
	}
}
