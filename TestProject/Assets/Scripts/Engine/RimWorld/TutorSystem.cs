﻿using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class TutorSystem
	{
		
		// (get) Token: 0x06005F1D RID: 24349 RVA: 0x0020D221 File Offset: 0x0020B421
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		
		// (get) Token: 0x06005F1E RID: 24350 RVA: 0x0020D247 File Offset: 0x0020B447
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.AdaptiveTrainingEnabled && (Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining);
			}
		}

		
		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		
		public static void Notify_Event(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return;
			}
			if (DebugViewSettings.logTutor)
			{
				Log.Message("Notify_Event: " + ep, false);
			}
			if (Current.Game == null)
			{
				return;
			}
			Lesson lesson = Find.ActiveLesson.Current;
			if (Find.ActiveLesson.Current != null)
			{
				Find.ActiveLesson.Current.Notify_Event(ep);
			}
			foreach (InstructionDef instructionDef in DefDatabase<InstructionDef>.AllDefs)
			{
				if (instructionDef.eventTagInitiate == ep.Tag && (instructionDef.eventTagInitiateSource == null || (lesson != null && instructionDef.eventTagInitiateSource == lesson.Instruction)) && (TutorSystem.TutorialMode || !instructionDef.tutorialModeOnly))
				{
					Find.ActiveLesson.Activate(instructionDef);
					break;
				}
			}
		}

		
		public static bool AllowAction(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return true;
			}
			if (DebugViewSettings.logTutor)
			{
				Log.Message("AllowAction: " + ep, false);
			}
			if (ep.Cells != null && ep.Cells.Count<IntVec3>() == 1)
			{
				return TutorSystem.AllowAction(new EventPack(ep.Tag, ep.Cells.First<IntVec3>()));
			}
			if (Find.ActiveLesson.Current != null)
			{
				AcceptanceReport acceptanceReport = Find.ActiveLesson.Current.AllowAction(ep);
				if (!acceptanceReport.Accepted)
				{
					Messages.Message((!acceptanceReport.Reason.NullOrEmpty()) ? acceptanceReport.Reason : Find.ActiveLesson.Current.DefaultRejectInputMessage, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return true;
		}
	}
}
