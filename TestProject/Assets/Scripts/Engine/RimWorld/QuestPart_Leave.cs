using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Leave : QuestPart
	{
		
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x00131A9A File Offset: 0x0012FC9A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest);
			}
		}

		
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.leaveOnCleanup)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<bool>(ref this.leaveOnCleanup, "leaveOnCleanup", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				if (randomPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
				{
					this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.First<Pawn>());
				}
			}
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public string inSignal;

		
		public List<Pawn> pawns = new List<Pawn>();

		
		public bool sendStandardLetter = true;

		
		public bool leaveOnCleanup = true;
	}
}
