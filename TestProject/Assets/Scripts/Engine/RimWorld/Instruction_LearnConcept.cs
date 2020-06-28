using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1D RID: 3869
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06005EB8 RID: 24248 RVA: 0x0020BED3 File Offset: 0x0020A0D3
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x0020BEE5 File Offset: 0x0020A0E5
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x0020BF02 File Offset: 0x0020A102
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (PlayerKnowledgeDatabase.IsComplete(this.def.concept))
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
