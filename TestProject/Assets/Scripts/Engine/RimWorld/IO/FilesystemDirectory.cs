using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	
	internal class FilesystemDirectory : VirtualDirectory
	{
		
		
		public override string Name
		{
			get
			{
				return this.dirInfo.Name;
			}
		}

		
		
		public override string FullPath
		{
			get
			{
				return this.dirInfo.FullName;
			}
		}

		
		
		public override bool Exists
		{
			get
			{
				return this.dirInfo.Exists;
			}
		}

		
		public FilesystemDirectory(string dir)
		{
			this.dirInfo = new DirectoryInfo(dir);
		}

		
		public FilesystemDirectory(DirectoryInfo dir)
		{
			this.dirInfo = dir;
		}

		
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			foreach (DirectoryInfo dir in this.dirInfo.GetDirectories(searchPattern, searchOption))
			{
				yield return new FilesystemDirectory(dir);
			}
			DirectoryInfo[] array = null;
			yield break;
		}

		
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			return new FilesystemDirectory(Path.Combine(this.FullPath, directoryName));
		}

		
		public override VirtualFile GetFile(string filename)
		{
			return new FilesystemFile(new FileInfo(Path.Combine(this.FullPath, filename)));
		}

		
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			foreach (FileInfo fileInfo in this.dirInfo.GetFiles(searchPattern, searchOption))
			{
				yield return new FilesystemFile(fileInfo);
			}
			FileInfo[] array = null;
			yield break;
		}

		
		private DirectoryInfo dirInfo;
	}
}
