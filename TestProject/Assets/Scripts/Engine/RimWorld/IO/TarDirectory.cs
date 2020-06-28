using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Tar;

namespace RimWorld.IO
{
	// Token: 0x020012A4 RID: 4772
	internal class TarDirectory : VirtualDirectory
	{
		// Token: 0x060070A8 RID: 28840 RVA: 0x002746D4 File Offset: 0x002728D4
		public static void ClearCache()
		{
			TarDirectory.cache.Clear();
		}

		// Token: 0x060070A9 RID: 28841 RVA: 0x002746E0 File Offset: 0x002728E0
		public static TarDirectory ReadFromFileOrCache(string file)
		{
			string key = file.Replace('\\', '/');
			TarDirectory tarDirectory;
			if (!TarDirectory.cache.TryGetValue(key, out tarDirectory))
			{
				tarDirectory = new TarDirectory(file, "");
				tarDirectory.lazyLoadArchive = file;
				TarDirectory.cache.Add(key, tarDirectory);
			}
			return tarDirectory;
		}

		// Token: 0x060070AA RID: 28842 RVA: 0x00274728 File Offset: 0x00272928
		private void CheckLazyLoad()
		{
			if (this.lazyLoadArchive != null)
			{
				using (FileStream fileStream = File.OpenRead(this.lazyLoadArchive))
				{
					using (TarInputStream tarInputStream = new TarInputStream(fileStream))
					{
						TarDirectory.ParseTAR(this, tarInputStream, this.lazyLoadArchive);
					}
				}
				this.lazyLoadArchive = null;
			}
		}

		// Token: 0x060070AB RID: 28843 RVA: 0x00274798 File Offset: 0x00272998
		private static void ParseTAR(TarDirectory root, TarInputStream input, string fullPath)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			List<TarEntry> list = new List<TarEntry>();
			List<TarDirectory> list2 = new List<TarDirectory>();
			byte[] buffer = new byte[16384];
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					TarEntry nextEntry;
					while ((nextEntry = input.GetNextEntry()) != null)
					{
						TarDirectory.ReadTarEntryData(input, memoryStream, buffer);
						dictionary.Add(nextEntry.Name, memoryStream.ToArray());
						list.Add(nextEntry);
						memoryStream.Position = 0L;
						memoryStream.SetLength(0L);
					}
				}
				list2.Add(root);
				foreach (TarEntry tarEntry in from e in list
				where e.IsDirectory && !string.IsNullOrEmpty(e.Name)
				select e)
				{
					string str = TarDirectory.FormatFolderPath(tarEntry.Name);
					list2.Add(new TarDirectory(fullPath + "/" + str, str));
				}
				foreach (TarEntry tarEntry2 in from e in list
				where !e.IsDirectory
				select e)
				{
					string b = TarDirectory.FormatFolderPath(Path.GetDirectoryName(tarEntry2.Name));
					TarDirectory tarDirectory = null;
					foreach (TarDirectory tarDirectory2 in list2)
					{
						if (tarDirectory2.inArchiveFullPath == b)
						{
							tarDirectory = tarDirectory2;
							break;
						}
					}
					tarDirectory.files.Add(new TarFile(dictionary[tarEntry2.Name], fullPath + "/" + tarEntry2.Name, Path.GetFileName(tarEntry2.Name)));
				}
				foreach (TarDirectory tarDirectory3 in list2)
				{
					if (!string.IsNullOrEmpty(tarDirectory3.inArchiveFullPath))
					{
						string b2 = TarDirectory.FormatFolderPath(Path.GetDirectoryName(tarDirectory3.inArchiveFullPath));
						TarDirectory tarDirectory4 = null;
						foreach (TarDirectory tarDirectory5 in list2)
						{
							if (tarDirectory5.inArchiveFullPath == b2)
							{
								tarDirectory4 = tarDirectory5;
								break;
							}
						}
						tarDirectory4.subDirectories.Add(tarDirectory3);
					}
				}
			}
			finally
			{
				input.Close();
			}
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x00274AD4 File Offset: 0x00272CD4
		private static string FormatFolderPath(string str)
		{
			if (str.Length == 0)
			{
				return str;
			}
			if (str.IndexOf('\\') != -1)
			{
				str = str.Replace('\\', '/');
			}
			if (str[str.Length - 1] == '/')
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}

		// Token: 0x060070AD RID: 28845 RVA: 0x00274B28 File Offset: 0x00272D28
		private static void ReadTarEntryData(TarInputStream tarIn, Stream outStream, byte[] buffer = null)
		{
			if (buffer == null)
			{
				buffer = new byte[4096];
			}
			for (int i = tarIn.Read(buffer, 0, buffer.Length); i > 0; i = tarIn.Read(buffer, 0, buffer.Length))
			{
				outStream.Write(buffer, 0, i);
			}
		}

		// Token: 0x060070AE RID: 28846 RVA: 0x00274B6B File Offset: 0x00272D6B
		private static IEnumerable<TarDirectory> EnumerateAllChildrenRecursive(TarDirectory of)
		{
			foreach (TarDirectory dir in of.subDirectories)
			{
				yield return dir;
				foreach (TarDirectory tarDirectory in TarDirectory.EnumerateAllChildrenRecursive(dir))
				{
					yield return tarDirectory;
				}
				IEnumerator<TarDirectory> enumerator2 = null;
				dir = null;
			}
			List<TarDirectory>.Enumerator enumerator = default(List<TarDirectory>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060070AF RID: 28847 RVA: 0x00274B7B File Offset: 0x00272D7B
		private static IEnumerable<TarFile> EnumerateAllFilesRecursive(TarDirectory of)
		{
			foreach (TarFile tarFile in of.files)
			{
				yield return tarFile;
			}
			List<TarFile>.Enumerator enumerator = default(List<TarFile>.Enumerator);
			foreach (TarDirectory of2 in of.subDirectories)
			{
				foreach (TarFile tarFile2 in TarDirectory.EnumerateAllFilesRecursive(of2))
				{
					yield return tarFile2;
				}
				IEnumerator<TarFile> enumerator3 = null;
			}
			List<TarDirectory>.Enumerator enumerator2 = default(List<TarDirectory>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060070B0 RID: 28848 RVA: 0x00274B8C File Offset: 0x00272D8C
		private static Func<string, bool> GetPatternMatcher(string searchPattern)
		{
			Func<string, bool> func = null;
			if (searchPattern.Length == 1 && searchPattern[0] == '*')
			{
				func = ((string str) => true);
			}
			else if (searchPattern.Length > 2 && searchPattern[0] == '*' && searchPattern[1] == '.')
			{
				string extension = searchPattern.Substring(2);
				func = ((string str) => str.Substring(str.Length - extension.Length) == extension);
			}
			if (func == null)
			{
				func = ((string str) => false);
			}
			return func;
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x060070B1 RID: 28849 RVA: 0x00274C32 File Offset: 0x00272E32
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x060070B2 RID: 28850 RVA: 0x00274C3A File Offset: 0x00272E3A
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x060070B3 RID: 28851 RVA: 0x00274C42 File Offset: 0x00272E42
		public override bool Exists
		{
			get
			{
				return this.exists;
			}
		}

		// Token: 0x060070B4 RID: 28852 RVA: 0x00274C4A File Offset: 0x00272E4A
		private TarDirectory(string fullPath, string inArchiveFullPath)
		{
			this.name = Path.GetFileNameWithoutExtension(fullPath);
			this.fullPath = fullPath;
			this.inArchiveFullPath = inArchiveFullPath;
			this.exists = true;
		}

		// Token: 0x060070B5 RID: 28853 RVA: 0x00274C89 File Offset: 0x00272E89
		private TarDirectory()
		{
			this.exists = false;
		}

		// Token: 0x060070B6 RID: 28854 RVA: 0x00274CB0 File Offset: 0x00272EB0
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			this.CheckLazyLoad();
			string text = directoryName;
			if (!string.IsNullOrEmpty(this.fullPath))
			{
				text = this.fullPath + "/" + text;
			}
			foreach (TarDirectory tarDirectory in this.subDirectories)
			{
				if (tarDirectory.fullPath == text)
				{
					return tarDirectory;
				}
			}
			return TarDirectory.NotFound;
		}

		// Token: 0x060070B7 RID: 28855 RVA: 0x00274D3C File Offset: 0x00272F3C
		public override VirtualFile GetFile(string filename)
		{
			this.CheckLazyLoad();
			VirtualDirectory virtualDirectory = this;
			string[] array = filename.Split(new char[]
			{
				'/',
				'\\'
			});
			for (int i = 0; i < array.Length - 1; i++)
			{
				virtualDirectory = virtualDirectory.GetDirectory(array[i]);
			}
			filename = array[array.Length - 1];
			if (virtualDirectory == this)
			{
				using (List<TarFile>.Enumerator enumerator = this.files.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TarFile tarFile = enumerator.Current;
						if (tarFile.Name == filename)
						{
							return tarFile;
						}
					}
					goto IL_93;
				}
				goto IL_8B;
				IL_93:
				return TarFile.NotFound;
			}
			IL_8B:
			return virtualDirectory.GetFile(filename);
		}

		// Token: 0x060070B8 RID: 28856 RVA: 0x00274DF4 File Offset: 0x00272FF4
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			this.CheckLazyLoad();
			IEnumerable<TarFile> enumerable = this.files;
			if (searchOption == SearchOption.AllDirectories)
			{
				enumerable = TarDirectory.EnumerateAllFilesRecursive(this);
			}
			Func<string, bool> matcher = TarDirectory.GetPatternMatcher(searchPattern);
			foreach (TarFile tarFile in enumerable)
			{
				if (matcher(tarFile.Name))
				{
					yield return tarFile;
				}
			}
			IEnumerator<TarFile> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060070B9 RID: 28857 RVA: 0x00274E12 File Offset: 0x00273012
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			this.CheckLazyLoad();
			IEnumerable<TarDirectory> enumerable = this.subDirectories;
			if (searchOption == SearchOption.AllDirectories)
			{
				enumerable = TarDirectory.EnumerateAllChildrenRecursive(this);
			}
			Func<string, bool> matcher = TarDirectory.GetPatternMatcher(searchPattern);
			foreach (TarDirectory tarDirectory in enumerable)
			{
				if (matcher(tarDirectory.Name))
				{
					yield return tarDirectory;
				}
			}
			IEnumerator<TarDirectory> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060070BA RID: 28858 RVA: 0x00274E30 File Offset: 0x00273030
		public override string ToString()
		{
			return string.Format("TarDirectory [{0}], {1} files", this.fullPath, this.files.Count.ToString());
		}

		// Token: 0x0400452A RID: 17706
		private static Dictionary<string, TarDirectory> cache = new Dictionary<string, TarDirectory>();

		// Token: 0x0400452B RID: 17707
		private string lazyLoadArchive;

		// Token: 0x0400452C RID: 17708
		private static readonly TarDirectory NotFound = new TarDirectory();

		// Token: 0x0400452D RID: 17709
		private string fullPath;

		// Token: 0x0400452E RID: 17710
		private string inArchiveFullPath;

		// Token: 0x0400452F RID: 17711
		private string name;

		// Token: 0x04004530 RID: 17712
		private bool exists;

		// Token: 0x04004531 RID: 17713
		public List<TarDirectory> subDirectories = new List<TarDirectory>();

		// Token: 0x04004532 RID: 17714
		public List<TarFile> files = new List<TarFile>();
	}
}
