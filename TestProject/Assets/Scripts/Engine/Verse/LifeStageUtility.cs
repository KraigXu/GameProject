using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200045B RID: 1115
	public static class LifeStageUtility
	{
		// Token: 0x06002138 RID: 8504 RVA: 0x000CBD08 File Offset: 0x000C9F08
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, float volumeFactor = 1f)
		{
			SoundDef soundDef;
			float pitchFactor;
			float num;
			LifeStageUtility.GetNearestLifestageSound(pawn, getter, out soundDef, out pitchFactor, out num);
			if (soundDef == null)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
			info.pitchFactor = pitchFactor;
			info.volumeFactor = num * volumeFactor;
			soundDef.PlayOneShot(info);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000CBD64 File Offset: 0x000C9F64
		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			LifeStageAge lifeStageAge;
			for (;;)
			{
				lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					break;
				}
				num++;
				if (num < 0 || num >= pawn.RaceProps.lifeStageAges.Count)
				{
					goto IL_84;
				}
			}
			pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
			volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
			return;
			IL_84:
			def = null;
			pitch = (volume = 1f);
		}
	}
}
