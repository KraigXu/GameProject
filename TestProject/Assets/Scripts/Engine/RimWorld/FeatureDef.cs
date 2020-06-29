using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class FeatureDef : Def
	{
		
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

		
		public Type workerClass = typeof(FeatureWorker);

		
		public float order;

		
		public int minSize = 50;

		
		public int maxSize = int.MaxValue;

		
		public bool canTouchWorldEdge = true;

		
		public RulePackDef nameMaker;

		
		public int maxPossiblyAllowedSizeToTake = 30;

		
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		
		public int maxSpaceBetweenRootGroups = 5;

		
		public int minRootGroupsInCluster = 3;

		
		public int minRootGroupSize = 10;

		
		public int maxRootGroupSize = int.MaxValue;

		
		public int maxPassageWidth = 3;

		
		public float maxPctOfWholeArea = 0.1f;

		
		[Unsaved(false)]
		private FeatureWorker workerInt;
	}
}
