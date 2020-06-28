﻿using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000083 RID: 131
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00017A17 File Offset: 0x00015C17
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00017A30 File Offset: 0x00015C30
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

		// Token: 0x04000214 RID: 532
		private const float DarkAmp = 0.4f;
	}
}
