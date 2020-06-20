using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020012A6 RID: 4774
	public abstract class VirtualDirectory
	{
		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x060070C9 RID: 28873
		public abstract string Name { get; }

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x060070CA RID: 28874
		public abstract string FullPath { get; }

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x060070CB RID: 28875
		public abstract bool Exists { get; }

		// Token: 0x060070CC RID: 28876
		public abstract VirtualDirectory GetDirectory(string directoryName);

		// Token: 0x060070CD RID: 28877
		public abstract VirtualFile GetFile(string filename);

		// Token: 0x060070CE RID: 28878
		public abstract IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption);

		// Token: 0x060070CF RID: 28879
		public abstract IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption);

		// Token: 0x060070D0 RID: 28880 RVA: 0x00275035 File Offset: 0x00273235
		public string ReadAllText(string filename)
		{
			return this.GetFile(filename).ReadAllText();
		}

		// Token: 0x060070D1 RID: 28881 RVA: 0x00275043 File Offset: 0x00273243
		public bool FileExists(string filename)
		{
			return this.GetFile(filename).Exists;
		}

		// Token: 0x060070D2 RID: 28882 RVA: 0x00275051 File Offset: 0x00273251
		public override string ToString()
		{
			return this.FullPath;
		}
	}
}
