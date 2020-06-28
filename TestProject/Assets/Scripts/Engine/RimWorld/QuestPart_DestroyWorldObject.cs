using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000970 RID: 2416
	public class QuestPart_DestroyWorldObject : QuestPart
	{
		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600393B RID: 14651 RVA: 0x00130B00 File Offset: 0x0012ED00
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

		// Token: 0x0600393C RID: 14652 RVA: 0x00130B10 File Offset: 0x0012ED10
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x00130B37 File Offset: 0x0012ED37
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x00130B64 File Offset: 0x0012ED64
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

		// Token: 0x0600393F RID: 14655 RVA: 0x00130BB8 File Offset: 0x0012EDB8
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

		// Token: 0x040021BA RID: 8634
		public string inSignal;

		// Token: 0x040021BB RID: 8635
		public WorldObject worldObject;
	}
}
