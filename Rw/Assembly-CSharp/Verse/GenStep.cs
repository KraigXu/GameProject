using System;

namespace Verse
{
	// Token: 0x020000B3 RID: 179
	public abstract class GenStep
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000574 RID: 1396
		public abstract int SeedPart { get; }

		// Token: 0x06000575 RID: 1397
		public abstract void Generate(Map map, GenStepParams parms);

		// Token: 0x040003A9 RID: 937
		public GenStepDef def;
	}
}
