using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F29 RID: 3881
	public static class PlayerKnowledgeDatabase
	{
		// Token: 0x06005F0C RID: 24332 RVA: 0x0020CD46 File Offset: 0x0020AF46
		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x0020CD50 File Offset: 0x0020AF50
		public static void ReloadAndRebind()
		{
			PlayerKnowledgeDatabase.data = DirectXmlLoader.ItemFromXmlFile<PlayerKnowledgeDatabase.ConceptKnowledge>(GenFilePaths.ConceptKnowledgeFilePath, true);
			foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
			{
				if (!PlayerKnowledgeDatabase.data.knowledge.ContainsKey(conceptDef.defName))
				{
					Log.Warning("Knowledge data was missing key " + conceptDef + ". Adding it...", false);
					PlayerKnowledgeDatabase.data.knowledge.Add(conceptDef.defName, 0f);
				}
			}
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x0020CDEC File Offset: 0x0020AFEC
		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new PlayerKnowledgeDatabase.ConceptKnowledge();
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x0020CE1C File Offset: 0x0020B01C
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(PlayerKnowledgeDatabase.data, typeof(PlayerKnowledgeDatabase.ConceptKnowledge));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.ConceptKnowledgeFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.ConceptKnowledgeFilePath, ex.ToString()));
				Log.Error("Exception saving knowledge database: " + ex, false);
			}
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x0020CEA4 File Offset: 0x0020B0A4
		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def.defName];
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x0020CEBC File Offset: 0x0020B0BC
		public static void SetKnowledge(ConceptDef def, float value)
		{
			float num = PlayerKnowledgeDatabase.data.knowledge[def.defName];
			float num2 = Mathf.Clamp01(value);
			PlayerKnowledgeDatabase.data.knowledge[def.defName] = num2;
			if (num < 0.999f && num2 >= 0.999f)
			{
				PlayerKnowledgeDatabase.NewlyLearned(def);
			}
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x0020CF10 File Offset: 0x0020B110
		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc.defName] > 0.999f;
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x0020CF2E File Offset: 0x0020B12E
		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x0020CF64 File Offset: 0x0020B164
		public static void KnowledgeDemonstrated(ConceptDef conc, KnowledgeAmount know)
		{
			float num;
			switch (know)
			{
			case KnowledgeAmount.FrameDisplayed:
				num = ((Event.current.type == EventType.Repaint) ? 0.004f : 0f);
				break;
			case KnowledgeAmount.FrameInteraction:
				num = 0.008f;
				break;
			case KnowledgeAmount.TinyInteraction:
				num = 0.03f;
				break;
			case KnowledgeAmount.SmallInteraction:
				num = 0.1f;
				break;
			case KnowledgeAmount.SpecificInteraction:
				num = 0.4f;
				break;
			case KnowledgeAmount.Total:
				num = 1f;
				break;
			case KnowledgeAmount.NoteClosed:
				num = 0.5f;
				break;
			case KnowledgeAmount.NoteTaught:
				num = 1f;
				break;
			default:
				throw new NotImplementedException();
			}
			if (num <= 0f)
			{
				return;
			}
			PlayerKnowledgeDatabase.SetKnowledge(conc, PlayerKnowledgeDatabase.GetKnowledge(conc) + num);
			LessonAutoActivator.Notify_KnowledgeDemonstrated(conc);
			if (Find.ActiveLesson != null)
			{
				Find.ActiveLesson.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x0400338F RID: 13199
		private static PlayerKnowledgeDatabase.ConceptKnowledge data;

		// Token: 0x02001E27 RID: 7719
		private class ConceptKnowledge
		{
			// Token: 0x0600A80D RID: 43021 RVA: 0x00317270 File Offset: 0x00315470
			public ConceptKnowledge()
			{
				foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(conceptDef.defName, 0f);
				}
			}

			// Token: 0x0400717B RID: 29051
			public Dictionary<string, float> knowledge = new Dictionary<string, float>();
		}
	}
}
