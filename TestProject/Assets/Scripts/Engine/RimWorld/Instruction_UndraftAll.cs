using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F22 RID: 3874
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x0020C36F File Offset: 0x0020A56F
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06005ED7 RID: 24279 RVA: 0x0020C395 File Offset: 0x0020A595
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06005ED8 RID: 24280 RVA: 0x0020C3CC File Offset: 0x0020A5CC
		public override void LessonUpdate()
		{
			foreach (Pawn pawn in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(pawn.DrawPos, false);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
