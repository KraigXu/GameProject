using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DestroyWorldObject : QuestPart
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

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSiteNormal(null, tile, null, true, null);
			}
		}

		
		public static void TryRemove(WorldObject worldObject)
		{
			if (worldObject != null && worldObject.Spawned)
			{
				MapParent mapParent = worldObject as MapParent;
				if (mapParent != null && mapParent.HasMap)
				{
					mapParent.forceRemoveWorldObjectWhenMapRemoved = true;
					return;
				}
				worldObject.Destroy();
			}
		}

		
		public string inSignal;

		
		public WorldObject worldObject;
	}
}
