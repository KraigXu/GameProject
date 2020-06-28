using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001037 RID: 4151
	public class Verb_Spawn : Verb
	{
		// Token: 0x0600633E RID: 25406 RVA: 0x00227E14 File Offset: 0x00226014
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			GenSpawn.Spawn(this.verbProps.spawnDef, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			if (this.verbProps.colonyWideTaleDef != null)
			{
				Pawn pawn = this.caster.Map.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
				TaleRecorder.RecordTale(this.verbProps.colonyWideTaleDef, new object[]
				{
					this.caster,
					pawn
				});
			}
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
			return true;
		}
	}
}
