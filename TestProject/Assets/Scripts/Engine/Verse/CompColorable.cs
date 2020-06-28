using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031C RID: 796
	public class CompColorable : ThingComp
	{
		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x00085508 File Offset: 0x00083708
		// (set) Token: 0x06001742 RID: 5954 RVA: 0x0008552E File Offset: 0x0008372E
		public Color Color
		{
			get
			{
				if (!this.active)
				{
					return this.parent.def.graphicData.color;
				}
				return this.color;
			}
			set
			{
				if (value == this.color)
				{
					return;
				}
				this.active = true;
				this.color = value;
				this.parent.Notify_ColorChanged();
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00085558 File Offset: 0x00083758
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00085560 File Offset: 0x00083760
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000855C8 File Offset: 0x000837C8
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
			{
				return;
			}
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00085618 File Offset: 0x00083818
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x04000EA1 RID: 3745
		private Color color = Color.white;

		// Token: 0x04000EA2 RID: 3746
		private bool active;
	}
}
