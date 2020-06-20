using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9C RID: 3740
	public class DragBox
	{
		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06005B30 RID: 23344 RVA: 0x001F6326 File Offset: 0x001F4526
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06005B31 RID: 23345 RVA: 0x001F6342 File Offset: 0x001F4542
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06005B32 RID: 23346 RVA: 0x001F635E File Offset: 0x001F455E
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06005B33 RID: 23347 RVA: 0x001F637A File Offset: 0x001F457A
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06005B34 RID: 23348 RVA: 0x001F6398 File Offset: 0x001F4598
		public Rect ScreenRect
		{
			get
			{
				Vector2 vector = this.start.MapToUIPosition();
				Vector2 mousePosition = Event.current.mousePosition;
				if (mousePosition.x < vector.x)
				{
					float x = mousePosition.x;
					mousePosition.x = vector.x;
					vector.x = x;
				}
				if (mousePosition.y < vector.y)
				{
					float y = mousePosition.y;
					mousePosition.y = vector.y;
					vector.y = y;
				}
				return new Rect
				{
					xMin = vector.x,
					xMax = mousePosition.x,
					yMin = vector.y,
					yMax = mousePosition.y
				};
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06005B35 RID: 23349 RVA: 0x001F6450 File Offset: 0x001F4650
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x001F647C File Offset: 0x001F467C
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x001F648E File Offset: 0x001F468E
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x001F64A4 File Offset: 0x001F46A4
		public bool Contains(Thing t)
		{
			if (t is Pawn)
			{
				return this.Contains((t as Pawn).Drawer.DrawPos);
			}
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if (this.Contains(intVec.ToVector3Shifted()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x001F6528 File Offset: 0x001F4728
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}

		// Token: 0x040031CB RID: 12747
		public bool active;

		// Token: 0x040031CC RID: 12748
		public Vector3 start;

		// Token: 0x040031CD RID: 12749
		private const float DragBoxMinDiagonal = 0.5f;
	}
}
