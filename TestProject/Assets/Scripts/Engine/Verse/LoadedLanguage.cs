using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	
	public class LoadedLanguage
	{
		
		
		public string DisplayName
		{
			get
			{
				return GenText.SplitCamelCase(this.folderName);
			}
		}

		
		
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
					
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		
		
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

		
		
		public string LegacyFolderName
		{
			get
			{
				return this.legacyFolderName;
			}
		}

		
		public LoadedLanguage(string folderName)
		{
			this.folderName = folderName;
			this.legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
		}

		
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
				List<string>.Enumerator enumerator2 = list.GetEnumerator();
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

		
		private void EnsureAllDefTypesHaveDefInjectionPackage()
		{
			IEnumerator<Type> enumerator = GenDefDatabase.AllDefTypesWithDatabases().GetEnumerator();
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

		
		public bool HaveTextForKey(string key, bool allowPlaceholders = false)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			return key != null && this.keyedReplacements.TryGetValue(key, out keyedReplacement) && (allowPlaceholders || !keyedReplacement.isPlaceholder);
		}

		
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

		
		public string GetKeySourceFileAndLine(string key)
		{
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement))
			{
				return "unknown";
			}
			return keyedReplacement.fileSource + ":" + keyedReplacement.fileSourceLine;
		}

		
		public Gender ResolveGender(string str, string fallback = null)
		{
			return this.wordInfo.ResolveGender(str, fallback);
		}

		
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

		
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		
		public string folderName;

		
		public LanguageInfo info;

		
		private LanguageWorker workerInt;

		
		private LanguageWordInfo wordInfo = new LanguageWordInfo();

		
		private bool dataIsLoaded;

		
		public List<string> loadErrors = new List<string>();

		
		public List<string> backstoriesLoadErrors = new List<string>();

		
		public bool anyKeyedReplacementsXmlParseError;

		
		public string lastKeyedReplacementsXmlParseErrorInFile;

		
		public bool anyDefInjectionsXmlParseError;

		
		public string lastDefInjectionsXmlParseErrorInFile;

		
		public bool anyError;

		
		private string legacyFolderName;

		
		private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

		
		public Texture2D icon = BaseContent.BadTex;

		
		public Dictionary<string, LoadedLanguage.KeyedReplacement> keyedReplacements = new Dictionary<string, LoadedLanguage.KeyedReplacement>();

		
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		
		public const string OldKeyedTranslationsFolderName = "CodeLinked";

		
		public const string KeyedTranslationsFolderName = "Keyed";

		
		public const string OldDefInjectionsFolderName = "DefLinked";

		
		public const string DefInjectionsFolderName = "DefInjected";

		
		public const string LanguagesFolderName = "Languages";

		
		public const string PlaceholderText = "TODO";

		
		private bool infoIsRealMetadata;

		
		public class KeyedReplacement
		{
			
			public string key;

			
			public string value;

			
			public string fileSource;

			
			public int fileSourceLine;

			
			public string fileSourceFullPath;

			
			public bool isPlaceholder;
		}
	}
}
