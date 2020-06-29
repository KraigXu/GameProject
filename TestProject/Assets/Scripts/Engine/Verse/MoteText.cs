using System;
using UnityEngine;

namespace Verse
{
	
	public class MoteText : MoteThrown
	{
		
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

		
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0007EEF1 File Offset: 0x0007D0F1
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		
		public override void Draw()
		{
		}

		
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		
		public string text;

		
		public Color textColor = Color.white;

		
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
