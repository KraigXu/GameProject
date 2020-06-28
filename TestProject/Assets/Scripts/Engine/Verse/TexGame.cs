using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000051 RID: 81
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x000134F8 File Offset: 0x000116F8
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}

		// Token: 0x0400010B RID: 267
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x0400010C RID: 268
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x0400010D RID: 269
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);
	}
}
