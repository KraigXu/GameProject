using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldDragBox
	{
		
		// (get) Token: 0x06007025 RID: 28709 RVA: 0x002722A9 File Offset: 0x002704A9
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		
		// (get) Token: 0x06007026 RID: 28710 RVA: 0x002722C5 File Offset: 0x002704C5
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		
		// (get) Token: 0x06007027 RID: 28711 RVA: 0x002722E1 File Offset: 0x002704E1
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		
		// (get) Token: 0x06007028 RID: 28712 RVA: 0x002722FD File Offset: 0x002704FD
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		
		// (get) Token: 0x06007029 RID: 28713 RVA: 0x00272319 File Offset: 0x00270519
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		
		// (get) Token: 0x0600702A RID: 28714 RVA: 0x00272348 File Offset: 0x00270548
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		
		// (get) Token: 0x0600702B RID: 28715 RVA: 0x00272381 File Offset: 0x00270581
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		
		// (get) Token: 0x0600702C RID: 28716 RVA: 0x00272390 File Offset: 0x00270590
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		
		public bool active;

		
		public Vector2 start;

		
		private const float DragBoxMinDiagonal = 7f;
	}
}
