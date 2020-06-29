using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_SituationalThought : QuestPartActivable
	{
		
		// (get) Token: 0x060039EF RID: 14831 RVA: 0x0013395A File Offset: 0x00131B5A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.stage, "stage", 0, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThoughtDefOf.DecreeUnmet;
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		
		public ThoughtDef def;

		
		public Pawn pawn;

		
		public int stage;

		
		public int delayTicks;
	}
}
