using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3F RID: 3903
	public static class CostListCalculator
	{
		// Token: 0x06005FBF RID: 24511 RVA: 0x002146B5 File Offset: 0x002128B5
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x002146C1 File Offset: 0x002128C1
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x06005FC1 RID: 24513 RVA: 0x002146D8 File Offset: 0x002128D8
		public static List<ThingDefCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			CostListCalculator.CostListPair key = new CostListCalculator.CostListPair(entDef, stuff);
			List<ThingDefCountClass> list;
			if (!CostListCalculator.cachedCosts.TryGetValue(key, out list))
			{
				list = new List<ThingDefCountClass>();
				int num = 0;
				if (entDef.MadeFromStuff)
				{
					if (errorOnNullStuff && stuff == null)
					{
						Log.Error("Cannot get AdjustedCostList for " + entDef + " with null Stuff.", false);
						if (GenStuff.DefaultStuffFor(entDef) == null)
						{
							return null;
						}
						return entDef.CostListAdjusted(GenStuff.DefaultStuffFor(entDef), true);
					}
					else if (stuff != null)
					{
						num = Mathf.RoundToInt((float)entDef.costStuffCount / stuff.VolumePerUnit);
						if (num < 1)
						{
							num = 1;
						}
					}
					else
					{
						num = entDef.costStuffCount;
					}
				}
				else if (stuff != null)
				{
					Log.Error(string.Concat(new object[]
					{
						"Got AdjustedCostList for ",
						entDef,
						" with stuff ",
						stuff,
						" but is not MadeFromStuff."
					}), false);
				}
				bool flag = false;
				if (entDef.costList != null)
				{
					for (int i = 0; i < entDef.costList.Count; i++)
					{
						ThingDefCountClass thingDefCountClass = entDef.costList[i];
						if (thingDefCountClass.thingDef == stuff)
						{
							list.Add(new ThingDefCountClass(thingDefCountClass.thingDef, thingDefCountClass.count + num));
							flag = true;
						}
						else
						{
							list.Add(thingDefCountClass);
						}
					}
				}
				if (!flag && num > 0)
				{
					list.Add(new ThingDefCountClass(stuff, num));
				}
				CostListCalculator.cachedCosts.Add(key, list);
			}
			return list;
		}

		// Token: 0x040033F6 RID: 13302
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x02001E55 RID: 7765
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x0600A8BD RID: 43197 RVA: 0x00318B9C File Offset: 0x00316D9C
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x0600A8BE RID: 43198 RVA: 0x00318BAC File Offset: 0x00316DAC
			public override int GetHashCode()
			{
				return Gen.HashCombine<ThingDef>(Gen.HashCombine<BuildableDef>(0, this.buildable), this.stuff);
			}

			// Token: 0x0600A8BF RID: 43199 RVA: 0x00318BC5 File Offset: 0x00316DC5
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x0600A8C0 RID: 43200 RVA: 0x00318BDD File Offset: 0x00316DDD
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x0600A8C1 RID: 43201 RVA: 0x00318BEB File Offset: 0x00316DEB
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x0600A8C2 RID: 43202 RVA: 0x00318C0B File Offset: 0x00316E0B
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04007221 RID: 29217
			public BuildableDef buildable;

			// Token: 0x04007222 RID: 29218
			public ThingDef stuff;
		}

		// Token: 0x02001E56 RID: 7766
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x0600A8C3 RID: 43203 RVA: 0x00318C17 File Offset: 0x00316E17
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x0600A8C4 RID: 43204 RVA: 0x00318C20 File Offset: 0x00316E20
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04007223 RID: 29219
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();
		}
	}
}
