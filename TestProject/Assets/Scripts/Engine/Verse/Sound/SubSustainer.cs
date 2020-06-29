﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SubSustainer
	{
		
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000DB0A3 File Offset: 0x000D92A3
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000DB0B0 File Offset: 0x000D92B0
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		
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

		
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		
		public Sustainer parent;

		
		public SubSoundDef subDef;

		
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		
		private float nextSampleStartTime;

		
		public int creationFrame = -1;

		
		public int creationTick = -1;

		
		public float creationRealTime = -1f;

		
		private const float MinSampleStartInterval = 0.01f;
	}
}
