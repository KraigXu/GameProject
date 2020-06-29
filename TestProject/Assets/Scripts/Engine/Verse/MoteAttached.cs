using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class MoteAttached : Mote
	{
		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		
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
