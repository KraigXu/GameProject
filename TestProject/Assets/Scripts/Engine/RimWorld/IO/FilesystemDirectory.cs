using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020012A2 RID: 4770
	internal class FilesystemDirectory : VirtualDirectory
	{
		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06007094 RID: 28820 RVA: 0x00274563 File Offset: 0x00272763
		public override string Name
		{
			get
			{
				return this.dirInfo.Name;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06007095 RID: 28821 RVA: 0x00274570 File Offset: 0x00272770
		public override string FullPath
		{
			get
			{
				return this.dirInfo.FullName;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06007096 RID: 28822 RVA: 0x0027457D File Offset: 0x0027277D
		public override bool Exists
		{
			get
			{
				return this.dirInfo.Exists;
			}
		}

		// Token: 0x06007097 RID: 28823 RVA: 0x0027458A File Offset: 0x0027278A
		public FilesystemDirectory(string dir)
		{
			this.dirInfo = new DirectoryInfo(dir);
		}

		// Token: 0x06007098 RID: 28824 RVA: 0x0027459E File Offset: 0x0027279E
		public FilesystemDirectory(DirectoryInfo dir)
		{
			this.dirInfo = dir;
		}

		// Token: 0x06007099 RID: 28825 RVA: 0x002745AD File Offset: 0x002727AD
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			foreach (DirectoryInfo dir in this.dirInfo.GetDirectories(searchPattern, searchOption))
			{
				yield return new FilesystemDirectory(dir);
			}
			DirectoryInfo[] array = null;
			yield break;
		}

		// Token: 0x0600709A RID: 28826 RVA: 0x002745CB File Offset: 0x002727CB
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			return new FilesystemDirectory(Path.Combine(this.FullPath, directoryName));
		}

		// Token: 0x0600709B RID: 28827 RVA: 0x002745DE File Offset: 0x002727DE
		public override VirtualFile GetFile(string filename)
		{
			return new FilesystemFile(new FileInfo(Path.Combine(this.FullPath, filename)));
		}

		// Token: 0x0600709C RID: 28828 RVA: 0x002745F6 File Offset: 0x002727F6
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			foreach (FileInfo fileInfo in this.dirInfo.GetFiles(searchPattern, searchOption))
			{
				yield return new FilesystemFile(fileInfo);
			}
			FileInfo[] array = null;
			yield break;
		}

		// Token: 0x04004528 RID: 17704
		private DirectoryInfo dirInfo;
	}
}
