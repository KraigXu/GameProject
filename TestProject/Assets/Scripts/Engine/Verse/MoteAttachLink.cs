using System;
using UnityEngine;

namespace Verse
{
	
	public struct MoteAttachLink
	{
		
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0007E7BB File Offset: 0x0007C9BB
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x0007E7C8 File Offset: 0x0007C9C8
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0007E7D0 File Offset: 0x0007C9D0
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0007E7D8 File Offset: 0x0007C9D8
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		
		public MoteAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos;
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		
		private TargetInfo targetInt;

		
		private Vector3 lastDrawPosInt;
	}
}
