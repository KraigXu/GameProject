using System;
using System.IO;

namespace RimWorld.IO
{
	
	internal class FilesystemFile : VirtualFile
	{
		
		
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		
		
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		
		
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		
		
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
