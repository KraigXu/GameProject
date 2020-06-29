using System;
using Verse;

namespace RimWorld
{
	
	public class Tutor : IExposable
	{
		
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", Array.Empty<object>());
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", Array.Empty<object>());
		}

		
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		
		public LearningReadout learningReadout = new LearningReadout();

		
		public TutorialState tutorialState = new TutorialState();
	}
}
