using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000993 RID: 2451
	public class QuestPart_SpawnWorldObject : QuestPart
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060039FD RID: 14845 RVA: 0x00133D7F File Offset: 0x00131F7F
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060039FE RID: 14846 RVA: 0x00133D90 File Offset: 0x00131F90
		public override bool IncreasesPopulation
		{
			get
			{
				Site site = this.worldObject as Site;
				return site != null && site.IncreasesPopulation;
			}
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x00133DB8 File Offset: 0x00131FB8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !this.spawned)
			{
				int num = this.worldObject.Tile;
				if (num == -1)
				{
					if (!TileFinder.TryFindNewSiteTile(out num, 7, 27, false, true, -1))
					{
						num = -1;
					}
				}
				else if (Find.WorldObjects.AnyWorldObjectAt(num))
				{
					if (!TileFinder.TryFindPassableTileWithTraversalDistance(num, 1, 50, out num, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x), false, true, false))
					{
						num = -1;
					}
				}
				if (num != -1)
				{
					this.worldObject.Tile = num;
					Find.WorldObjects.Add(this.worldObject);
					this.spawned = true;
				}
			}
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00133E78 File Offset: 0x00132078
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			Site site;
			if ((site = (this.worldObject as Site)) != null)
			{
				for (int i = 0; i < site.parts.Count; i++)
				{
					if (site.parts[i].things != null)
					{
						for (int j = 0; j < site.parts[i].things.Count; j++)
						{
							if (site.parts[i].things[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00133F14 File Offset: 0x00132114
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.defsToExcludeFromHyperlinks, "defsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			if (this.spawned)
			{
				Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
				return;
			}
			Scribe_Deep.Look<WorldObject>(ref this.worldObject, "worldObject", Array.Empty<object>());
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00133F90 File Offset: 0x00132190
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x04002233 RID: 8755
		public string inSignal;

		// Token: 0x04002234 RID: 8756
		public WorldObject worldObject;

		// Token: 0x04002235 RID: 8757
		public List<ThingDef> defsToExcludeFromHyperlinks;

		// Token: 0x04002236 RID: 8758
		private bool spawned;
	}
}
