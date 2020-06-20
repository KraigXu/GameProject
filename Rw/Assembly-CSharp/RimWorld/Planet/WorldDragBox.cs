using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001299 RID: 4761
	public class WorldDragBox
	{
		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06007025 RID: 28709 RVA: 0x002722A9 File Offset: 0x002704A9
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06007026 RID: 28710 RVA: 0x002722C5 File Offset: 0x002704C5
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06007027 RID: 28711 RVA: 0x002722E1 File Offset: 0x002704E1
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06007028 RID: 28712 RVA: 0x002722FD File Offset: 0x002704FD
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06007029 RID: 28713 RVA: 0x00272319 File Offset: 0x00270519
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x0600702A RID: 28714 RVA: 0x00272348 File Offset: 0x00270548
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x0600702B RID: 28715 RVA: 0x00272381 File Offset: 0x00270581
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x0600702C RID: 28716 RVA: 0x00272390 File Offset: 0x00270590
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x002723A2 File Offset: 0x002705A2
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x002723B8 File Offset: 0x002705B8
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x0600702F RID: 28719 RVA: 0x002723C8 File Offset: 0x002705C8
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		// Token: 0x04004504 RID: 17668
		public bool active;

		// Token: 0x04004505 RID: 17669
		public Vector2 start;

		// Token: 0x04004506 RID: 17670
		private const float DragBoxMinDiagonal = 7f;
	}
}
