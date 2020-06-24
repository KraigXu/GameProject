using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000497 RID: 1175
	public static class NoiseRenderer
	{
		// Token: 0x060022D5 RID: 8917 RVA: 0x000D3853 File Offset: 0x000D1A53
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000D3878 File Offset: 0x000D1A78
		public static Texture2D NoiseRendered(CellRect rect, ModuleBase noise)
		{
			Texture2D texture2D = new Texture2D(rect.Width, rect.Height);
			texture2D.name = "NoiseRender";
			foreach (IntVec2 intVec in rect.Cells2D)
			{
				texture2D.SetPixel(intVec.x, intVec.z, NoiseRenderer.ColorForValue(noise.GetValue(intVec)));
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000D3904 File Offset: 0x000D1B04
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}

		// Token: 0x04001536 RID: 5430
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x04001537 RID: 5431
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};
	}
}
