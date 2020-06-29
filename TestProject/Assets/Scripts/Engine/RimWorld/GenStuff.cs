using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class GenStuff
	{
		
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

		
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td, TechLevel.Undefined).RandomElement<ThingDef>();
		}

		
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

		
		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			return GenStuff.AllowedStuffsFor(td, maxTechLevel).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
		}

		
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

		
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, validator);
		}

		
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
