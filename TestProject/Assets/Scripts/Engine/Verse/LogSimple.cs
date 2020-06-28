using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000427 RID: 1063
	public static class LogSimple
	{
		// Token: 0x06001FCA RID: 8138 RVA: 0x000C2908 File Offset: 0x000C0B08
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000C293D File Offset: 0x000C0B3D
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000C2951 File Offset: 0x000C0B51
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000C2960 File Offset: 0x000C0B60
		public static void FlushToFileAndOpen()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			string value = LogSimple.CompiledLog();
			string path = GenFilePaths.SaveDataFolderPath + Path.DirectorySeparatorChar.ToString() + "LogSimple.txt";
			using (StreamWriter streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(value);
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Application.OpenURL(path);
			});
			LogSimple.messages.Clear();
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000C29F4 File Offset: 0x000C0BF4
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			Log.Message(LogSimple.CompiledLog(), false);
			LogSimple.messages.Clear();
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000C2A18 File Offset: 0x000C0C18
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x040013A1 RID: 5025
		private static List<string> messages = new List<string>();

		// Token: 0x040013A2 RID: 5026
		private static int tabDepth = 0;
	}
}
