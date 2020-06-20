using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0B RID: 3851
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06005E51 RID: 24145 RVA: 0x0020A7F7 File Offset: 0x002089F7
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06005E52 RID: 24146 RVA: 0x0020A7FF File Offset: 0x002089FF
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x06005E53 RID: 24147 RVA: 0x0020A818 File Offset: 0x00208A18
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x0020A830 File Offset: 0x00208A30
		public void Activate(InstructionDef id)
		{
			Lesson_Instruction lesson_Instruction = this.activeLesson as Lesson_Instruction;
			if (lesson_Instruction != null && id == lesson_Instruction.def)
			{
				return;
			}
			Lesson_Instruction lesson_Instruction2 = (Lesson_Instruction)Activator.CreateInstance(id.instructionClass);
			lesson_Instruction2.def = id;
			this.activeLesson = lesson_Instruction2;
			this.activeLesson.OnActivated();
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x0020A880 File Offset: 0x00208A80
		public void Activate(Lesson lesson)
		{
			Lesson_Note lesson_Note = lesson as Lesson_Note;
			if (lesson_Note != null && this.activeLesson != null)
			{
				lesson_Note.doFadeIn = false;
			}
			this.activeLesson = lesson;
			this.activeLesson.OnActivated();
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x0020A8B8 File Offset: 0x00208AB8
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x0020A8DC File Offset: 0x00208ADC
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonOnGUI();
		}

		// Token: 0x06005E58 RID: 24152 RVA: 0x0020A8FE File Offset: 0x00208AFE
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonUpdate();
		}

		// Token: 0x06005E59 RID: 24153 RVA: 0x0020A920 File Offset: 0x00208B20
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x0400335A RID: 13146
		private Lesson activeLesson;
	}
}
