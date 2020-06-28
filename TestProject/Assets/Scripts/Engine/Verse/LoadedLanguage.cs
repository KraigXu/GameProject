using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000145 RID: 325
	public class LoadedLanguage
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0002F91B File Offset: 0x0002DB1B
		public string DisplayName
		{
			get
			{
				return GenText.SplitCamelCase(this.folderName);
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x0002F928 File Offset: 0x0002DB28
		public string FriendlyNameNative
		{
			get
			{
				if (this.info == null || this.info.friendlyNameNative.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameNative;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0002F956 File Offset: 0x0002DB56
		public string FriendlyNameEnglish
		{
			get
			{
				if (this.info == null || this.info.friendlyNameEnglish.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameEnglish;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0002F984 File Offset: 0x0002DB84
		public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> AllDirectories
		{
			get
			{
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					foreach (string text in mod.foldersToLoadDescendingOrder)
					{
						string path = Path.Combine(text, "Languages");
						VirtualDirectory directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.folderName));
						if (directory.Exists)
						{
							yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
						}
						else
						{
							directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.legacyFolderName));
							if (directory.Exists)
							{
								yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
							}
						}
					}
					List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0002F994 File Offset: 0x0002DB94
		public LanguageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LanguageWorker)Activator.CreateInstance(this.info.languageWorkerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x0002F9BF File Offset: 0x0002DBBF
		public string LegacyFolderName
		{
			get
			{
				return this.legacyFolderName;
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002F9C8 File Offset: 0x0002DBC8
		public LoadedLanguage(string folderName)
		{
			this.folderName = folderName;
			this.legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002FA6C File Offset: 0x0002DC6C
		public void LoadMetadata()
		{
			if (this.info != null && this.infoIsRealMetadata)
			{
				return;
			}
			this.infoIsRealMetadata = true;
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (virtualDirectory.Name == this.folderName || virtualDirectory.Name == this.legacyFolderName)
							{
								this.info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(virtualDirectory, "LanguageInfo.xml", false);
								if (this.info.friendlyNameNative.NullOrEmpty() && virtualDirectory.FileExists("FriendlyName.txt"))
								{
									this.info.friendlyNameNative = virtualDirectory.ReadAllText("FriendlyName.txt");
								}
								if (this.info.friendlyNameNative.NullOrEmpty())
								{
									this.info.friendlyNameNative = this.folderName;
								}
								if (this.info.friendlyNameEnglish.NullOrEmpty())
								{
									this.info.friendlyNameEnglish = this.folderName;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002FC54 File Offset: 0x0002DE54
		public void InitMetadata(VirtualDirectory directory)
		{
			this.infoIsRealMetadata = false;
			this.info = new LanguageInfo();
			string text = Regex.Replace(directory.Name, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
			string friendlyNameEnglish = text;
			string friendlyNameNative = text;
			int num = text.FirstIndexOf((char c) => c == '(');
			int num2 = text.LastIndexOf(")");
			if (num2 > num)
			{
				friendlyNameEnglish = text.Substring(0, num - 1);
				friendlyNameNative = text.Substring(num + 1, num2 - num - 1);
			}
			this.info.friendlyNameEnglish = friendlyNameEnglish;
			this.info.friendlyNameNative = friendlyNameNative;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002FCF8 File Offset: 0x0002DEF8
		public void LoadData()
		{
			if (this.dataIsLoaded)
			{
				return;
			}
			this.dataIsLoaded = true;
			DeepProfiler.Start("Loading language data: " + this.folderName);
			try
			{
				this.tmpAlreadyLoadedFiles.Clear();
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in this.AllDirectories)
				{
					Tuple<VirtualDirectory, ModContentPack, string> localDirectory = tuple;
					if (!this.tmpAlreadyLoadedFiles.ContainsKey(localDirectory.Item2))
					{
						this.tmpAlreadyLoadedFiles[localDirectory.Item2] = new HashSet<string>();
					}
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.icon == BaseContent.BadTex)
						{
							VirtualFile file = localDirectory.Item1.GetFile("LangIcon.png");
							if (file.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(file).contentItem;
							}
						}
					});
					VirtualDirectory directory = localDirectory.Item1.GetDirectory("CodeLinked");
					if (directory.Exists)
					{
						this.loadErrors.Add("Translations aren't called CodeLinked any more. Please rename to Keyed: " + directory);
					}
					else
					{
						directory = localDirectory.Item1.GetDirectory("Keyed");
					}
					if (directory.Exists)
					{
						foreach (VirtualFile virtualFile in directory.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							if (this.TryRegisterFileIfNew(localDirectory, virtualFile.FullPath))
							{
								this.LoadFromFile_Keyed(virtualFile);
							}
						}
					}
					VirtualDirectory directory2 = localDirectory.Item1.GetDirectory("DefLinked");
					if (directory2.Exists)
					{
						this.loadErrors.Add("Translations aren't called DefLinked any more. Please rename to DefInjected: " + directory2);
					}
					else
					{
						directory2 = localDirectory.Item1.GetDirectory("DefInjected");
					}
					if (directory2.Exists)
					{
						foreach (VirtualDirectory virtualDirectory in directory2.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							string name = virtualDirectory.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name, null);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1), null);
							}
							if (typeInAnyAssembly == null)
							{
								this.loadErrors.Add(string.Concat(new object[]
								{
									"Error loading language from ",
									tuple,
									": dir ",
									virtualDirectory.Name,
									" doesn't correspond to any def type. Skipping..."
								}));
							}
							else
							{
								foreach (VirtualFile virtualFile2 in virtualDirectory.GetFiles("*.xml", SearchOption.AllDirectories))
								{
									if (this.TryRegisterFileIfNew(localDirectory, virtualFile2.FullPath))
									{
										this.LoadFromFile_DefInject(virtualFile2, typeInAnyAssembly);
									}
								}
							}
						}
					}
					this.EnsureAllDefTypesHaveDefInjectionPackage();
					VirtualDirectory directory3 = localDirectory.Item1.GetDirectory("Strings");
					if (directory3.Exists)
					{
						foreach (VirtualDirectory virtualDirectory2 in directory3.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							foreach (VirtualFile virtualFile3 in virtualDirectory2.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								if (this.TryRegisterFileIfNew(localDirectory, virtualFile3.FullPath))
								{
									this.LoadFromFile_Strings(virtualFile3, directory3);
								}
							}
						}
					}
					this.wordInfo.LoadFrom(localDirectory, this);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading language data. Rethrowing. Exception: " + arg, false);
				throw;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0003017C File Offset: 0x0002E37C
		public bool TryRegisterFileIfNew(Tuple<VirtualDirectory, ModContentPack, string> dir, string filePath)
		{
			if (!filePath.StartsWith(dir.Item3))
			{
				Log.Error("Failed to get a relative path for a file: " + filePath + ", located in " + dir.Item3, false);
				return false;
			}
			string item = filePath.Substring(dir.Item3.Length);
			if (!this.tmpAlreadyLoadedFiles.ContainsKey(dir.Item2))
			{
				this.tmpAlreadyLoadedFiles[dir.Item2] = new HashSet<string>();
			}
			else if (this.tmpAlreadyLoadedFiles[dir.Item2].Contains(item))
			{
				return false;
			}
			this.tmpAlreadyLoadedFiles[dir.Item2].Add(item);
			return true;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00030228 File Offset: 0x0002E428
		private void LoadFromFile_Strings(VirtualFile file, VirtualDirectory stringsTopDir)
		{
			string text;
			try
			{
				text = file.ReadAllText();
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from strings file ",
					file,
					": ",
					ex
				}));
				return;
			}
			string text2 = file.FullPath;
			if (stringsTopDir != null)
			{
				text2 = text2.Substring(stringsTopDir.FullPath.Length + 1);
			}
			text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
			text2 = text2.Replace('\\', '/');
			List<string> list = new List<string>();
			foreach (string item in GenText.LinesFromString(text))
			{
				list.Add(item);
			}
			List<string> list2;
			if (this.stringFiles.TryGetValue(text2, out list2))
			{
				using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string item2 = enumerator2.Current;
						list2.Add(item2);
					}
					return;
				}
			}
			this.stringFiles.Add(text2, list);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00030370 File Offset: 0x0002E570
		private void LoadFromFile_Keyed(VirtualFile file)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(file))
				{
					if (this.keyedReplacements.ContainsKey(xmlKeyValuePair.key) || dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.loadErrors.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from translation file ",
					file,
					": ",
					ex
				}));
				dictionary.Clear();
				dictionary2.Clear();
				this.anyKeyedReplacementsXmlParseError = true;
				this.lastKeyedReplacementsXmlParseErrorInFile = file.Name;
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string text = keyValuePair.Value;
				LoadedLanguage.KeyedReplacement keyedReplacement = new LoadedLanguage.KeyedReplacement();
				if (text == "TODO")
				{
					keyedReplacement.isPlaceholder = true;
					text = "";
				}
				keyedReplacement.key = keyValuePair.Key;
				keyedReplacement.value = text;
				keyedReplacement.fileSource = file.Name;
				keyedReplacement.fileSourceLine = dictionary2[keyValuePair.Key];
				keyedReplacement.fileSourceFullPath = file.FullPath;
				this.keyedReplacements.Add(keyValuePair.Key, keyedReplacement);
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00030554 File Offset: 0x0002E754
		public void LoadFromFile_DefInject(VirtualFile file, Type defType)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault<DefInjectionPackage>();
			if (defInjectionPackage == null)
			{
				defInjectionPackage = new DefInjectionPackage(defType);
				this.defInjections.Add(defInjectionPackage);
			}
			bool flag;
			defInjectionPackage.AddDataFromFile(file, out flag);
			if (flag)
			{
				this.anyDefInjectionsXmlParseError = true;
				this.lastDefInjectionsXmlParseErrorInFile = file.Name;
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000305C8 File Offset: 0x0002E7C8
		private void EnsureAllDefTypesHaveDefInjectionPackage()
		{
			using (IEnumerator<Type> enumerator = GenDefDatabase.AllDefTypesWithDatabases().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type defType = enumerator.Current;
					if (!this.defInjections.Any((DefInjectionPackage x) => x.defType == defType))
					{
						this.defInjections.Add(new DefInjectionPackage(defType));
					}
				}
			}
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00030648 File Offset: 0x0002E848
		public bool HaveTextForKey(string key, bool allowPlaceholders = false)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			return key != null && this.keyedReplacements.TryGetValue(key, out keyedReplacement) && (allowPlaceholders || !keyedReplacement.isPlaceholder);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00030688 File Offset: 0x0002E888
		public bool TryGetTextFromKey(string key, out TaggedString translated)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (key == null)
			{
				translated = key;
				return false;
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement) || keyedReplacement.isPlaceholder)
			{
				translated = key;
				return false;
			}
			translated = keyedReplacement.value;
			return true;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x000306EC File Offset: 0x0002E8EC
		public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (!this.stringFiles.TryGetValue(fileName, out stringsList))
			{
				stringsList = null;
				return false;
			}
			return true;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00030714 File Offset: 0x0002E914
		public string GetKeySourceFileAndLine(string key)
		{
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement))
			{
				return "unknown";
			}
			return keyedReplacement.fileSource + ":" + keyedReplacement.fileSourceLine;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00030752 File Offset: 0x0002E952
		public Gender ResolveGender(string str, string fallback = null)
		{
			return this.wordInfo.ResolveGender(str, fallback);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00030764 File Offset: 0x0002E964
		public void InjectIntoData_BeforeImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(false);
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg, false);
				}
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000307E8 File Offset: 0x0002E9E8
		public void InjectIntoData_AfterImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			int num = this.loadErrors.Count;
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(true);
					num += defInjectionPackage.loadErrors.Count;
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg, false);
				}
			}
			BackstoryTranslationUtility.LoadAndInjectBackstoryData(this.AllDirectories, this.backstoriesLoadErrors);
			num += this.backstoriesLoadErrors.Count;
			if (num != 0)
			{
				this.anyError = true;
				Log.Warning(string.Concat(new object[]
				{
					"Translation data for language ",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish,
					" has ",
					num,
					" errors. Generate translation report for more info."
				}), false);
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x000308F0 File Offset: 0x0002EAF0
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		// Token: 0x0400077E RID: 1918
		public string folderName;

		// Token: 0x0400077F RID: 1919
		public LanguageInfo info;

		// Token: 0x04000780 RID: 1920
		private LanguageWorker workerInt;

		// Token: 0x04000781 RID: 1921
		private LanguageWordInfo wordInfo = new LanguageWordInfo();

		// Token: 0x04000782 RID: 1922
		private bool dataIsLoaded;

		// Token: 0x04000783 RID: 1923
		public List<string> loadErrors = new List<string>();

		// Token: 0x04000784 RID: 1924
		public List<string> backstoriesLoadErrors = new List<string>();

		// Token: 0x04000785 RID: 1925
		public bool anyKeyedReplacementsXmlParseError;

		// Token: 0x04000786 RID: 1926
		public string lastKeyedReplacementsXmlParseErrorInFile;

		// Token: 0x04000787 RID: 1927
		public bool anyDefInjectionsXmlParseError;

		// Token: 0x04000788 RID: 1928
		public string lastDefInjectionsXmlParseErrorInFile;

		// Token: 0x04000789 RID: 1929
		public bool anyError;

		// Token: 0x0400078A RID: 1930
		private string legacyFolderName;

		// Token: 0x0400078B RID: 1931
		private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

		// Token: 0x0400078C RID: 1932
		public Texture2D icon = BaseContent.BadTex;

		// Token: 0x0400078D RID: 1933
		public Dictionary<string, LoadedLanguage.KeyedReplacement> keyedReplacements = new Dictionary<string, LoadedLanguage.KeyedReplacement>();

		// Token: 0x0400078E RID: 1934
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		// Token: 0x0400078F RID: 1935
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		// Token: 0x04000790 RID: 1936
		public const string OldKeyedTranslationsFolderName = "CodeLinked";

		// Token: 0x04000791 RID: 1937
		public const string KeyedTranslationsFolderName = "Keyed";

		// Token: 0x04000792 RID: 1938
		public const string OldDefInjectionsFolderName = "DefLinked";

		// Token: 0x04000793 RID: 1939
		public const string DefInjectionsFolderName = "DefInjected";

		// Token: 0x04000794 RID: 1940
		public const string LanguagesFolderName = "Languages";

		// Token: 0x04000795 RID: 1941
		public const string PlaceholderText = "TODO";

		// Token: 0x04000796 RID: 1942
		private bool infoIsRealMetadata;

		// Token: 0x0200139C RID: 5020
		public class KeyedReplacement
		{
			// Token: 0x04004A83 RID: 19075
			public string key;

			// Token: 0x04004A84 RID: 19076
			public string value;

			// Token: 0x04004A85 RID: 19077
			public string fileSource;

			// Token: 0x04004A86 RID: 19078
			public int fileSourceLine;

			// Token: 0x04004A87 RID: 19079
			public string fileSourceFullPath;

			// Token: 0x04004A88 RID: 19080
			public bool isPlaceholder;
		}
	}
}
