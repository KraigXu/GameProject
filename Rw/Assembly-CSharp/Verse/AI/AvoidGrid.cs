using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005C2 RID: 1474
	public class AvoidGrid
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002904 RID: 10500 RVA: 0x000F212F File Offset: 0x000F032F
		public ByteGrid Grid
		{
			get
			{
				if (this.gridDirty)
				{
					this.Regenerate();
				}
				return this.grid;
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000F2145 File Offset: 0x000F0345
		public AvoidGrid(Map map)
		{
			this.map = map;
			this.grid = new ByteGrid(map);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000F2168 File Offset: 0x000F0368
		public void Regenerate()
		{
			this.gridDirty = false;
			this.grid.Clear(0);
			List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.ai_combatDangerous)
				{
					Building_TurretGun building_TurretGun = allBuildingsColonist[i] as Building_TurretGun;
					if (building_TurretGun != null)
					{
						this.PrintAvoidGridAroundTurret(building_TurretGun);
					}
				}
			}
			this.ExpandAvoidGridIntoEdifices();
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000F21DF File Offset: 0x000F03DF
		public void Notify_BuildingSpawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000F21DF File Offset: 0x000F03DF
		public void Notify_BuildingDespawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000F2202 File Offset: 0x000F0402
		public void DebugDrawOnMap()
		{
			if (DebugViewSettings.drawAvoidGrid && Find.CurrentMap == this.map)
			{
				this.Grid.DebugDraw();
			}
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000F2224 File Offset: 0x000F0424
		private void PrintAvoidGridAroundTurret(Building_TurretGun tur)
		{
			float range = tur.GunCompEq.PrimaryVerb.verbProps.range;
			float num = tur.GunCompEq.PrimaryVerb.verbProps.EffectiveMinRange(true);
			int num2 = GenRadial.NumCellsInRadius(range + 4f);
			for (int i = (num < 1f) ? 0 : GenRadial.NumCellsInRadius(num); i < num2; i++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(tur.Map) && intVec.Walkable(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					this.IncrementAvoidGrid(intVec, 45);
				}
			}
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000F22DC File Offset: 0x000F04DC
		private void IncrementAvoidGrid(IntVec3 c, int num)
		{
			byte b = this.grid[c];
			b = (byte)Mathf.Min(255, (int)b + num);
			this.grid[c] = b;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000F2314 File Offset: 0x000F0514
		private void ExpandAvoidGridIntoEdifices()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				if (this.grid[i] != 0 && this.map.edificeGrid[i] == null)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c = this.map.cellIndices.IndexToCell(i) + GenAdj.AdjacentCells[j];
						if (c.InBounds(this.map) && c.GetEdifice(this.map) != null)
						{
							this.grid[c] = (byte)Mathf.Min(255, Mathf.Max((int)this.grid[c], (int)this.grid[i]));
						}
					}
				}
			}
		}

		// Token: 0x040018B6 RID: 6326
		public Map map;

		// Token: 0x040018B7 RID: 6327
		private ByteGrid grid;

		// Token: 0x040018B8 RID: 6328
		private bool gridDirty = true;
	}
}
