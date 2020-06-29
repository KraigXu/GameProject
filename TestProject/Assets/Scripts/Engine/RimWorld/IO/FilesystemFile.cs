using System;
using System.IO;

namespace RimWorld.IO
{
	
	internal class FilesystemFile : VirtualFile
	{
		
		// (get) Token: 0x0600709D RID: 28829 RVA: 0x00274614 File Offset: 0x00272814
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		
		// (get) Token: 0x0600709E RID: 28830 RVA: 0x00274621 File Offset: 0x00272821
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		
		// (get) Token: 0x0600709F RID: 28831 RVA: 0x0027462E File Offset: 0x0027282E
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		
		// (get) Token: 0x060070A0 RID: 28832 RVA: 0x0027463B File Offset: 0x0027283B
		public override long Length
		{
			get
			{
				return this.fileInfo.Length;
			}
		}

		
		public FilesystemFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}

		
		public override Stream CreateReadStream()
		{
			return this.fileInfo.OpenRead();
		}

		
		public override byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.fileInfo.FullName);
		}

		
		public override string[] ReadAllLines()
		{
			return File.ReadAllLines(this.fileInfo.FullName);
		}

		
		public override string ReadAllText()
		{
			return File.ReadAllText(this.fileInfo.FullName);
		}

		
		public static implicit operator FilesystemFile(FileInfo fileInfo)
		{
			return new FilesystemFile(fileInfo);
		}

		
		public override string ToString()
		{
			return string.Format("FilesystemFile [{0}], Length {1}", this.FullPath, this.fileInfo.Length.ToString());
		}

		
		private FileInfo fileInfo;
	}
}
