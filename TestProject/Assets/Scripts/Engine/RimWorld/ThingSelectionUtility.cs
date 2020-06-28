using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB7 RID: 3767
	public static class ThingSelectionUtility
	{
		// Token: 0x06005BEC RID: 23532 RVA: 0x001FBE78 File Offset: 0x001FA078
		public static bool SelectableByMapClick(Thing t)
		{
			if (!t.def.selectable)
			{
				return false;
			}
			if (!t.Spawned)
			{
				return false;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				return !t.Position.Fogged(t.Map);
			}
			using (CellRect.Enumerator enumerator = t.OccupiedRect().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Fogged(t.Map))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005BED RID: 23533 RVA: 0x001FBF30 File Offset: 0x001FA130
		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x001FBF47 File Offset: 0x001FA147
		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedThings.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						List<Thing> cellThings = Find.CurrentMap.thingGrid.ThingsListAt(c);
						if (cellThings != null)
						{
							int num;
							for (int i = 0; i < cellThings.Count; i = num + 1)
							{
								Thing t = cellThings[i];
								if (ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
								{
									yield return t;
									ThingSelectionUtility.yieldedThings.Add(t);
								}
								t = null;
								num = i;
							}
						}
						cellThings = null;
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedThings.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x001FBF57 File Offset: 0x001FA157
		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedZones.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						Zone zone = c.GetZone(Find.CurrentMap);
						if (zone != null && zone.IsMultiselectable)
						{
							if (!ThingSelectionUtility.yieldedZones.Contains(zone))
							{
								yield return zone;
								ThingSelectionUtility.yieldedZones.Add(zone);
							}
							zone = null;
						}
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedZones.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x001FBF68 File Offset: 0x001FA168
		private static CellRect GetMapRect(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new CellRect
			{
				minX = Mathf.FloorToInt(vector.x),
				minZ = Mathf.FloorToInt(vector2.z),
				maxX = Mathf.FloorToInt(vector2.x),
				maxZ = Mathf.FloorToInt(vector.z)
			};
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x001FC01C File Offset: 0x001FA21C
		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			int num = -1;
			for (int i = ThingSelectionUtility.tmpColonists.Count - 1; i >= 0; i--)
			{
				if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[0]);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count]);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x001FC11C File Offset: 0x001FA31C
		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			int num = -1;
			for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
			{
				if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1]);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)]);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x04003237 RID: 12855
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		// Token: 0x04003238 RID: 12856
		private static HashSet<Zone> yieldedZones = new HashSet<Zone>();

		// Token: 0x04003239 RID: 12857
		private static List<Pawn> tmpColonists = new List<Pawn>();
	}
}
