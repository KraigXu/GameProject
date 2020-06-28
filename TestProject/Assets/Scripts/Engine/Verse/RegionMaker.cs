﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C3 RID: 451
	public class RegionMaker
	{
		// Token: 0x06000C99 RID: 3225 RVA: 0x0004800C File Offset: 0x0004620C
		public RegionMaker(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00048060 File Offset: 0x00046260
		public Region TryGenerateRegionFrom(IntVec3 root)
		{
			RegionType expectedRegionType = root.GetExpectedRegionType(this.map);
			if (expectedRegionType == RegionType.None)
			{
				return null;
			}
			if (this.working)
			{
				Log.Error("Trying to generate a new region but we are currently generating one. Nested calls are not allowed.", false);
				return null;
			}
			this.working = true;
			Region result;
			try
			{
				this.regionGrid = this.map.regionGrid;
				this.newReg = Region.MakeNewUnfilled(root, this.map);
				this.newReg.type = expectedRegionType;
				if (this.newReg.type == RegionType.Portal)
				{
					this.newReg.door = root.GetDoor(this.map);
				}
				this.FloodFillAndAddCells(root);
				this.CreateLinks();
				this.RegisterThingsInRegionListers();
				result = this.newReg;
			}
			finally
			{
				this.working = false;
			}
			return result;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00048124 File Offset: 0x00046324
		private void FloodFillAndAddCells(IntVec3 root)
		{
			this.newRegCells.Clear();
			if (this.newReg.type.IsOneCellRegion())
			{
				this.AddCell(root);
				return;
			}
			this.map.floodFiller.FloodFill(root, (IntVec3 x) => this.newReg.extentsLimit.Contains(x) && x.GetExpectedRegionType(this.map) == this.newReg.type, delegate(IntVec3 x)
			{
				this.AddCell(x);
			}, int.MaxValue, false, null);
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00048188 File Offset: 0x00046388
		private void AddCell(IntVec3 c)
		{
			this.regionGrid.SetRegionAt(c, this.newReg);
			this.newRegCells.Add(c);
			if (this.newReg.extentsClose.minX > c.x)
			{
				this.newReg.extentsClose.minX = c.x;
			}
			if (this.newReg.extentsClose.maxX < c.x)
			{
				this.newReg.extentsClose.maxX = c.x;
			}
			if (this.newReg.extentsClose.minZ > c.z)
			{
				this.newReg.extentsClose.minZ = c.z;
			}
			if (this.newReg.extentsClose.maxZ < c.z)
			{
				this.newReg.extentsClose.maxZ = c.z;
			}
			if (c.x == 0 || c.x == this.map.Size.x - 1 || c.z == 0 || c.z == this.map.Size.z - 1)
			{
				this.newReg.touchesMapEdge = true;
			}
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x000482BC File Offset: 0x000464BC
		private void CreateLinks()
		{
			for (int i = 0; i < this.linksProcessedAt.Length; i++)
			{
				this.linksProcessedAt[i].Clear();
			}
			for (int j = 0; j < this.newRegCells.Count; j++)
			{
				IntVec3 c = this.newRegCells[j];
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.North, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.South, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.East, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.West, c);
			}
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0004833C File Offset: 0x0004653C
		private void SweepInTwoDirectionsAndTryToCreateLink(Rot4 potentialOtherRegionDir, IntVec3 c)
		{
			if (!potentialOtherRegionDir.IsValid)
			{
				return;
			}
			HashSet<IntVec3> hashSet = this.linksProcessedAt[potentialOtherRegionDir.AsInt];
			if (hashSet.Contains(c))
			{
				return;
			}
			IntVec3 c2 = c + potentialOtherRegionDir.FacingCell;
			if (c2.InBounds(this.map) && this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2) == this.newReg)
			{
				return;
			}
			RegionType expectedRegionType = c2.GetExpectedRegionType(this.map);
			if (expectedRegionType == RegionType.None)
			{
				return;
			}
			Rot4 rot = potentialOtherRegionDir;
			rot.Rotate(RotationDirection.Clockwise);
			int num = 0;
			int num2 = 0;
			hashSet.Add(c);
			if (!expectedRegionType.IsOneCellRegion())
			{
				for (;;)
				{
					IntVec3 intVec = c + rot.FacingCell * (num + 1);
					if (!intVec.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec) != this.newReg || (intVec + potentialOtherRegionDir.FacingCell).GetExpectedRegionType(this.map) != expectedRegionType)
					{
						break;
					}
					if (!hashSet.Add(intVec))
					{
						Log.Error("We've processed the same cell twice.", false);
					}
					num++;
				}
				for (;;)
				{
					IntVec3 intVec2 = c - rot.FacingCell * (num2 + 1);
					if (!intVec2.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec2) != this.newReg || (intVec2 + potentialOtherRegionDir.FacingCell).GetExpectedRegionType(this.map) != expectedRegionType)
					{
						break;
					}
					if (!hashSet.Add(intVec2))
					{
						Log.Error("We've processed the same cell twice.", false);
					}
					num2++;
				}
			}
			int length = num + num2 + 1;
			SpanDirection dir;
			IntVec3 root;
			if (potentialOtherRegionDir == Rot4.North)
			{
				dir = SpanDirection.East;
				root = c - rot.FacingCell * num2;
				root.z++;
			}
			else if (potentialOtherRegionDir == Rot4.South)
			{
				dir = SpanDirection.East;
				root = c + rot.FacingCell * num;
			}
			else if (potentialOtherRegionDir == Rot4.East)
			{
				dir = SpanDirection.North;
				root = c + rot.FacingCell * num;
				root.x++;
			}
			else
			{
				dir = SpanDirection.North;
				root = c - rot.FacingCell * num2;
			}
			EdgeSpan span = new EdgeSpan(root, dir, length);
			RegionLink regionLink = this.map.regionLinkDatabase.LinkFrom(span);
			regionLink.Register(this.newReg);
			this.newReg.links.Add(regionLink);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x000485B0 File Offset: 0x000467B0
		private void RegisterThingsInRegionListers()
		{
			CellRect cellRect = this.newReg.extentsClose;
			cellRect = cellRect.ExpandedBy(1);
			cellRect.ClipInsideMap(this.map);
			RegionMaker.tmpProcessedThings.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				bool flag = false;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.map) && this.regionGrid.GetValidRegionAt(c) == this.newReg)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					RegionListersUpdater.RegisterAllAt(intVec, this.map, RegionMaker.tmpProcessedThings);
				}
			}
			RegionMaker.tmpProcessedThings.Clear();
		}

		// Token: 0x040009EB RID: 2539
		private Map map;

		// Token: 0x040009EC RID: 2540
		private Region newReg;

		// Token: 0x040009ED RID: 2541
		private List<IntVec3> newRegCells = new List<IntVec3>();

		// Token: 0x040009EE RID: 2542
		private bool working;

		// Token: 0x040009EF RID: 2543
		private HashSet<IntVec3>[] linksProcessedAt = new HashSet<IntVec3>[]
		{
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>()
		};

		// Token: 0x040009F0 RID: 2544
		private RegionGrid regionGrid;

		// Token: 0x040009F1 RID: 2545
		private static HashSet<Thing> tmpProcessedThings = new HashSet<Thing>();
	}
}
