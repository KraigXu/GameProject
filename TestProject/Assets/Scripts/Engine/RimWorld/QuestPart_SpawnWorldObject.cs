using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_SpawnWorldObject : QuestPart
	{
		
		
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{


				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		
		
		public override bool IncreasesPopulation
		{
			get
			{
				Site site = this.worldObject as Site;
				return site != null && site.IncreasesPopulation;
			}
		}

		
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

		
		public string inSignal;

		
		public WorldObject worldObject;

		
		public List<ThingDef> defsToExcludeFromHyperlinks;

		
		private bool spawned;
	}
}
