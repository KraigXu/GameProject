using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C1 RID: 2241
	public class FeatureDef : Def
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060035FF RID: 13823 RVA: 0x0012539E File Offset: 0x0012359E
		public FeatureWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (FeatureWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04001E21 RID: 7713
		public Type workerClass = typeof(FeatureWorker);

		// Token: 0x04001E22 RID: 7714
		public float order;

		// Token: 0x04001E23 RID: 7715
		public int minSize = 50;

		// Token: 0x04001E24 RID: 7716
		public int maxSize = int.MaxValue;

		// Token: 0x04001E25 RID: 7717
		public bool canTouchWorldEdge = true;

		// Token: 0x04001E26 RID: 7718
		public RulePackDef nameMaker;

		// Token: 0x04001E27 RID: 7719
		public int maxPossiblyAllowedSizeToTake = 30;

		// Token: 0x04001E28 RID: 7720
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		// Token: 0x04001E29 RID: 7721
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		// Token: 0x04001E2A RID: 7722
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		// Token: 0x04001E2B RID: 7723
		public int maxSpaceBetweenRootGroups = 5;

		// Token: 0x04001E2C RID: 7724
		public int minRootGroupsInCluster = 3;

		// Token: 0x04001E2D RID: 7725
		public int minRootGroupSize = 10;

		// Token: 0x04001E2E RID: 7726
		public int maxRootGroupSize = int.MaxValue;

		// Token: 0x04001E2F RID: 7727
		public int maxPassageWidth = 3;

		// Token: 0x04001E30 RID: 7728
		public float maxPctOfWholeArea = 0.1f;

		// Token: 0x04001E31 RID: 7729
		[Unsaved(false)]
		private FeatureWorker workerInt;
	}
}
