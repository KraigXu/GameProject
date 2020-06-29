using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_MechCluster : QuestPart
	{
		
		// (get) Token: 0x060039A1 RID: 14753 RVA: 0x001326EA File Offset: 0x001308EA
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.spawnedClusterPos.IsValid && this.mapParent != null && this.mapParent.HasMap)
				{
					yield return new GlobalTargetInfo(this.spawnedClusterPos, this.mapParent.Map, false);
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
				this.spawnedClusterPos = MechClusterUtility.FindClusterPosition(this.mapParent.Map, this.sketch, 100, 0.5f);
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					return;
				}
				MechClusterUtility.SpawnCluster(this.spawnedClusterPos, this.mapParent.Map, this.sketch, true, false, this.tag);
				Find.LetterStack.ReceiveLetter("LetterLabelMechClusterArrived".Translate(), "LetterMechClusterArrived".Translate(), LetterDefOf.ThreatBig, new TargetInfo(this.spawnedClusterPos, this.mapParent.Map, false), null, this.quest, null, null);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.spawnedClusterPos, "spawnedClusterPos", default(IntVec3), false);
		}

		
		public MechClusterSketch sketch;

		
		public string inSignal;

		
		public string tag;

		
		public MapParent mapParent;

		
		private IntVec3 spawnedClusterPos = IntVec3.Invalid;
	}
}
