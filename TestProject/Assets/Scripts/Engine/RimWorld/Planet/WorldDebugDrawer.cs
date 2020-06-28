using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E0 RID: 4576
	public class WorldDebugDrawer
	{
		// Token: 0x060069E8 RID: 27112 RVA: 0x0024F910 File Offset: 0x0024DB10
		public void FlashTile(int tile, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.colorPct = colorPct;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x0024F94C File Offset: 0x0024DB4C
		public void FlashTile(int tile, Material mat, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.customMat = mat;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x0024F988 File Offset: 0x0024DB88
		public void FlashLine(Vector3 a, Vector3 b, bool onPlanetSurface = false, int duration = 50)
		{
			DebugWorldLine debugWorldLine = new DebugWorldLine(a, b, onPlanetSurface);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x0024F9B4 File Offset: 0x0024DBB4
		public void FlashLine(int tileA, int tileB, int duration = 50)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(tileA);
			Vector3 tileCenter2 = worldGrid.GetTileCenter(tileB);
			DebugWorldLine debugWorldLine = new DebugWorldLine(tileCenter, tileCenter2, true);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x0024F9F4 File Offset: 0x0024DBF4
		public void WorldDebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				this.debugTiles[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x0024FA50 File Offset: 0x0024DC50
		public void WorldDebugDrawerTick()
		{
			for (int i = this.debugTiles.Count - 1; i >= 0; i--)
			{
				DebugTile debugTile = this.debugTiles[i];
				debugTile.ticksLeft--;
				if (debugTile.ticksLeft <= 0)
				{
					this.debugTiles.RemoveAt(i);
				}
			}
			for (int j = this.debugLines.Count - 1; j >= 0; j--)
			{
				DebugWorldLine debugWorldLine = this.debugLines[j];
				debugWorldLine.ticksLeft--;
				if (debugWorldLine.ticksLeft <= 0)
				{
					this.debugLines.RemoveAt(j);
				}
			}
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x0024FAEC File Offset: 0x0024DCEC
		public void WorldDebugDrawerOnGUI()
		{
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				if (this.debugTiles[i].DistanceToCamera <= 39f)
				{
					this.debugTiles[i].OnGUI();
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040041FE RID: 16894
		private List<DebugTile> debugTiles = new List<DebugTile>();

		// Token: 0x040041FF RID: 16895
		private List<DebugWorldLine> debugLines = new List<DebugWorldLine>();

		// Token: 0x04004200 RID: 16896
		private const int DefaultLifespanTicks = 50;

		// Token: 0x04004201 RID: 16897
		private const float MaxDistToCameraToDisplayLabel = 39f;
	}
}
