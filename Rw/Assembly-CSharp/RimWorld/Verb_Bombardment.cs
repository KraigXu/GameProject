using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102F RID: 4143
	public class Verb_Bombardment : Verb
	{
		// Token: 0x0600631E RID: 25374 RVA: 0x002271C0 File Offset: 0x002253C0
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			bombardment.duration = 540;
			bombardment.instigator = this.caster;
			bombardment.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			bombardment.StartStrike();
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
			return true;
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x00227279 File Offset: 0x00225479
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04003C4D RID: 15437
		private const int DurationTicks = 540;
	}
}
