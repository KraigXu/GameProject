using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004FC RID: 1276
	public class SampleSustainer : Sample
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060024C0 RID: 9408 RVA: 0x000DA38E File Offset: 0x000D858E
		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060024C1 RID: 9409 RVA: 0x000DA39B File Offset: 0x000D859B
		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060024C2 RID: 9410 RVA: 0x000DA3A9 File Offset: 0x000D85A9
		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x000DA3B7 File Offset: 0x000D85B7
		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060024C4 RID: 9412 RVA: 0x000DA3C4 File Offset: 0x000D85C4
		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x000DA3D4 File Offset: 0x000D85D4
		protected override float Volume
		{
			get
			{
				float num = base.Volume * this.subSustainer.parent.scopeFader.inScopePercent;
				float num2 = 1f;
				if (this.subSustainer.parent.Ended)
				{
					num2 = 1f - Mathf.Min(this.subSustainer.parent.TimeSinceEnd / this.subDef.parentDef.sustainFadeoutTime, 1f);
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (base.AgeRealTime < this.subDef.sustainAttack)
				{
					if (this.resolvedSkipAttack || this.subDef.sustainAttack < 0.01f)
					{
						return num * num2;
					}
					float num3 = base.AgeRealTime / this.subDef.sustainAttack;
					num3 = Mathf.Sqrt(num3);
					return Mathf.Lerp(0f, num, num3) * num2;
				}
				else
				{
					if (realtimeSinceStartup > this.scheduledEndTime - this.subDef.sustainRelease)
					{
						float num4 = (realtimeSinceStartup - (this.scheduledEndTime - this.subDef.sustainRelease)) / this.subDef.sustainRelease;
						num4 = 1f - num4;
						num4 = Mathf.Max(num4, 0f);
						num4 = Mathf.Sqrt(num4);
						num4 = 1f - num4;
						return Mathf.Lerp(num, 0f, num4) * num2;
					}
					return num * num2;
				}
			}
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000DA51D File Offset: 0x000D871D
		private SampleSustainer(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000DA528 File Offset: 0x000D8728
		public static SampleSustainer TryMakeAndPlay(SubSustainer subSus, AudioClip clip, float scheduledEndTime)
		{
			SampleSustainer sampleSustainer = new SampleSustainer(subSus.subDef);
			sampleSustainer.subSustainer = subSus;
			sampleSustainer.scheduledEndTime = scheduledEndTime;
			GameObject gameObject = new GameObject(string.Concat(new object[]
			{
				"SampleSource_",
				sampleSustainer.subDef.name,
				"_",
				sampleSustainer.startRealTime
			}));
			GameObject gameObject2 = subSus.subDef.onCamera ? Find.Camera.gameObject : subSus.parent.worldRootObject;
			gameObject.transform.parent = gameObject2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			sampleSustainer.source = AudioSourceMaker.NewAudioSourceOn(gameObject);
			if (sampleSustainer.source == null)
			{
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				return null;
			}
			sampleSustainer.source.clip = clip;
			sampleSustainer.source.volume = sampleSustainer.SanitizedVolume;
			sampleSustainer.source.pitch = sampleSustainer.SanitizedPitch;
			sampleSustainer.source.minDistance = sampleSustainer.subDef.distRange.TrueMin;
			sampleSustainer.source.maxDistance = sampleSustainer.subDef.distRange.TrueMax;
			sampleSustainer.source.spatialBlend = 1f;
			List<SoundFilter> filters = sampleSustainer.subDef.filters;
			for (int i = 0; i < filters.Count; i++)
			{
				filters[i].SetupOn(sampleSustainer.source);
			}
			if (sampleSustainer.subDef.sustainLoop)
			{
				sampleSustainer.source.loop = true;
			}
			sampleSustainer.Update();
			sampleSustainer.source.Play();
			sampleSustainer.source.Play();
			return sampleSustainer;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000DA6DB File Offset: 0x000D88DB
		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if (this.source != null && this.source.gameObject != null)
			{
				UnityEngine.Object.Destroy(this.source.gameObject);
			}
		}

		// Token: 0x0400164F RID: 5711
		public SubSustainer subSustainer;

		// Token: 0x04001650 RID: 5712
		public float scheduledEndTime;

		// Token: 0x04001651 RID: 5713
		public bool resolvedSkipAttack;
	}
}
