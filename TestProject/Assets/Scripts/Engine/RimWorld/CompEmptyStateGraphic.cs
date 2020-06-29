using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompEmptyStateGraphic : ThingComp
	{
		
		
		private CompProperties_EmptyStateGraphic Props
		{
			get
			{
				return (CompProperties_EmptyStateGraphic)this.props;
			}
		}

		
		
		public bool ParentIsEmpty
		{
			get
			{
				Building_Casket building_Casket = this.parent as Building_Casket;
				if (building_Casket != null && !building_Casket.HasAnyContents)
				{
					return true;
				}
				CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = this.parent.TryGetComp<CompPawnSpawnOnWakeup>();
				return compPawnSpawnOnWakeup != null && !compPawnSpawnOnWakeup.CanSpawn;
			}
		}

		
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.ParentIsEmpty)
			{
				Mesh mesh = this.Props.graphicData.Graphic.MeshAt(this.parent.Rotation);
				Vector3 drawPos = this.parent.DrawPos;
				drawPos.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
				Graphics.DrawMesh(mesh, drawPos + this.Props.graphicData.drawOffset.RotatedBy(this.parent.Rotation), Quaternion.identity, this.Props.graphicData.Graphic.MatAt(this.parent.Rotation, null), 0);
			}
		}
	}
}
