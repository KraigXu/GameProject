using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A86 RID: 2694
	public class PowerNetGrid
	{
		// Token: 0x06003FAF RID: 16303 RVA: 0x00152CB4 File Offset: 0x00150EB4
		public PowerNetGrid(Map map)
		{
			this.map = map;
			this.netGrid = new PowerNet[map.cellIndices.NumGridCells];
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00152CE4 File Offset: 0x00150EE4
		public PowerNet TransmittedPowerNetAt(IntVec3 c)
		{
			return this.netGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00152D00 File Offset: 0x00150F00
		public void Notify_PowerNetCreated(PowerNet newNet)
		{
			if (this.powerNetCells.ContainsKey(newNet))
			{
				Log.Warning("Net " + newNet + " is already registered in PowerNetGrid.", false);
				this.powerNetCells.Remove(newNet);
			}
			List<IntVec3> list = new List<IntVec3>();
			this.powerNetCells.Add(newNet, list);
			for (int i = 0; i < newNet.transmitters.Count; i++)
			{
				CellRect cellRect = newNet.transmitters[i].parent.OccupiedRect();
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						int num = this.map.cellIndices.CellToIndex(k, j);
						if (this.netGrid[num] != null)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Two power nets on the same cell (",
								k,
								", ",
								j,
								"). First transmitters: ",
								newNet.transmitters[0].parent.LabelCap,
								" and ",
								this.netGrid[num].transmitters.NullOrEmpty<CompPower>() ? "[none]" : this.netGrid[num].transmitters[0].parent.LabelCap,
								"."
							}), false);
						}
						this.netGrid[num] = newNet;
						list.Add(new IntVec3(k, 0, j));
					}
				}
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00152EA0 File Offset: 0x001510A0
		public void Notify_PowerNetDeleted(PowerNet deadNet)
		{
			List<IntVec3> list;
			if (!this.powerNetCells.TryGetValue(deadNet, out list))
			{
				Log.Warning("Net " + deadNet + " does not exist in PowerNetGrid's dictionary.", false);
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int num = this.map.cellIndices.CellToIndex(list[i]);
				if (this.netGrid[num] == deadNet)
				{
					this.netGrid[num] = null;
				}
				else
				{
					Log.Warning("Multiple nets on the same cell " + list[i] + ". This is probably a result of an earlier error.", false);
				}
			}
			this.powerNetCells.Remove(deadNet);
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00152F44 File Offset: 0x00151144
		public void DrawDebugPowerNetGrid()
		{
			if (!DebugViewSettings.drawPowerNetGrid)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			Rand.PushState();
			foreach (IntVec3 c in Find.CameraDriver.CurrentViewRect.ClipInsideMap(this.map))
			{
				PowerNet powerNet = this.netGrid[this.map.cellIndices.CellToIndex(c)];
				if (powerNet != null)
				{
					Rand.Seed = powerNet.GetHashCode();
					CellRenderer.RenderCell(c, Rand.Value);
				}
			}
			Rand.PopState();
		}

		// Token: 0x04002519 RID: 9497
		private Map map;

		// Token: 0x0400251A RID: 9498
		private PowerNet[] netGrid;

		// Token: 0x0400251B RID: 9499
		private Dictionary<PowerNet, List<IntVec3>> powerNetCells = new Dictionary<PowerNet, List<IntVec3>>();
	}
}
