using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000154 RID: 340
	public sealed class DebugCellDrawer
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x000344E8 File Offset: 0x000326E8
		public void FlashCell(IntVec3 c, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugCell debugCell = new DebugCell();
			debugCell.c = c;
			debugCell.displayString = text;
			debugCell.colorPct = colorPct;
			debugCell.ticksLeft = duration;
			this.debugCells.Add(debugCell);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00034524 File Offset: 0x00032724
		public void FlashCell(IntVec3 c, Material mat, string text = null, int duration = 50)
		{
			DebugCell debugCell = new DebugCell();
			debugCell.c = c;
			debugCell.displayString = text;
			debugCell.customMat = mat;
			debugCell.ticksLeft = duration;
			this.debugCells.Add(debugCell);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00034560 File Offset: 0x00032760
		public void FlashLine(IntVec3 a, IntVec3 b, int duration = 50, SimpleColor color = SimpleColor.White)
		{
			this.debugLines.Add(new DebugLine(a.ToVector3Shifted(), b.ToVector3Shifted(), duration, color));
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00034584 File Offset: 0x00032784
		public void DebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugCells.Count; i++)
			{
				this.debugCells[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x000345E4 File Offset: 0x000327E4
		public void DebugDrawerTick()
		{
			for (int i = this.debugCells.Count - 1; i >= 0; i--)
			{
				DebugCell debugCell = this.debugCells[i];
				debugCell.ticksLeft--;
				if (debugCell.ticksLeft <= 0)
				{
					this.debugCells.RemoveAt(i);
				}
			}
			this.debugLines.RemoveAll((DebugLine dl) => dl.Done);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00034664 File Offset: 0x00032864
		public void DebugDrawerOnGUI()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				for (int i = 0; i < this.debugCells.Count; i++)
				{
					this.debugCells[i].OnGUI();
				}
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x040007E1 RID: 2017
		private List<DebugCell> debugCells = new List<DebugCell>();

		// Token: 0x040007E2 RID: 2018
		private List<DebugLine> debugLines = new List<DebugLine>();

		// Token: 0x040007E3 RID: 2019
		private const int DefaultLifespanTicks = 50;
	}
}
