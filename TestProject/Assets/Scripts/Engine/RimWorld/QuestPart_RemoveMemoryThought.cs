using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000987 RID: 2439
	public class QuestPart_RemoveMemoryThought : QuestPart
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x00132E05 File Offset: 0x00131005
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

		// Token: 0x060039C0 RID: 14784 RVA: 0x00132E18 File Offset: 0x00131018
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				if (this.count != null)
				{
					for (int i = 0; i < this.count.Value; i++)
					{
						Thought_Memory thought_Memory = this.pawn.needs.mood.thoughts.memories.Memories.FirstOrDefault((Thought_Memory m) => this.def == m.def && (this.otherPawn == null || m.otherPawn == this.otherPawn));
						if (thought_Memory == null)
						{
							return;
						}
						this.pawn.needs.mood.thoughts.memories.RemoveMemory(thought_Memory);
					}
					return;
				}
				if (this.otherPawn == null)
				{
					this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.def);
					return;
				}
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(this.def, this.otherPawn);
			}
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x00132F34 File Offset: 0x00131134
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Values.Look<int?>(ref this.count, "count", null, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x00132FB7 File Offset: 0x001311B7
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x00132FE3 File Offset: 0x001311E3
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

		// Token: 0x0400220D RID: 8717
		public string inSignal;

		// Token: 0x0400220E RID: 8718
		public ThoughtDef def;

		// Token: 0x0400220F RID: 8719
		public Pawn pawn;

		// Token: 0x04002210 RID: 8720
		public Pawn otherPawn;

		// Token: 0x04002211 RID: 8721
		public int? count;

		// Token: 0x04002212 RID: 8722
		public bool addToLookTargets = true;
	}
}
