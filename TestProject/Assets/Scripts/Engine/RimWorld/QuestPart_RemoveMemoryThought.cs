using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_RemoveMemoryThought : QuestPart
	{
		
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x00132E05 File Offset: 0x00131005
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		
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

		
		public string inSignal;

		
		public ThoughtDef def;

		
		public Pawn pawn;

		
		public Pawn otherPawn;

		
		public int? count;

		
		public bool addToLookTargets = true;
	}
}
