using System;

namespace Verse
{
	// Token: 0x02000101 RID: 257
	public class WorldGenStepDef : Def
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0002018F File Offset: 0x0001E38F
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x04000689 RID: 1673
		public float order;

		// Token: 0x0400068A RID: 1674
		public WorldGenStep worldGenStep;
	}
}
