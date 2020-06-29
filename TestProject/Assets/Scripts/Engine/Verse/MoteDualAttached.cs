using System;
using UnityEngine;

namespace Verse
{
	
	public class MoteDualAttached : Mote
	{
		
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		
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

		
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
