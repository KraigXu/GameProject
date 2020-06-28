using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001D RID: 29
	public static class GenFile
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x000097AA File Offset: 0x000079AA
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000097B4 File Offset: 0x000079B4
		public static string TextFromResourceFile(string filePath)
		{
			TextAsset textAsset = Resources.Load("Text/" + filePath) as TextAsset;
			if (textAsset == null)
			{
				Log.Message("Found no text asset in resources at " + filePath, false);
				return null;
			}
			return GenFile.GetTextWithoutBOM(textAsset);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000097FC File Offset: 0x000079FC
		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string result = null;
			using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000985C File Offset: 0x00007A5C
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string text = GenFile.TextFromResourceFile(filePath);
			foreach (string text2 in GenText.LinesFromString(text))
			{
				yield return text2;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000986C File Offset: 0x00007A6C
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool useLinuxLineEndings = false)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destDirName, fileInfo.Name);
				if (useLinuxLineEndings && (fileInfo.Extension == ".sh" || fileInfo.Extension == ".txt"))
				{
					if (!File.Exists(text))
					{
						File.WriteAllText(text, File.ReadAllText(fileInfo.FullName).Replace("\r\n", "\n").Replace("\r", "\n"));
					}
				}
				else
				{
					fileInfo.CopyTo(text, false);
				}
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings);
				}
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009980 File Offset: 0x00007B80
		public static string SanitizedFileName(string fileName)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			string text = "";
			for (int i = 0; i < fileName.Length; i++)
			{
				if (!invalidFileNameChars.Contains(fileName[i]))
				{
					text += fileName[i].ToString();
				}
			}
			if (text.Length == 0)
			{
				text = "unnamed";
			}
			return text;
		}
	}
}
