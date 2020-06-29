using System;
using Verse;

namespace RimWorld
{
	
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		
		// (get) Token: 0x06005EB8 RID: 24248 RVA: 0x0020BED3 File Offset: 0x0020A0D3
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		
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
