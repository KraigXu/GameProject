using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001214 RID: 4628
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0025567C File Offset: 0x0025387C
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x00255684 File Offset: 0x00253884
		public override void GenerateFresh(string seed)
		{
			Find.World.features = new WorldFeatures();
			foreach (FeatureDef featureDef in from x in DefDatabase<FeatureDef>.AllDefsListForReading
			orderby x.order, x.index
			select x)
			{
				try
				{
					featureDef.Worker.GenerateWhereAppropriate();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate world features of def ",
						featureDef,
						": ",
						ex
					}), false);
				}
			}
		}
	}
}
