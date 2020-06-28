using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020012A5 RID: 4773
	internal class TarFile : VirtualFile
	{
		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x060070BC RID: 28860 RVA: 0x00274E76 File Offset: 0x00273076
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x060070BD RID: 28861 RVA: 0x00274E7E File Offset: 0x0027307E
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x060070BE RID: 28862 RVA: 0x00274E86 File Offset: 0x00273086
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x060070BF RID: 28863 RVA: 0x00274E91 File Offset: 0x00273091
		public override long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		// Token: 0x060070C0 RID: 28864 RVA: 0x00274E9C File Offset: 0x0027309C
		public TarFile(byte[] data, string fullPath, string name)
		{
			this.data = data;
			this.fullPath = fullPath;
			this.name = name;
		}

		// Token: 0x060070C1 RID: 28865 RVA: 0x00274EB9 File Offset: 0x002730B9
		private TarFile()
		{
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x00274EC1 File Offset: 0x002730C1
		private void CheckAccess()
		{
			if (this.data == null)
			{
				throw new FileNotFoundException();
			}
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x00274ED1 File Offset: 0x002730D1
		public override Stream CreateReadStream()
		{
			this.CheckAccess();
			return new MemoryStream(this.ReadAllBytes());
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x00274EE4 File Offset: 0x002730E4
		public override byte[] ReadAllBytes()
		{
			this.CheckAccess();
			byte[] array = new byte[this.data.Length];
			Buffer.BlockCopy(this.data, 0, array, 0, this.data.Length);
			return array;
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x00274F1C File Offset: 0x0027311C
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

		// Token: 0x060070C6 RID: 28870 RVA: 0x00274F98 File Offset: 0x00273198
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

		// Token: 0x060070C7 RID: 28871 RVA: 0x00274FFC File Offset: 0x002731FC
		public override string ToString()
		{
			return string.Format("TarFile [{0}], Length {1}", this.fullPath, this.data.Length.ToString());
		}

		// Token: 0x04004533 RID: 17715
		public static readonly TarFile NotFound = new TarFile();

		// Token: 0x04004534 RID: 17716
		public byte[] data;

		// Token: 0x04004535 RID: 17717
		public string fullPath;

		// Token: 0x04004536 RID: 17718
		public string name;
	}
}
