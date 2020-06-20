using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000965 RID: 2405
	public class QuestPart_AddMemoryThought : QuestPart
	{
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x0012FCD8 File Offset: 0x0012DED8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null && this.addToLookTargets)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0012FCE8 File Offset: 0x0012DEE8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def, this.otherPawn);
			}
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x0012FD50 File Offset: 0x0012DF50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x0012FDB9 File Offset: 0x0012DFB9
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x0012FDE5 File Offset: 0x0012DFE5
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
			if (this.otherPawn == replace)
			{
				this.otherPawn = with;
			}
		}

		// Token: 0x04002195 RID: 8597
		public string inSignal;

		// Token: 0x04002196 RID: 8598
		public ThoughtDef def;

		// Token: 0x04002197 RID: 8599
		public Pawn pawn;

		// Token: 0x04002198 RID: 8600
		public Pawn otherPawn;

		// Token: 0x04002199 RID: 8601
		public bool addToLookTargets = true;
	}
}
