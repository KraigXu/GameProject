using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000991 RID: 2449
	public class QuestPart_SituationalThought : QuestPartActivable
	{
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060039EF RID: 14831 RVA: 0x0013395A File Offset: 0x00131B5A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x0013396C File Offset: 0x00131B6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.stage, "stage", 0, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x001339C4 File Offset: 0x00131BC4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThoughtDefOf.DecreeUnmet;
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x001339E7 File Offset: 0x00131BE7
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002225 RID: 8741
		public ThoughtDef def;

		// Token: 0x04002226 RID: 8742
		public Pawn pawn;

		// Token: 0x04002227 RID: 8743
		public int stage;

		// Token: 0x04002228 RID: 8744
		public int delayTicks;
	}
}
