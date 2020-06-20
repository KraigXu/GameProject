using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2A RID: 3882
	public class Tutor : IExposable
	{
		// Token: 0x06005F15 RID: 24341 RVA: 0x0020D024 File Offset: 0x0020B224
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", Array.Empty<object>());
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", Array.Empty<object>());
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x0020D070 File Offset: 0x0020B270
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x0020D088 File Offset: 0x0020B288
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		// Token: 0x04003390 RID: 13200
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04003391 RID: 13201
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04003392 RID: 13202
		public TutorialState tutorialState = new TutorialState();
	}
}
