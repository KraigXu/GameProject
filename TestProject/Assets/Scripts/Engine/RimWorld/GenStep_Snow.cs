using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A5B RID: 2651
	public class GenStep_Snow : GenStep
	{
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003EA9 RID: 16041 RVA: 0x0014C458 File Offset: 0x0014A658
		public override int SeedPart
		{
			get
			{
				return 306693816;
			}
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x0014C460 File Offset: 0x0014A660
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			for (int i = (int)(GenLocalDate.Twelfth(map) - Twelfth.Third); i <= (int)GenLocalDate.Twelfth(map); i++)
			{
				int num2 = i;
				if (num2 < 0)
				{
					num2 += 12;
				}
				Twelfth twelfth = (Twelfth)num2;
				if (GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth) < 0f)
				{
					num++;
				}
			}
			float num3 = 0f;
			switch (num)
			{
			case 0:
				return;
			case 1:
				num3 = 0.3f;
				break;
			case 2:
				num3 = 0.7f;
				break;
			case 3:
				num3 = 1f;
				break;
			}
			if (map.mapTemperature.SeasonalTemp > 0f)
			{
				num3 *= 0.4f;
			}
			if ((double)num3 < 0.3)
			{
				return;
			}
			foreach (IntVec3 c in map.AllCells)
			{
				if (!c.Roofed(map))
				{
					map.steadyEnvironmentEffects.AddFallenSnowAt(c, num3);
				}
			}
		}
	}
}
