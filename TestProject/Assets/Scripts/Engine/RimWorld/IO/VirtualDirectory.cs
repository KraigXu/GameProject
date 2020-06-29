using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	
	public abstract class VirtualDirectory
	{
		
		// (get) Token: 0x060070C9 RID: 28873
		public abstract string Name { get; }

		
		// (get) Token: 0x060070CA RID: 28874
		public abstract string FullPath { get; }

		
		// (get) Token: 0x060070CB RID: 28875
		public abstract bool Exists { get; }

		
		public abstract VirtualDirectory GetDirectory(string directoryName);

		
		public abstract VirtualFile GetFile(string filename);

		
		public abstract IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption);

		
		public abstract IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption);

		
		public string ReadAllText(string filename)
		{
			return this.GetFile(filename).ReadAllText();
		}

		
		public bool FileExists(string filename)
		{
			return this.GetFile(filename).Exists;
		}

		
		public override string ToString()
		{
			return this.FullPath;
		}
	}
}
