using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DropMonumentMarkerCopy : QuestPart
	{
		
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x00131149 File Offset: 0x0012F349
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
				if (this.copy != null)
				{
					yield return this.copy;
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
				this.copy = null;
				MonumentMarker arg = signal.args.GetArg<MonumentMarker>("SUBJECT");
				if (arg != null && this.mapParent != null && this.mapParent.HasMap)
				{
					Map map = this.mapParent.Map;
					IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map);
					this.copy = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
					this.copy.sketch = arg.sketch.DeepCopy();
					if (!arg.questTags.NullOrEmpty<string>())
					{
						this.copy.questTags = new List<string>();
						this.copy.questTags.AddRange(arg.questTags);
					}
					DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(this.copy.MakeMinified()), 110, false, false, true, false);
				}
				if (!this.outSignalResult.NullOrEmpty())
				{
					if (this.copy != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, this.copy.Named("SUBJECT")));
						return;
					}
					Find.SignalManager.SendSignal(new Signal(this.outSignalResult));
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<MonumentMarker>(ref this.copy, "copy", false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		
		public MapParent mapParent;

		
		public string inSignal;

		
		public string outSignalResult;

		
		private MonumentMarker copy;
	}
}
