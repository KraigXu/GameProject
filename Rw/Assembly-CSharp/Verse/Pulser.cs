using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CA RID: 970
	public static class Pulser
	{
		// Token: 0x06001C95 RID: 7317 RVA: 0x000ADCCC File Offset: 0x000ABECC
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000ADCDC File Offset: 0x000ABEDC
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.28318548f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}
