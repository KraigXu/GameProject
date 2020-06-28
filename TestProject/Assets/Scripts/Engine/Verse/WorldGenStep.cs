using System;

namespace Verse
{
	// Token: 0x02000102 RID: 258
	public abstract class WorldGenStep
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006FA RID: 1786
		public abstract int SeedPart { get; }

		// Token: 0x060006FB RID: 1787
		public abstract void GenerateFresh(string seed);

		// Token: 0x060006FC RID: 1788 RVA: 0x000201A3 File Offset: 0x0001E3A3
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GenerateFromScribe(string seed)
		{
		}

		// Token: 0x0400068B RID: 1675
		public WorldGenStepDef def;
	}
}
