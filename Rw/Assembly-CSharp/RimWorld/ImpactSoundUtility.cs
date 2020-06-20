using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000FC5 RID: 4037
	public static class ImpactSoundUtility
	{
		// Token: 0x0600610E RID: 24846 RVA: 0x0021AF68 File Offset: 0x00219168
		public static void PlayImpactSound(Thing hitThing, ImpactSoundTypeDef ist, Map map)
		{
			if (ist == null)
			{
				return;
			}
			if (ist.playOnlyIfHitPawn && !(hitThing is Pawn))
			{
				return;
			}
			if (map == null)
			{
				Log.Warning("Can't play impact sound because map is null.", false);
				return;
			}
			SoundDef soundDef;
			if (hitThing.Stuff != null)
			{
				soundDef = hitThing.Stuff.stuffProps.soundImpactStuff;
			}
			else
			{
				soundDef = hitThing.def.soundImpactDefault;
			}
			if (soundDef.NullOrUndefined())
			{
				soundDef = SoundDefOf.BulletImpact_Ground;
			}
			soundDef.PlayOneShot(new TargetInfo(hitThing.PositionHeld, map, false));
		}
	}
}
