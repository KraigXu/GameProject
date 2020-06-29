using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	
	internal class TarFile : VirtualFile
	{
		
		// (get) Token: 0x060070BC RID: 28860 RVA: 0x00274E76 File Offset: 0x00273076
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		
		// (get) Token: 0x060070BD RID: 28861 RVA: 0x00274E7E File Offset: 0x0027307E
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		
		// (get) Token: 0x060070BE RID: 28862 RVA: 0x00274E86 File Offset: 0x00273086
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		
		// (get) Token: 0x060070BF RID: 28863 RVA: 0x00274E91 File Offset: 0x00273091
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
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
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
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
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
