using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000458 RID: 1112
	public class KeyPrefs
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x000CB31F File Offset: 0x000C951F
		// (set) Token: 0x06002124 RID: 8484 RVA: 0x000CB326 File Offset: 0x000C9526
		public static KeyPrefsData KeyPrefsData
		{
			get
			{
				return KeyPrefs.data;
			}
			set
			{
				KeyPrefs.data = value;
			}
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000CB330 File Offset: 0x000C9530
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.KeyPrefsFilePath).Exists;
			Dictionary<string, KeyBindingData> dictionary = DirectXmlLoader.ItemFromXmlFile<Dictionary<string, KeyBindingData>>(GenFilePaths.KeyPrefsFilePath, true);
			KeyPrefs.data = new KeyPrefsData();
			KeyPrefs.unresolvedBindings = new Dictionary<string, KeyBindingData>();
			foreach (KeyValuePair<string, KeyBindingData> keyValuePair in dictionary)
			{
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(keyValuePair.Key);
				if (namedSilentFail != null)
				{
					KeyPrefs.data.keyPrefs[namedSilentFail] = keyValuePair.Value;
				}
				else
				{
					KeyPrefs.unresolvedBindings[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (flag)
			{
				KeyPrefs.data.ResetToDefaults();
			}
			KeyPrefs.data.AddMissingDefaultBindings();
			KeyPrefs.data.ErrorCheck();
			if (flag)
			{
				KeyPrefs.Save();
			}
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000CB414 File Offset: 0x000C9614
		public static void Save()
		{
			try
			{
				Dictionary<string, KeyBindingData> dictionary = new Dictionary<string, KeyBindingData>();
				foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in KeyPrefs.data.keyPrefs)
				{
					dictionary[keyValuePair.Key.defName] = keyValuePair.Value;
				}
				foreach (KeyValuePair<string, KeyBindingData> keyValuePair2 in KeyPrefs.unresolvedBindings)
				{
					try
					{
						dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					catch (ArgumentException)
					{
					}
				}
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(dictionary, typeof(KeyPrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.KeyPrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.KeyPrefsFilePath, ex.ToString()));
				Log.Error("Exception saving keyprefs: " + ex, false);
			}
		}

		// Token: 0x04001431 RID: 5169
		private static KeyPrefsData data;

		// Token: 0x04001432 RID: 5170
		private static Dictionary<string, KeyBindingData> unresolvedBindings;

		// Token: 0x020016A9 RID: 5801
		public enum BindingSlot : byte
		{
			// Token: 0x040056EE RID: 22254
			A,
			// Token: 0x040056EF RID: 22255
			B
		}
	}
}
