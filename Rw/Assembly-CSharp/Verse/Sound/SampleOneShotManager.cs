using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x020004FB RID: 1275
	public class SampleOneShotManager
	{
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x000DA058 File Offset: 0x000D8258
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000DA060 File Offset: 0x000D8260
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000DA094 File Offset: 0x000D8294
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000DA0B3 File Offset: 0x000D82B3
		private float ImportanceOf(SoundDef def, SoundInfo info, float ageRealTime)
		{
			if (def.priorityMode == VoicePriorityMode.PrioritizeNearest)
			{
				return 1f / (this.CameraDistanceSquaredOf(info) + 1f);
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeNewest)
			{
				return 1f / (ageRealTime + 1f);
			}
			throw new NotImplementedException();
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000DA0F0 File Offset: 0x000D82F0
		public bool CanAddPlayingOneShot(SoundDef def, SoundInfo info)
		{
			if (!SoundDefHelper.CorrectContextNow(def, info.Maker.Map))
			{
				return false;
			}
			if ((from s in this.samples
			where s.subDef.parentDef == def && s.AgeRealTime < 0.05f
			select s).Count<SampleOneShot>() >= def.MaxSimultaneousSamples)
			{
				return false;
			}
			SampleOneShot sampleOneShot = this.LeastImportantOf(def);
			return sampleOneShot == null || this.ImportanceOf(def, info, 0f) >= this.ImportanceOf(sampleOneShot);
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000DA184 File Offset: 0x000D8384
		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			if ((from s in this.samples
			where s.subDef == newSample.subDef
			select s).Count<SampleOneShot>() >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef.parentDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000DA214 File Offset: 0x000D8414
		private SampleOneShot LeastImportantOf(SoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.parentDef == def && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000DA26C File Offset: 0x000D846C
		public void SampleOneShotManagerUpdate()
		{
			for (int i = 0; i < this.samples.Count; i++)
			{
				this.samples[i].Update();
			}
			this.cleanupList.Clear();
			for (int j = 0; j < this.samples.Count; j++)
			{
				SampleOneShot sampleOneShot = this.samples[j];
				if (sampleOneShot.source == null || !sampleOneShot.source.isPlaying || !SoundDefHelper.CorrectContextNow(sampleOneShot.subDef.parentDef, sampleOneShot.Map))
				{
					if (sampleOneShot.source != null && sampleOneShot.source.isPlaying)
					{
						sampleOneShot.source.Stop();
					}
					sampleOneShot.SampleCleanup();
					this.cleanupList.Add(sampleOneShot);
				}
			}
			if (this.cleanupList.Count > 0)
			{
				this.samples.RemoveAll((SampleOneShot s) => this.cleanupList.Contains(s));
			}
		}

		// Token: 0x0400164D RID: 5709
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x0400164E RID: 5710
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
