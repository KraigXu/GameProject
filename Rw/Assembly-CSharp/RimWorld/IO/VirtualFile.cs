using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020012A7 RID: 4775
	public abstract class VirtualFile
	{
		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x060070D4 RID: 28884
		public abstract string Name { get; }

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x060070D5 RID: 28885
		public abstract string FullPath { get; }

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x060070D6 RID: 28886
		public abstract bool Exists { get; }

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x060070D7 RID: 28887
		public abstract long Length { get; }

		// Token: 0x060070D8 RID: 28888
		public abstract Stream CreateReadStream();

		// Token: 0x060070D9 RID: 28889
		public abstract string ReadAllText();

		// Token: 0x060070DA RID: 28890
		public abstract string[] ReadAllLines();

		// Token: 0x060070DB RID: 28891
		public abstract byte[] ReadAllBytes();
	}
}
