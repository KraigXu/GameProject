using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_TrackWhenExitMentalState : QuestPart
	{
		
		
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

		
		public string tag;

		
		public List<string> inSignals;

		
		public string outSignal;

		
		public MapParent mapParent;

		
		public MentalStateDef mentalStateDef;

		
		private bool signalSent;

		
		[Unsaved(false)]
		private List<Pawn> cachedPawns;
	}
}
