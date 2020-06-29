using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	
	public class LanguageWordInfo
	{
		
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		
		public Gender ResolveGender(string str, string fallback = null)
		{
			Gender result;
			if (!this.TryResolveGender(str, out result) && fallback != null)
			{
				this.TryResolveGender(str, out result);
			}
			return result;
		}

		
		private bool TryResolveGender(string str, out Gender gender)
		{
			LanguageWordInfo.tmpLowercase.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				LanguageWordInfo.tmpLowercase.Append(char.ToLower(str[i]));
			}
			string key = LanguageWordInfo.tmpLowercase.ToString();
			if (this.genders.TryGetValue(key, out gender))
			{
				return true;
			}
			gender = Gender.Male;
			return false;
		}

		
		private void TryLoadFromFile(VirtualFile file, Gender gender, Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			string[] array;
			try
			{
				array = file.ReadAllLines();
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (FileNotFoundException)
			{
				return;
			}
			if (!lang.TryRegisterFileIfNew(dir, file.FullPath))
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].NullOrEmpty() && !this.genders.ContainsKey(array[i]))
				{
					this.genders.Add(array[i], gender);
				}
			}
		}

		
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		
		private const string FolderName = "WordInfo";

		
		private const string GendersFolderName = "Gender";

		
		private const string MaleFileName = "Male.txt";

		
		private const string FemaleFileName = "Female.txt";

		
		private const string NeuterFileName = "Neuter.txt";

		
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
