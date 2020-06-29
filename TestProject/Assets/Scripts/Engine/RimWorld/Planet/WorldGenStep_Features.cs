using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldGenStep_Features : WorldGenStep
	{
		
		// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0025567C File Offset: 0x0025387C
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		
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
