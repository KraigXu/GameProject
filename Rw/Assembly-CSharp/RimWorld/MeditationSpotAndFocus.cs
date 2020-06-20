using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000706 RID: 1798
	public struct MeditationSpotAndFocus
	{
		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x0010B763 File Offset: 0x00109963
		public bool IsValid
		{
			get
			{
				return this.spot.IsValid;
			}
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x0010B770 File Offset: 0x00109970
		public MeditationSpotAndFocus(LocalTargetInfo spot)
		{
			this.spot = spot;
			this.focus = LocalTargetInfo.Invalid;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x0010B784 File Offset: 0x00109984
		public MeditationSpotAndFocus(LocalTargetInfo spot, LocalTargetInfo focus)
		{
			this.spot = spot;
			this.focus = focus;
		}

		// Token: 0x04001AC4 RID: 6852
		public LocalTargetInfo spot;

		// Token: 0x04001AC5 RID: 6853
		public LocalTargetInfo focus;
	}
}
