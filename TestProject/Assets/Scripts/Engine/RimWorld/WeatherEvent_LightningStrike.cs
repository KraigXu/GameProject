using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000AB1 RID: 2737
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrike : WeatherEvent_LightningFlash
	{
		// Token: 0x060040D4 RID: 16596 RVA: 0x0015B4A4 File Offset: 0x001596A4
		public WeatherEvent_LightningStrike(Map map) : base(map)
		{
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x0015B4B8 File Offset: 0x001596B8
		public WeatherEvent_LightningStrike(Map map, IntVec3 forcedStrikeLoc) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x0015B4D4 File Offset: 0x001596D4
		public override void FireEvent()
		{
			base.FireEvent();
			if (!this.strikeLoc.IsValid)
			{
				this.strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(this.map) && !this.map.roofGrid.Roofed(sq), this.map, 1000);
			}
			this.boltMesh = LightningBoltMeshPool.RandomBoltMesh;
			if (!this.strikeLoc.Fogged(this.map))
			{
				GenExplosion.DoExplosion(this.strikeLoc, this.map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
				Vector3 loc = this.strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					MoteMaker.ThrowSmoke(loc, this.map, 1.5f);
					MoteMaker.ThrowMicroSparks(loc, this.map);
					MoteMaker.ThrowLightningGlow(loc, this.map, 1.5f);
				}
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, this.map, false), MaintenanceType.None);
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x0015B5E6 File Offset: 0x001597E6
		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_LightningStrike.LightningMat, base.LightningBrightness), 0);
		}

		// Token: 0x040025A7 RID: 9639
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x040025A8 RID: 9640
		private Mesh boltMesh;

		// Token: 0x040025A9 RID: 9641
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}
