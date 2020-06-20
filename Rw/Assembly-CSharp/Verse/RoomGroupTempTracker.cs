using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CB RID: 459
	public sealed class RoomGroupTempTracker
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00049C68 File Offset: 0x00047E68
		private Map Map
		{
			get
			{
				return this.roomGroup.Map;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00049C75 File Offset: 0x00047E75
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00049C8A File Offset: 0x00047E8A
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x00049C92 File Offset: 0x00047E92
		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -273.15f, 1000f);
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00049CAA File Offset: 0x00047EAA
		public RoomGroupTempTracker(RoomGroup roomGroup, Map map)
		{
			this.roomGroup = roomGroup;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00049CD5 File Offset: 0x00047ED5
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00049CDD File Offset: 0x00047EDD
		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00049D00 File Offset: 0x00047F00
		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.roomGroup.RoomCount == 0)
			{
				return;
			}
			Map map = this.Map;
			if (!this.roomGroup.UsesOutdoorTemperature)
			{
				int num = 0;
				foreach (IntVec3 c in this.roomGroup.Cells)
				{
					RoofDef roof = c.GetRoof(map);
					if (roof == null)
					{
						this.noRoofCoverage += 1f;
					}
					else if (roof.isThickRoof)
					{
						this.thickRoofCoverage += 1f;
					}
					num++;
				}
				this.thickRoofCoverage /= (float)num;
				this.noRoofCoverage /= (float)num;
				foreach (IntVec3 a in this.roomGroup.Cells)
				{
					int i = 0;
					while (i < 4)
					{
						IntVec3 intVec = a + GenAdj.CardinalDirections[i];
						IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
						if (!intVec.InBounds(map))
						{
							goto IL_1E4;
						}
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region == null)
						{
							goto IL_1E4;
						}
						if (region.type == RegionType.Portal)
						{
							bool flag = false;
							for (int j = 0; j < region.links.Count; j++)
							{
								Region regionA = region.links[j].RegionA;
								Region regionB = region.links[j].RegionB;
								if (regionA.Room.Group != this.roomGroup && !regionA.IsDoorway)
								{
									flag = true;
									break;
								}
								if (regionB.Room.Group != this.roomGroup && !regionB.IsDoorway)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_1E4;
							}
						}
						IL_248:
						i++;
						continue;
						IL_1E4:
						if (!intVec2.InBounds(map) || intVec2.GetRoomGroup(map) == this.roomGroup)
						{
							goto IL_248;
						}
						bool flag2 = false;
						for (int k = 0; k < 4; k++)
						{
							if ((intVec2 + GenAdj.CardinalDirections[k]).GetRoomGroup(map) == this.roomGroup)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.equalizeCells.Add(intVec2);
							goto IL_248;
						}
						goto IL_248;
					}
				}
				this.equalizeCells.Shuffle<IntVec3>();
			}
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00049FBC File Offset: 0x000481BC
		public void EqualizeTemperature()
		{
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
				return;
			}
			if (this.roomGroup.RoomCount != 0 && this.roomGroup.Rooms[0].RegionType == RegionType.Portal)
			{
				return;
			}
			float num = this.ThinRoofEqualizationTempChangePerInterval();
			float num2 = this.NoRoofEqualizationTempChangePerInterval();
			float num3 = this.WallEqualizationTempChangePerInterval();
			float num4 = this.DeepEqualizationTempChangePerInterval();
			this.Temperature += num + num2 + num3 + num4;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0004A044 File Offset: 0x00048244
		private float WallEqualizationTempChangePerInterval()
		{
			if (this.equalizeCells.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = Mathf.CeilToInt((float)this.equalizeCells.Count * 0.2f);
			for (int i = 0; i < num2; i++)
			{
				this.cycleIndex++;
				int index = this.cycleIndex % this.equalizeCells.Count;
				float num3;
				if (GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num3))
				{
					num += num3 - this.Temperature;
				}
				else
				{
					num += Mathf.Lerp(this.Temperature, this.Map.mapTemperature.OutdoorTemp, 0.5f) - this.Temperature;
				}
			}
			return num / (float)num2 * (float)this.equalizeCells.Count * 120f * 0.00017f / (float)this.roomGroup.CellCount;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0004A12C File Offset: 0x0004832C
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0004A181 File Offset: 0x00048381
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.ThinRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004A1AF File Offset: 0x000483AF
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0004A1E0 File Offset: 0x000483E0
		private float DeepEqualizationTempChangePerInterval()
		{
			if (this.thickRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = 15f - this.temperatureInt;
			if (num > 0f)
			{
				return 0f;
			}
			return num * this.thickRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0004A230 File Offset: 0x00048430
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0004A288 File Offset: 0x00048488
		internal string DebugString()
		{
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				return "uses outdoor temperature";
			}
			if (Time.frameCount > RoomGroupTempTracker.debugGetFrame + 120)
			{
				RoomGroupTempTracker.debugWallEq = 0f;
				for (int i = 0; i < 40; i++)
				{
					RoomGroupTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
				}
				RoomGroupTempTracker.debugWallEq /= 40f;
				RoomGroupTempTracker.debugGetFrame = Time.frameCount;
			}
			return string.Concat(new object[]
			{
				"  thick roof coverage: ",
				this.thickRoofCoverage.ToStringPercent("F0"),
				"\n  thin roof coverage: ",
				this.ThinRoofCoverage.ToStringPercent("F0"),
				"\n  no roof coverage: ",
				this.noRoofCoverage.ToStringPercent("F0"),
				"\n\n  wall equalization: ",
				RoomGroupTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
				"\n  thin roof equalization: ",
				this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  no roof equalization: ",
				this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  deep equalization: ",
				this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n\n  temp diff from outdoors, adjusted: ",
				this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
				"\n  tempChange e=20 targ= 200C: ",
				GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First<IntVec3>(), this.roomGroup.Map, 20f, 200f),
				"\n  tempChange e=20 targ=-200C: ",
				GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First<IntVec3>(), this.roomGroup.Map, 20f, -200f),
				"\n  equalize interval ticks: ",
				120,
				"\n  equalize cells count:",
				this.equalizeCells.Count
			});
		}

		// Token: 0x04000A1D RID: 2589
		private RoomGroup roomGroup;

		// Token: 0x04000A1E RID: 2590
		private float temperatureInt;

		// Token: 0x04000A1F RID: 2591
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x04000A20 RID: 2592
		private float noRoofCoverage;

		// Token: 0x04000A21 RID: 2593
		private float thickRoofCoverage;

		// Token: 0x04000A22 RID: 2594
		private int cycleIndex;

		// Token: 0x04000A23 RID: 2595
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x04000A24 RID: 2596
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x04000A25 RID: 2597
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04000A26 RID: 2598
		private static int debugGetFrame = -999;

		// Token: 0x04000A27 RID: 2599
		private static float debugWallEq;
	}
}
