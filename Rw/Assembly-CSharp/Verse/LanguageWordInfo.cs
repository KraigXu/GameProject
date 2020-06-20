using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000132 RID: 306
	public class LanguageWordInfo
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x0002D624 File Offset: 0x0002B824
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002D688 File Offset: 0x0002B888
		public Gender ResolveGender(string str, string fallback = null)
		{
			Gender result;
			if (!this.TryResolveGender(str, out result) && fallback != null)
			{
				this.TryResolveGender(str, out result);
			}
			return result;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002D6B0 File Offset: 0x0002B8B0
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

		// Token: 0x060008A5 RID: 2213 RVA: 0x0002D710 File Offset: 0x0002B910
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

		// Token: 0x0400076C RID: 1900
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		// Token: 0x0400076D RID: 1901
		private const string FolderName = "WordInfo";

		// Token: 0x0400076E RID: 1902
		private const string GendersFolderName = "Gender";

		// Token: 0x0400076F RID: 1903
		private const string MaleFileName = "Male.txt";

		// Token: 0x04000770 RID: 1904
		private const string FemaleFileName = "Female.txt";

		// Token: 0x04000771 RID: 1905
		private const string NeuterFileName = "Neuter.txt";

		// Token: 0x04000772 RID: 1906
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
