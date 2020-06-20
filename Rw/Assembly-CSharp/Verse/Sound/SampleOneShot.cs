using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004FA RID: 1274
	public class SampleOneShot : Sample
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000D9DE2 File Offset: 0x000D7FE2
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060024B0 RID: 9392 RVA: 0x000D9DEA File Offset: 0x000D7FEA
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060024B1 RID: 9393 RVA: 0x000D9DF3 File Offset: 0x000D7FF3
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x000D9DFC File Offset: 0x000D7FFC
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060024B3 RID: 9395 RVA: 0x000D9E04 File Offset: 0x000D8004
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000D9E0C File Offset: 0x000D800C
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000D9E20 File Offset: 0x000D8020
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

		// Token: 0x0400164B RID: 5707
		public SoundInfo info;

		// Token: 0x0400164C RID: 5708
		private SoundParams externalParams = new SoundParams();
	}
}
