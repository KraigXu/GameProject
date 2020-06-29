using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SampleOneShot : Sample
	{
		
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000D9DE2 File Offset: 0x000D7FE2
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		
		// (get) Token: 0x060024B0 RID: 9392 RVA: 0x000D9DEA File Offset: 0x000D7FEA
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		
		// (get) Token: 0x060024B1 RID: 9393 RVA: 0x000D9DF3 File Offset: 0x000D7FF3
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x000D9DFC File Offset: 0x000D7FFC
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		
		// (get) Token: 0x060024B3 RID: 9395 RVA: 0x000D9E04 File Offset: 0x000D8004
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		
		public static SampleOneShot TryMakeAndPlay(SubSoundDef def, AudioClip clip, SoundInfo info)
		{
			if ((double)info.pitchFactor <= 0.0001)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Played sound with pitchFactor ",
					info.pitchFactor,
					": ",
					def,
					", ",
					info
				}), 632321, false);
				return null;
			}
			SampleOneShot sampleOneShot = new SampleOneShot(def);
			sampleOneShot.info = info;
			sampleOneShot.source = Find.SoundRoot.sourcePool.GetSource(def.onCamera);
			if (sampleOneShot.source == null)
			{
				return null;
			}
			sampleOneShot.source.clip = clip;
			sampleOneShot.source.volume = sampleOneShot.SanitizedVolume;
			sampleOneShot.source.pitch = sampleOneShot.SanitizedPitch;
			sampleOneShot.source.minDistance = sampleOneShot.subDef.distRange.TrueMin;
			sampleOneShot.source.maxDistance = sampleOneShot.subDef.distRange.TrueMax;
			if (!def.onCamera)
			{
				sampleOneShot.source.gameObject.transform.position = info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				sampleOneShot.source.minDistance = def.distRange.TrueMin;
				sampleOneShot.source.maxDistance = def.distRange.TrueMax;
				sampleOneShot.source.spatialBlend = 1f;
			}
			else
			{
				sampleOneShot.source.spatialBlend = 0f;
			}
			for (int i = 0; i < def.filters.Count; i++)
			{
				def.filters[i].SetupOn(sampleOneShot.source);
			}
			foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
			{
				sampleOneShot.externalParams[keyValuePair.Key] = keyValuePair.Value;
			}
			sampleOneShot.Update();
			sampleOneShot.source.Play();
			Find.SoundRoot.oneShotManager.TryAddPlayingOneShot(sampleOneShot);
			return sampleOneShot;
		}

		
		public SoundInfo info;

		
		private SoundParams externalParams = new SoundParams();
	}
}
