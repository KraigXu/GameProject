using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000308 RID: 776
	public class MoteText : MoteThrown
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0007EED5 File Offset: 0x0007D0D5
		protected float TimeBeforeStartFadeout
		{
			get
			{
				if (this.overrideTimeBeforeStartFadeout < 0f)
				{
					return base.SolidTime;
				}
				return this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0007EEF1 File Offset: 0x0007D0F1
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00002681 File Offset: 0x00000881
		public override void Draw()
		{
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0007EF18 File Offset: 0x0007D118
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x04000E47 RID: 3655
		public string text;

		// Token: 0x04000E48 RID: 3656
		public Color textColor = Color.white;

		// Token: 0x04000E49 RID: 3657
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
