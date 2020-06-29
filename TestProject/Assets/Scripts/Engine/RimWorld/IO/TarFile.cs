using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	
	internal class TarFile : VirtualFile
	{
		
		
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		
		
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		
		
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		
		
		public override long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		
		public TarFile(byte[] data, string fullPath, string name)
		{
			this.data = data;
			this.fullPath = fullPath;
			this.name = name;
		}

		
		private TarFile()
		{
		}

		
		private void CheckAccess()
		{
			if (this.data == null)
			{
				throw new FileNotFoundException();
			}
		}

		
		public override Stream CreateReadStream()
		{
			this.CheckAccess();
			return new MemoryStream(this.ReadAllBytes());
		}

		
		public override byte[] ReadAllBytes()
		{
			this.CheckAccess();
			byte[] array = new byte[this.data.Length];
			Buffer.BlockCopy(this.data, 0, array, 0, this.data.Length);
			return array;
		}

		
		public override string[] ReadAllLines()
		{
			this.CheckAccess();
			List<string> list = new List<string>();
			MemoryStream memoryStream = new MemoryStream(this.data);
			{
				StreamReader streamReader = new StreamReader(memoryStream, true);
				{
					while (!streamReader.EndOfStream)
					{
						list.Add(streamReader.ReadLine());
					}
				}
			}
			return list.ToArray();
		}

		
		public override string ReadAllText()
		{
			this.CheckAccess();
			string result;
			MemoryStream memoryStream = new MemoryStream(this.data);
			{
				StreamReader streamReader = new StreamReader(memoryStream, true);
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		
		public override string ToString()
		{
			return string.Format("TarFile [{0}], Length {1}", this.fullPath, this.data.Length.ToString());
		}

		
		public static readonly TarFile NotFound = new TarFile();

		
		public byte[] data;

		
		public string fullPath;

		
		public string name;
	}
}
