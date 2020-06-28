using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000929 RID: 2345
	public static class ExternalHistoryUtility
	{
		// Token: 0x060037BA RID: 14266 RVA: 0x0012B038 File Offset: 0x00129238
		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message, false);
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060037BB RID: 14267 RVA: 0x0012B090 File Offset: 0x00129290
		public static IEnumerable<FileInfo> Files
		{
			get
			{
				int num;
				for (int i = 0; i < ExternalHistoryUtility.cachedFiles.Count; i = num)
				{
					yield return ExternalHistoryUtility.cachedFiles[i];
					num = i + 1;
				}
				yield break;
			}
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x0012B09C File Offset: 0x0012929C
		public static ExternalHistory Load(string path)
		{
			ExternalHistory result = null;
			try
			{
				result = new ExternalHistory();
				Scribe.loader.InitLoading(path);
				try
				{
					Scribe_Deep.Look<ExternalHistory>(ref result, "externalHistory", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Could not load external history (" + path + "): " + ex.Message, false);
				return null;
			}
			return result;
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x0012B124 File Offset: 0x00129324
		public static string GetRandomGameplayID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ExternalHistoryUtility.gameplayIDLength; i++)
			{
				int index = Rand.Range(0, ExternalHistoryUtility.gameplayIDAvailableChars.Length);
				stringBuilder.Append(ExternalHistoryUtility.gameplayIDAvailableChars[index]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x0012B170 File Offset: 0x00129370
		public static bool IsValidGameplayID(string ID)
		{
			if (ID.NullOrEmpty() || ID.Length != ExternalHistoryUtility.gameplayIDLength)
			{
				return false;
			}
			for (int i = 0; i < ID.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < ExternalHistoryUtility.gameplayIDAvailableChars.Length; j++)
				{
					if (ID[i] == ExternalHistoryUtility.gameplayIDAvailableChars[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x0012B1DC File Offset: 0x001293DC
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x0012B1FC File Offset: 0x001293FC
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}

		// Token: 0x04002102 RID: 8450
		private static List<FileInfo> cachedFiles;

		// Token: 0x04002103 RID: 8451
		private static int gameplayIDLength = 20;

		// Token: 0x04002104 RID: 8452
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	}
}
