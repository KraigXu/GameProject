using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000155 RID: 341
	internal sealed class DebugCell
	{
		// Token: 0x0600099E RID: 2462 RVA: 0x000346FC File Offset: 0x000328FC
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
				return;
			}
			CellRenderer.RenderCell(this.c, this.colorPct);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00034730 File Offset: 0x00032930
		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
				if (new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x040007E4 RID: 2020
		public IntVec3 c;

		// Token: 0x040007E5 RID: 2021
		public string displayString;

		// Token: 0x040007E6 RID: 2022
		public float colorPct;

		// Token: 0x040007E7 RID: 2023
		public int ticksLeft;

		// Token: 0x040007E8 RID: 2024
		public Material customMat;
	}
}
