using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x0200052C RID: 1324
	public static class ToilEffects
	{
		// Token: 0x060025E9 RID: 9705 RVA: 0x000E04CC File Offset: 0x000DE6CC
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000E050C File Offset: 0x000DE70C
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000E054C File Offset: 0x000DE74C
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef)
		{
			return toil.PlaySustainerOrSound(() => soundDef);
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000E0578 File Offset: 0x000DE778
		public static Toil PlaySustainerOrSound(this Toil toil, Func<SoundDef> soundDefGetter)
		{
			Sustainer sustainer = null;
			toil.AddPreInitAction(delegate
			{
				SoundDef soundDef = soundDefGetter();
				if (soundDef != null && !soundDef.sustain)
				{
					soundDef.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
				}
			});
			toil.AddPreTickAction(delegate
			{
				if (sustainer == null || sustainer.Ended)
				{
					SoundDef soundDef = soundDefGetter();
					if (soundDef != null && soundDef.sustain)
					{
						SoundInfo info = SoundInfo.InMap(toil.actor, MaintenanceType.PerTick);
						sustainer = soundDef.TrySpawnSustainer(info);
						return;
					}
				}
				else
				{
					sustainer.Maintain();
				}
			});
			return toil;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000E05D4 File Offset: 0x000DE7D4
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind)
		{
			return toil.WithEffect(() => effectDef, ind);
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000E0604 File Offset: 0x000DE804
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind));
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000E0640 File Offset: 0x000DE840
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing)
		{
			return toil.WithEffect(effecterDefGetter, () => thing);
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000E0670 File Offset: 0x000DE870
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Func<LocalTargetInfo> effectTargetGetter)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (effecter != null)
				{
					effecter.EffectTick(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
					return;
				}
				EffecterDef effecterDef = effecterDefGetter();
				if (effecterDef == null)
				{
					return;
				}
				effecter = effecterDef.Spawn();
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000E06D4 File Offset: 0x000DE8D4
		public static Toil WithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (toil.actor.Faction != Faction.OfPlayer)
				{
					return;
				}
				if (effecter == null)
				{
					EffecterDef progressBar = EffecterDefOf.ProgressBar;
					effecter = progressBar.Spawn();
					return;
				}
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ind);
				if (!target.IsValid || (target.HasThing && !target.Thing.Spawned))
				{
					effecter.EffectTick(toil.actor, TargetInfo.Invalid);
				}
				else if (interpolateBetweenActorAndTarget)
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
				}
				else
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
				}
				MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
				if (mote != null)
				{
					mote.progress = Mathf.Clamp01(progressGetter());
					mote.offsetZ = offsetZ;
				}
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000E0748 File Offset: 0x000DE948
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000E077C File Offset: 0x000DE97C
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ);
		}
	}
}
