using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004FD RID: 1277
	public static class SoundFilterUtility
	{
		// Token: 0x060024C9 RID: 9417 RVA: 0x000DA714 File Offset: 0x000D8914
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000DA73C File Offset: 0x000D893C
		private static void DisableFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T component = source.GetComponent<T>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}
}
