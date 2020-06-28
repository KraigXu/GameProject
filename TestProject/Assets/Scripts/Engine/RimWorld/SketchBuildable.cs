using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA4 RID: 2724
	public abstract class SketchBuildable : SketchEntity
	{
		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004058 RID: 16472
		public abstract BuildableDef Buildable { get; }

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004059 RID: 16473
		public abstract ThingDef Stuff { get; }

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x00158399 File Offset: 0x00156599
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x001583AD File Offset: 0x001565AD
		public override bool LostImportantReferences
		{
			get
			{
				return this.Buildable == null;
			}
		}

		// Token: 0x0600405C RID: 16476
		public abstract Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x0600405D RID: 16477 RVA: 0x001583B8 File Offset: 0x001565B8
		public override bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map)
		{
			return at.InBounds(map) && (this.IsSameSpawned(at, map) || this.GetSpawnedBlueprintOrFrame(at, map) != null);
		}
	}
}
