using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001031 RID: 4145
	public class Verb_MechCluster : Verb
	{
		// Token: 0x06006323 RID: 25379 RVA: 0x002272F4 File Offset: 0x002254F4
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

		// Token: 0x06006324 RID: 25380 RVA: 0x00227279 File Offset: 0x00225479
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04003C4E RID: 15438
		public const float Points = 2500f;
	}
}
