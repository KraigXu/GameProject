using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200050A RID: 1290
	public class SubSustainer
	{
		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000DB0A3 File Offset: 0x000D92A3
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000DB0B0 File Offset: 0x000D92B0
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000DB0C0 File Offset: 0x000D92C0
		public SubSustainer(Sustainer parent, SubSoundDef subSoundDef)
		{
			this.parent = parent;
			this.subDef = subSoundDef;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.creationFrame = Time.frameCount;
				this.creationRealTime = Time.realtimeSinceStartup;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.creationTick = Find.TickManager.TicksGame;
				}
				if (this.subDef.startDelayRange.TrueMax < 0.001f)
				{
					this.StartSample();
					return;
				}
				this.nextSampleStartTime = Time.realtimeSinceStartup + this.subDef.startDelayRange.RandomInRange;
			});
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000DB118 File Offset: 0x000D9318
		private void StartSample()
		{
			ResolvedGrain resolvedGrain = this.subDef.RandomizedResolvedGrain();
			if (resolvedGrain == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"SubSustainer for ",
					this.subDef,
					" of ",
					this.parent.def,
					" could not resolve any grains."
				}), false);
				this.parent.End();
				return;
			}
			float num;
			if (this.subDef.sustainLoop)
			{
				num = this.subDef.sustainLoopDurationRange.RandomInRange;
			}
			else
			{
				num = resolvedGrain.duration;
			}
			float num2 = Time.realtimeSinceStartup + num;
			this.nextSampleStartTime = num2 + this.subDef.sustainIntervalRange.RandomInRange;
			if (this.nextSampleStartTime < Time.realtimeSinceStartup + 0.01f)
			{
				this.nextSampleStartTime = Time.realtimeSinceStartup + 0.01f;
			}
			if (resolvedGrain is ResolvedGrain_Silence)
			{
				return;
			}
			SampleSustainer sampleSustainer = SampleSustainer.TryMakeAndPlay(this, ((ResolvedGrain_Clip)resolvedGrain).clip, num2);
			if (sampleSustainer == null)
			{
				return;
			}
			if (this.subDef.sustainSkipFirstAttack && Time.frameCount == this.creationFrame)
			{
				sampleSustainer.resolvedSkipAttack = true;
			}
			this.samples.Add(sampleSustainer);
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000DB238 File Offset: 0x000D9438
		public void SubSustainerUpdate()
		{
			for (int i = this.samples.Count - 1; i >= 0; i--)
			{
				if (Time.realtimeSinceStartup > this.samples[i].scheduledEndTime)
				{
					this.EndSample(this.samples[i]);
				}
			}
			if (Time.realtimeSinceStartup > this.nextSampleStartTime)
			{
				this.StartSample();
			}
			for (int j = 0; j < this.samples.Count; j++)
			{
				this.samples[j].Update();
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000DB2C1 File Offset: 0x000D94C1
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000DB2D6 File Offset: 0x000D94D6
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000DB2FA File Offset: 0x000D94FA
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000DB31C File Offset: 0x000D951C
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000DB334 File Offset: 0x000D9534
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400166D RID: 5741
		public Sustainer parent;

		// Token: 0x0400166E RID: 5742
		public SubSoundDef subDef;

		// Token: 0x0400166F RID: 5743
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x04001670 RID: 5744
		private float nextSampleStartTime;

		// Token: 0x04001671 RID: 5745
		public int creationFrame = -1;

		// Token: 0x04001672 RID: 5746
		public int creationTick = -1;

		// Token: 0x04001673 RID: 5747
		public float creationRealTime = -1f;

		// Token: 0x04001674 RID: 5748
		private const float MinSampleStartInterval = 0.01f;
	}
}
