using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000303 RID: 771
	public struct MoteAttachLink
	{
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0007E7BB File Offset: 0x0007C9BB
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x0007E7C8 File Offset: 0x0007C9C8
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0007E7D0 File Offset: 0x0007C9D0
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0007E7D8 File Offset: 0x0007C9D8
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0007E7E4 File Offset: 0x0007C9E4
		public MoteAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0007E808 File Offset: 0x0007CA08
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		// Token: 0x04000E36 RID: 3638
		private TargetInfo targetInt;

		// Token: 0x04000E37 RID: 3639
		private Vector3 lastDrawPosInt;
	}
}
