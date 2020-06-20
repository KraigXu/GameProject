﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC2 RID: 4034
	public static class GenThing
	{
		// Token: 0x060060F8 RID: 24824 RVA: 0x002198C0 File Offset: 0x00217AC0
		public static Vector3 TrueCenter(this Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				return pawn.Drawer.DrawPos;
			}
			return GenThing.TrueCenter(t.Position, t.Rotation, t.def.size, t.def.Altitude);
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x0021990C File Offset: 0x00217B0C
		public static Vector3 TrueCenter(IntVec3 loc, Rot4 rotation, IntVec2 thingSize, float altitude)
		{
			Vector3 result = loc.ToVector3ShiftedWithAltitude(altitude);
			if (thingSize.x != 1 || thingSize.z != 1)
			{
				if (rotation.IsHorizontal)
				{
					int x = thingSize.x;
					thingSize.x = thingSize.z;
					thingSize.z = x;
				}
				switch (rotation.AsInt)
				{
				case 0:
					if (thingSize.x % 2 == 0)
					{
						result.x += 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z += 0.5f;
					}
					break;
				case 1:
					if (thingSize.x % 2 == 0)
					{
						result.x += 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z -= 0.5f;
					}
					break;
				case 2:
					if (thingSize.x % 2 == 0)
					{
						result.x -= 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z -= 0.5f;
					}
					break;
				case 3:
					if (thingSize.x % 2 == 0)
					{
						result.x -= 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z += 0.5f;
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x00219A5C File Offset: 0x00217C5C
		public static bool TryDropAndSetForbidden(Thing th, IntVec3 pos, Map map, ThingPlaceMode mode, out Thing resultingThing, bool forbidden)
		{
			if (GenDrop.TryDropSpawn_NewTmp(th, pos, map, ThingPlaceMode.Near, out resultingThing, null, null, true))
			{
				if (resultingThing != null)
				{
					resultingThing.SetForbidden(forbidden, false);
				}
				return true;
			}
			resultingThing = null;
			return false;
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x00219A88 File Offset: 0x00217C88
		public static string ThingsToCommaList(IList<Thing> things, bool useAnd = false, bool aggregate = true, int maxCount = -1)
		{
			GenThing.tmpThings.Clear();
			GenThing.tmpThingLabels.Clear();
			GenThing.tmpThingCounts.Clear();
			GenThing.tmpThings.AddRange(things);
			if (GenThing.tmpThings.Count >= 2)
			{
				GenThing.tmpThings.SortByDescending((Thing x) => x is Pawn, (Thing x) => x.def.BaseMarketValue * (float)x.stackCount);
			}
			for (int i = 0; i < GenThing.tmpThings.Count; i++)
			{
				string text = (GenThing.tmpThings[i] is Pawn) ? GenThing.tmpThings[i].LabelShort : GenThing.tmpThings[i].LabelNoCount;
				bool flag = false;
				if (aggregate)
				{
					for (int j = 0; j < GenThing.tmpThingCounts.Count; j++)
					{
						if (GenThing.tmpThingCounts[j].First == text)
						{
							GenThing.tmpThingCounts[j] = new Pair<string, int>(GenThing.tmpThingCounts[j].First, GenThing.tmpThingCounts[j].Second + GenThing.tmpThings[i].stackCount);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					GenThing.tmpThingCounts.Add(new Pair<string, int>(text, GenThing.tmpThings[i].stackCount));
				}
			}
			GenThing.tmpThings.Clear();
			bool flag2 = false;
			int num = GenThing.tmpThingCounts.Count;
			if (maxCount >= 0 && num > maxCount)
			{
				num = maxCount;
				flag2 = true;
			}
			for (int k = 0; k < num; k++)
			{
				string text2 = GenThing.tmpThingCounts[k].First;
				if (GenThing.tmpThingCounts[k].Second != 1)
				{
					text2 = text2 + " x" + GenThing.tmpThingCounts[k].Second;
				}
				GenThing.tmpThingLabels.Add(text2);
			}
			string text3 = GenThing.tmpThingLabels.ToCommaList(useAnd && !flag2);
			if (flag2)
			{
				text3 += "...";
			}
			return text3;
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x00219CD8 File Offset: 0x00217ED8
		public static float GetMarketValue(IList<Thing> things)
		{
			float num = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += things[i].MarketValue * (float)things[i].stackCount;
			}
			return num;
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x00219D1C File Offset: 0x00217F1C
		public static bool CloserThingBetween(ThingDef thingDef, IntVec3 a, IntVec3 b, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 intVec in CellRect.FromLimits(a, b))
			{
				if (!(intVec == a) && !(intVec == b) && intVec.InBounds(map))
				{
					foreach (Thing thing in intVec.GetThingList(map))
					{
						if ((thingToIgnore == null || thingToIgnore != thing) && (thing.def == thingDef || thing.def.entityDefToBuild == thingDef))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04003B10 RID: 15120
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04003B11 RID: 15121
		private static List<string> tmpThingLabels = new List<string>();

		// Token: 0x04003B12 RID: 15122
		private static List<Pair<string, int>> tmpThingCounts = new List<Pair<string, int>>();
	}
}
