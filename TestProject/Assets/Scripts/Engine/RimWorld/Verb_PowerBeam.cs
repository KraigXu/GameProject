using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001035 RID: 4149
	public class Verb_PowerBeam : Verb
	{
		// Token: 0x06006337 RID: 25399 RVA: 0x00227CC4 File Offset: 0x00225EC4
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			powerBeam.duration = 600;
			powerBeam.instigator = this.caster;
			powerBeam.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			powerBeam.StartStrike();
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
			return true;
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x00227D7D File Offset: 0x00225F7D
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x04003C52 RID: 15442
		private const int DurationTicks = 600;
	}
}
