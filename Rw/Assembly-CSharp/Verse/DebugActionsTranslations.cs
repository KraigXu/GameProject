using System;

namespace Verse
{
	// Token: 0x02000338 RID: 824
	public static class DebugActionsTranslations
	{
		// Token: 0x0600185A RID: 6234 RVA: 0x0008C033 File Offset: 0x0008A233
		[DebugAction("Translation", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void WriteBackstoryTranslationFile()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0008C03A File Offset: 0x0008A23A
		[DebugAction("Translation", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void SaveTranslationReport()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}
	}
}
