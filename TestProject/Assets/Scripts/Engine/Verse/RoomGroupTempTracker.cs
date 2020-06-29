using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	
	public sealed class RoomGroupTempTracker
	{
		
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00049C68 File Offset: 0x00047E68
		private Map Map
		{
			get
			{
				return this.roomGroup.Map;
			}
		}

		
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00049C75 File Offset: 0x00047E75
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		
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

		
		public RoomGroupTempTracker(RoomGroup roomGroup, Map map)
		{
			this.roomGroup = roomGroup;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		
		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		
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

		
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.ThinRoofCoverage * 5E-05f * 120f;
		}

		
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		
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

		
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		
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

		
		private RoomGroup roomGroup;

		
		private float temperatureInt;

		
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		
		private float noRoofCoverage;

		
		private float thickRoofCoverage;

		
		private int cycleIndex;

		
		private const float ThinRoofEqualizeRate = 5E-05f;

		
		private const float NoRoofEqualizeRate = 0.0007f;

		
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		
		private static int debugGetFrame = -999;

		
		private static float debugWallEq;
	}
}
