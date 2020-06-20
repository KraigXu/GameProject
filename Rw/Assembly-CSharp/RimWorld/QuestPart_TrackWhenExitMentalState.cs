using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000995 RID: 2453
	public class QuestPart_TrackWhenExitMentalState : QuestPart
	{
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x0013416C File Offset: 0x0013236C
		private List<Pawn> TrackedPawns
		{
			get
			{
				if (this.cachedPawns == null)
				{
					this.cachedPawns = (from p in this.mapParent.Map.mapPawns.AllPawnsSpawned
					where p.InMentalState && p.MentalStateDef == this.mentalStateDef && p.questTags.Contains(this.tag)
					select p).ToList<Pawn>();
				}
				return this.cachedPawns;
			}
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x001341B8 File Offset: 0x001323B8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!this.signalSent && this.inSignals.Contains(signal.tag))
			{
				Pawn pawn = this.TrackedPawns.Find((Pawn p) => p == signal.args.GetArg<Pawn>("SUBJECT"));
				if (pawn != null)
				{
					this.cachedPawns.Remove(pawn);
				}
				if (!this.cachedPawns.Any<Pawn>())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal));
					this.signalSent = true;
				}
			}
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00134250 File Offset: 0x00132450
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Defs.Look<MentalStateDef>(ref this.mentalStateDef, "mentalStateDef");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.signalSent, "signalSent", false, false);
		}

		// Token: 0x0400223B RID: 8763
		public string tag;

		// Token: 0x0400223C RID: 8764
		public List<string> inSignals;

		// Token: 0x0400223D RID: 8765
		public string outSignal;

		// Token: 0x0400223E RID: 8766
		public MapParent mapParent;

		// Token: 0x0400223F RID: 8767
		public MentalStateDef mentalStateDef;

		// Token: 0x04002240 RID: 8768
		private bool signalSent;

		// Token: 0x04002241 RID: 8769
		[Unsaved(false)]
		private List<Pawn> cachedPawns;
	}
}
