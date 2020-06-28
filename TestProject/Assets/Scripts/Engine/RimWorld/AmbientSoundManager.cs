using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C2D RID: 3117
	public static class AmbientSoundManager
	{
		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06004A49 RID: 19017 RVA: 0x00191C70 File Offset: 0x0018FE70
		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06004A4A RID: 19018 RVA: 0x00191C86 File Offset: 0x0018FE86
		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x00191C9C File Offset: 0x0018FE9C
		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x00191CB6 File Offset: 0x0018FEB6
		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(AmbientSoundManager.recreateMapSustainers);
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x00191CC4 File Offset: 0x0018FEC4
		private static void RecreateMapSustainers()
		{
			if (!AmbientSoundManager.AltitudeWindSoundCreated)
			{
				SoundDefOf.Ambient_AltitudeWind.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
			SustainerManager sustainerManager = Find.SoundRoot.sustainerManager;
			for (int i = 0; i < AmbientSoundManager.biomeAmbientSustainers.Count; i++)
			{
				Sustainer sustainer = AmbientSoundManager.biomeAmbientSustainers[i];
				if (sustainerManager.AllSustainers.Contains(sustainer) && !sustainer.Ended)
				{
					sustainer.End();
				}
			}
			AmbientSoundManager.biomeAmbientSustainers.Clear();
			if (Find.CurrentMap != null)
			{
				List<SoundDef> soundsAmbient = Find.CurrentMap.Biome.soundsAmbient;
				for (int j = 0; j < soundsAmbient.Count; j++)
				{
					Sustainer item = soundsAmbient[j].TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
					AmbientSoundManager.biomeAmbientSustainers.Add(item);
				}
			}
		}

		// Token: 0x04002A27 RID: 10791
		private static List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		// Token: 0x04002A28 RID: 10792
		private static Action recreateMapSustainers = new Action(AmbientSoundManager.RecreateMapSustainers);
	}
}
