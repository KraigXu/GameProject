using System;
using Verse;

namespace RimWorld
{
	
	public class Verb_MechCluster : Verb
	{
		
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			MechClusterUtility.SpawnCluster(this.currentTarget.Cell, this.caster.Map, MechClusterGenerator.GenerateClusterSketch(2500f, this.caster.Map, true), true, false, null);
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
			return true;
		}

		
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		
		public const float Points = 2500f;
	}
}
