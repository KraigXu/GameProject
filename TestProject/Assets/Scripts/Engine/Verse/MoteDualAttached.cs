using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000305 RID: 773
	public class MoteDualAttached : Mote
	{
		// Token: 0x060015BF RID: 5567 RVA: 0x0007E96F File Offset: 0x0007CB6F
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0007E989 File Offset: 0x0007CB89
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0007E998 File Offset: 0x0007CB98
		protected void UpdatePositionAndRotation()
		{
			if (this.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					this.exactPosition = (this.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (this.def.mote.rotateTowardsTarget)
					{
						this.exactRotation = this.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90f;
					}
					if (this.def.mote.scaleToConnectTargets)
					{
						this.exactScale = new Vector3(this.def.graphicData.drawSize.y, 1f, (this.link2.LastDrawPos - this.link1.LastDrawPos).MagnitudeHorizontal());
					}
				}
				else
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					this.exactPosition = this.link1.LastDrawPos + this.def.mote.attachedDrawOffset;
				}
			}
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x04000E38 RID: 3640
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
