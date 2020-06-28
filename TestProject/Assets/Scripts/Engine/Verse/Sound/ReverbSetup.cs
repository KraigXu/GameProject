using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D4 RID: 1236
	public class ReverbSetup
	{
		// Token: 0x0600243C RID: 9276 RVA: 0x000D8804 File Offset: 0x000D6A04
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AudioReverbPreset)))
				{
					AudioReverbPreset audioReverbPreset = (AudioReverbPreset)obj;
					if (audioReverbPreset != AudioReverbPreset.User)
					{
						AudioReverbPreset localPreset = audioReverbPreset;
						list.Add(new FloatMenuOption(audioReverbPreset.ToString(), delegate
						{
							this.SetupAs(localPreset);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000D88D8 File Offset: 0x000D6AD8
		public void ApplyTo(AudioReverbFilter filter)
		{
			filter.dryLevel = this.dryLevel;
			filter.room = this.room;
			filter.roomHF = this.roomHF;
			filter.roomLF = this.roomLF;
			filter.decayTime = this.decayTime;
			filter.decayHFRatio = this.decayHFRatio;
			filter.reflectionsLevel = this.reflectionsLevel;
			filter.reflectionsDelay = this.reflectionsDelay;
			filter.reverbLevel = this.reverbLevel;
			filter.reverbDelay = this.reverbDelay;
			filter.hfReference = this.hfReference;
			filter.lfReference = this.lfReference;
			filter.diffusion = this.diffusion;
			filter.density = this.density;
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000D8990 File Offset: 0x000D6B90
		public static ReverbSetup Lerp(ReverbSetup A, ReverbSetup B, float t)
		{
			return new ReverbSetup
			{
				dryLevel = Mathf.Lerp(A.dryLevel, B.dryLevel, t),
				room = Mathf.Lerp(A.room, B.room, t),
				roomHF = Mathf.Lerp(A.roomHF, B.roomHF, t),
				roomLF = Mathf.Lerp(A.roomLF, B.roomLF, t),
				decayTime = Mathf.Lerp(A.decayTime, B.decayTime, t),
				decayHFRatio = Mathf.Lerp(A.decayHFRatio, B.decayHFRatio, t),
				reflectionsLevel = Mathf.Lerp(A.reflectionsLevel, B.reflectionsLevel, t),
				reflectionsDelay = Mathf.Lerp(A.reflectionsDelay, B.reflectionsDelay, t),
				reverbLevel = Mathf.Lerp(A.reverbLevel, B.reverbLevel, t),
				reverbDelay = Mathf.Lerp(A.reverbDelay, B.reverbDelay, t),
				hfReference = Mathf.Lerp(A.hfReference, B.hfReference, t),
				lfReference = Mathf.Lerp(A.lfReference, B.lfReference, t),
				diffusion = Mathf.Lerp(A.diffusion, B.diffusion, t),
				density = Mathf.Lerp(A.density, B.density, t)
			};
		}

		// Token: 0x040015DB RID: 5595
		public float dryLevel;

		// Token: 0x040015DC RID: 5596
		public float room;

		// Token: 0x040015DD RID: 5597
		public float roomHF;

		// Token: 0x040015DE RID: 5598
		public float roomLF;

		// Token: 0x040015DF RID: 5599
		public float decayTime = 1f;

		// Token: 0x040015E0 RID: 5600
		public float decayHFRatio = 0.5f;

		// Token: 0x040015E1 RID: 5601
		public float reflectionsLevel = -10000f;

		// Token: 0x040015E2 RID: 5602
		public float reflectionsDelay;

		// Token: 0x040015E3 RID: 5603
		public float reverbLevel;

		// Token: 0x040015E4 RID: 5604
		public float reverbDelay = 0.04f;

		// Token: 0x040015E5 RID: 5605
		public float hfReference = 5000f;

		// Token: 0x040015E6 RID: 5606
		public float lfReference = 250f;

		// Token: 0x040015E7 RID: 5607
		public float diffusion = 100f;

		// Token: 0x040015E8 RID: 5608
		public float density = 100f;
	}
}
