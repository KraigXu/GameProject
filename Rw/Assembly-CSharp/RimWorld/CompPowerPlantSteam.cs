using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A7B RID: 2683
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x06003F57 RID: 16215 RVA: 0x00150A4F File Offset: 0x0014EC4F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00150A6C File Offset: 0x0014EC6C
		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)this.parent.Map.thingGrid.ThingAt(this.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)this.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00150AE0 File Offset: 0x0014ECE0
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}

		// Token: 0x040024D1 RID: 9425
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x040024D2 RID: 9426
		private Building_SteamGeyser geyser;
	}
}
