using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	
	public abstract class Sample
	{
		
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000D9926 File Offset: 0x000D7B26
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x000D9934 File Offset: 0x000D7B34
		public int AgeTicks
		{
			get
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					return Find.TickManager.TicksGame - this.startTick;
				}
				return (int)(this.AgeRealTime * 60f);
			}
		}

		
		// (get) Token: 0x0600249B RID: 9371
		public abstract float ParentStartRealTime { get; }

		
		// (get) Token: 0x0600249C RID: 9372
		public abstract float ParentStartTick { get; }

		
		// (get) Token: 0x0600249D RID: 9373
		public abstract float ParentHashCode { get; }

		
		// (get) Token: 0x0600249E RID: 9374
		public abstract SoundParams ExternalParams { get; }

		
		// (get) Token: 0x0600249F RID: 9375
		public abstract SoundInfo Info { get; }

		
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x000D9960 File Offset: 0x000D7B60
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x000D9983 File Offset: 0x000D7B83
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000D9990 File Offset: 0x000D7B90
		protected float MappedVolumeMultiplier
		{
			get
			{
				float num = 1f;
				foreach (float num2 in this.volumeInMappings.Values)
				{
					num *= num2;
				}
				return num;
			}
		}

		
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x000D99EC File Offset: 0x000D7BEC
		protected float ContextVolumeMultiplier
		{
			get
			{
				if (SoundDefHelper.CorrectContextNow(this.subDef.parentDef, this.Map))
				{
					return 1f;
				}
				return 0f;
			}
		}

		
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000D9A14 File Offset: 0x000D7C14
		protected virtual float Volume
		{
			get
			{
				if (this.subDef.muteWhenPaused && Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused && !this.TestPlaying)
				{
					return 0f;
				}
				return this.resolvedVolume * this.Info.volumeFactor * this.MappedVolumeMultiplier * this.ContextVolumeMultiplier;
			}
		}

		
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x000D9A70 File Offset: 0x000D7C70
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x000D9A88 File Offset: 0x000D7C88
		protected virtual float Pitch
		{
			get
			{
				float num = this.resolvedPitch * this.Info.pitchFactor;
				if (this.subDef.tempoAffectedByGameSpeed && Current.ProgramState == ProgramState.Playing && !this.TestPlaying && !Find.TickManager.Paused)
				{
					num *= Find.TickManager.TickRateMultiplier;
				}
				return num;
			}
		}

		
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x000D9ADF File Offset: 0x000D7CDF
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		
		public Sample(SubSoundDef def)
		{
			this.subDef = def;
			this.resolvedVolume = def.RandomizedVolume();
			this.resolvedPitch = def.pitchRange.RandomInRange;
			this.startRealTime = Time.realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.startTick = 0;
			}
			foreach (SoundParamTarget_Volume key in (from m in this.subDef.paramMappings
			select m.outParam).OfType<SoundParamTarget_Volume>())
			{
				this.volumeInMappings.Add(key, 0f);
			}
		}

		
		public virtual void Update()
		{
			this.source.pitch = this.SanitizedPitch;
			this.ApplyMappedParameters();
			this.source.volume = this.SanitizedVolume;
			if (this.source.volume < 0.001f)
			{
				this.source.mute = true;
			}
			else
			{
				this.source.mute = false;
			}
			if (this.subDef.tempoAffectedByGameSpeed && !this.TestPlaying)
			{
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused)
				{
					if (this.source.isPlaying)
					{
						this.source.Pause();
						return;
					}
				}
				else if (!this.source.isPlaying)
				{
					this.source.UnPause();
				}
			}
		}

		
		public void ApplyMappedParameters()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.paramUpdateMode != SoundParamUpdateMode.OncePerSample || !this.mappingsApplied)
				{
					soundParameterMapping.Apply(this);
				}
			}
			this.mappingsApplied = true;
		}

		
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		
		public virtual void SampleCleanup()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.curve.HasView)
				{
					soundParameterMapping.curve.View.ClearDebugInputFrom(this);
				}
			}
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Sample_",
				this.subDef.name,
				" volume=",
				this.source.volume,
				" at ",
				this.source.transform.position.ToIntVec3()
			});
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		
		public SubSoundDef subDef;

		
		public AudioSource source;

		
		public float startRealTime;

		
		public int startTick;

		
		public float resolvedVolume;

		
		public float resolvedPitch;

		
		private bool mappingsApplied;

		
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
