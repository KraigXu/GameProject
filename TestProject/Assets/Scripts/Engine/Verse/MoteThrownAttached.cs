using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030A RID: 778
	internal class MoteThrownAttached : MoteThrown
	{
		// Token: 0x060015E4 RID: 5604 RVA: 0x0007F314 File Offset: 0x0007D514
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.link1.Linked)
			{
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0007F368 File Offset: 0x0007D568
		protected override Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = base.NextExactPosition(deltaTime);
			if (this.link1.Linked)
			{
				if (!this.link1.Target.ThingDestroyed)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.link1.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			return vector;
		}

		// Token: 0x04000E4C RID: 3660
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
	}
}
