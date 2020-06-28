using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000304 RID: 772
	public class MoteAttached : Mote
	{
		// Token: 0x060015BC RID: 5564 RVA: 0x0007E852 File Offset: 0x0007CA52
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0007E880 File Offset: 0x0007CA80
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (this.link1.Linked)
			{
				if (!this.link1.Target.ThingDestroyed)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.def.mote.attachedDrawOffset;
				if (this.def.mote.attachedToHead)
				{
					Pawn pawn = this.link1.Target.Thing as Pawn;
					if (pawn != null && pawn.story != null)
					{
						b = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle());
					}
				}
				this.exactPosition = this.link1.LastDrawPos + b;
			}
		}
	}
}
