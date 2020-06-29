using System;
using UnityEngine;

namespace Verse
{
	
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00017A17 File Offset: 0x00015C17
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		
		public override Color NewRandomizedColor()
		{
			if (Rand.Value < 0.1f)
			{
				return Color.white;
			}
			if (Rand.Value < 0.1f)
			{
				return new Color(0.4f, 0.4f, 0.4f);
			}
			Color white = Color.white;
			float num = Rand.Range(0f, 0.6f);
			white.r -= num * Rand.Value;
			white.g -= num * Rand.Value;
			white.b -= num * Rand.Value;
			if (Rand.Value < 0.2f)
			{
				white.r *= 0.4f;
				white.g *= 0.4f;
				white.b *= 0.4f;
			}
			return white;
		}

		
		private const float DarkAmp = 0.4f;
	}
}
