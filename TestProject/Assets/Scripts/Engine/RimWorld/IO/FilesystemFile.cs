using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020012A3 RID: 4771
	internal class FilesystemFile : VirtualFile
	{
		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x0600709D RID: 28829 RVA: 0x00274614 File Offset: 0x00272814
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x0600709E RID: 28830 RVA: 0x00274621 File Offset: 0x00272821
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x0600709F RID: 28831 RVA: 0x0027462E File Offset: 0x0027282E
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x060070A0 RID: 28832 RVA: 0x0027463B File Offset: 0x0027283B
		public override long Length
		{
			get
			{
				return this.fileInfo.Length;
			}
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x00274648 File Offset: 0x00272848
		public FilesystemFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x00274657 File Offset: 0x00272857
		public override Stream CreateReadStream()
		{
			return this.fileInfo.OpenRead();
		}

		// Token: 0x060070A3 RID: 28835 RVA: 0x00274664 File Offset: 0x00272864
		public override byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.fileInfo.FullName);
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x00274676 File Offset: 0x00272876
		public override string[] ReadAllLines()
		{
			return File.ReadAllLines(this.fileInfo.FullName);
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x00274688 File Offset: 0x00272888
		public override string ReadAllText()
		{
			return File.ReadAllText(this.fileInfo.FullName);
		}

		// Token: 0x060070A6 RID: 28838 RVA: 0x0027469A File Offset: 0x0027289A
		public static implicit operator FilesystemFile(FileInfo fileInfo)
		{
			return new FilesystemFile(fileInfo);
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x002746A4 File Offset: 0x002728A4
		public override string ToString()
		{
			return string.Format("FilesystemFile [{0}], Length {1}", this.FullPath, this.fileInfo.Length.ToString());
		}

		// Token: 0x04004529 RID: 17705
		private FileInfo fileInfo;
	}
}
