using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC1 RID: 4033
	public static class GenStuff
	{
		// Token: 0x060060EF RID: 24815 RVA: 0x00219568 File Offset: 0x00217768
		public static ThingDef DefaultStuffFor(BuildableDef bd)
		{
			if (!bd.MadeFromStuff)
			{
				return null;
			}
			ThingDef thingDef = bd as ThingDef;
			if (thingDef != null)
			{
				if (thingDef.IsMeleeWeapon)
				{
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
					if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Plasteel;
					}
				}
				if (thingDef.IsApparel)
				{
					if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Cloth;
					}
					if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Leather_Plain;
					}
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
				}
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(bd))
			{
				return ThingDefOf.WoodLog;
			}
			if (ThingDefOf.Steel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Steel;
			}
			if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Plasteel;
			}
			if (ThingDefOf.BlocksGranite.stuffProps.CanMake(bd))
			{
				return ThingDefOf.BlocksGranite;
			}
			if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Cloth;
			}
			if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Leather_Plain;
			}
			return GenStuff.AllowedStuffsFor(bd, TechLevel.Undefined).First<ThingDef>();
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x002196B0 File Offset: 0x002178B0
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td, TechLevel.Undefined).RandomElement<ThingDef>();
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x002196C8 File Offset: 0x002178C8
		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			ThingDef result;
			if (!GenStuff.TryRandomStuffByCommonalityFor(td, out result, maxTechLevel))
			{
				result = GenStuff.DefaultStuffFor(td);
			}
			return result;
		}

		// Token: 0x060060F2 RID: 24818 RVA: 0x002196F2 File Offset: 0x002178F2
		public static IEnumerable<ThingDef> AllowedStuffsFor(BuildableDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				yield break;
			}
			List<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allDefs.Count; i = num + 1)
			{
				ThingDef thingDef = allDefs[i];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || thingDef.techLevel <= maxTechLevel) && thingDef.stuffProps.CanMake(td))
				{
					yield return thingDef;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x00219709 File Offset: 0x00217909
		public static IEnumerable<ThingDef> AllowedStuffs(List<StuffCategoryDef> categories, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			List<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allDefs.Count; i = num + 1)
			{
				ThingDef thingDef = allDefs[i];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || thingDef.techLevel <= maxTechLevel))
				{
					bool flag = false;
					for (int j = 0; j < thingDef.stuffProps.categories.Count; j++)
					{
						for (int k = 0; k < categories.Count; k++)
						{
							if (thingDef.stuffProps.categories[j] == categories[k])
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						yield return thingDef;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x00219720 File Offset: 0x00217920
		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			return GenStuff.AllowedStuffsFor(td, maxTechLevel).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
		}

		// Token: 0x060060F5 RID: 24821 RVA: 0x0021975C File Offset: 0x0021795C
		public static bool TryRandomStuffFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(td, maxTechLevel);
			if (validator != null)
			{
				source = from x in source
				where validator(x)
				select x;
			}
			return source.TryRandomElement(out stuff);
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x002197AD File Offset: 0x002179AD
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, validator);
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x002197C8 File Offset: 0x002179C8
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!thingDef.MadeFromStuff)
			{
				return null;
			}
			IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(thingDef, maxTechLevel);
			if (validator != null)
			{
				enumerable = from x in enumerable
				where validator(x)
				select x;
			}
			float cheapestPrice = -1f;
			foreach (ThingDef thingDef2 in enumerable)
			{
				float num = thingDef2.BaseMarketValue / thingDef2.VolumePerUnit;
				if (cheapestPrice == -1f || num < cheapestPrice)
				{
					cheapestPrice = num;
				}
			}
			enumerable = from x in enumerable
			where x.BaseMarketValue / x.VolumePerUnit <= cheapestPrice * 4f
			select x;
			ThingDef result;
			if (enumerable.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out result))
			{
				return result;
			}
			return null;
		}
	}
}
