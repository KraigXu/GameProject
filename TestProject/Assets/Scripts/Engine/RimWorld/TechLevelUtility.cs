using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2E RID: 2606
	public static class TechLevelUtility
	{
		// Token: 0x06003D9C RID: 15772 RVA: 0x00145B18 File Offset: 0x00143D18
		public static string ToStringHuman(this TechLevel tl)
		{
			switch (tl)
			{
			case TechLevel.Undefined:
				return "Undefined".Translate();
			case TechLevel.Animal:
				return "TechLevel_Animal".Translate();
			case TechLevel.Neolithic:
				return "TechLevel_Neolithic".Translate();
			case TechLevel.Medieval:
				return "TechLevel_Medieval".Translate();
			case TechLevel.Industrial:
				return "TechLevel_Industrial".Translate();
			case TechLevel.Spacer:
				return "TechLevel_Spacer".Translate();
			case TechLevel.Ultra:
				return "TechLevel_Ultra".Translate();
			case TechLevel.Archotech:
				return "TechLevel_Archotech".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x00145BD8 File Offset: 0x00143DD8
		public static bool CanSpawnWithEquipmentFrom(this TechLevel pawnLevel, TechLevel gearLevel)
		{
			if (gearLevel == TechLevel.Undefined)
			{
				return false;
			}
			switch (pawnLevel)
			{
			case TechLevel.Undefined:
				return false;
			case TechLevel.Neolithic:
				return gearLevel <= TechLevel.Neolithic;
			case TechLevel.Medieval:
				return gearLevel <= TechLevel.Medieval;
			case TechLevel.Industrial:
				return gearLevel == TechLevel.Industrial;
			case TechLevel.Spacer:
				return gearLevel == TechLevel.Spacer || gearLevel == TechLevel.Industrial;
			case TechLevel.Ultra:
				return gearLevel == TechLevel.Ultra || gearLevel == TechLevel.Spacer;
			case TechLevel.Archotech:
				return gearLevel == TechLevel.Archotech;
			}
			Log.Error(string.Concat(new object[]
			{
				"Unknown tech levels ",
				pawnLevel,
				", ",
				gearLevel
			}), false);
			return true;
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00145C78 File Offset: 0x00143E78
		public static bool IsNeolithicOrWorse(this TechLevel techLevel)
		{
			return techLevel != TechLevel.Undefined && techLevel <= TechLevel.Neolithic;
		}
	}
}
