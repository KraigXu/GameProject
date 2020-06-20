using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097E RID: 2430
	public class QuestPart_Leave : QuestPart
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x00131A9A File Offset: 0x0012FC9A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
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

		// Token: 0x06003987 RID: 14727 RVA: 0x00131AAA File Offset: 0x0012FCAA
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest);
			}
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x00131ADD File Offset: 0x0012FCDD
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.leaveOnCleanup)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest);
			}
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x00131B04 File Offset: 0x0012FD04
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

		// Token: 0x0600398A RID: 14730 RVA: 0x00131B98 File Offset: 0x0012FD98
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

		// Token: 0x0600398B RID: 14731 RVA: 0x00131BFA File Offset: 0x0012FDFA
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040021E6 RID: 8678
		public string inSignal;

		// Token: 0x040021E7 RID: 8679
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040021E8 RID: 8680
		public bool sendStandardLetter = true;

		// Token: 0x040021E9 RID: 8681
		public bool leaveOnCleanup = true;
	}
}
