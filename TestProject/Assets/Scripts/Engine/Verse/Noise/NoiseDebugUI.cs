using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000496 RID: 1174
	public static class NoiseDebugUI
	{
		// Token: 0x170006DB RID: 1755
		// (set) Token: 0x060022CA RID: 8906 RVA: 0x000D328F File Offset: 0x000D148F
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000D3298 File Offset: 0x000D1498
		public static void StoreTexture(Texture2D texture, string name)
		{
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(texture, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000D32B8 File Offset: 0x000D14B8
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000D32C8 File Offset: 0x000D14C8
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000D32F8 File Offset: 0x000D14F8
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
			NoiseDebugUI.planetNoises.Add(item);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000D3328 File Offset: 0x000D1528
		public static void NoiseDebugOnGUI()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (Widgets.ButtonText(new Rect(0f, 40f, 200f, 30f), "Clear noise renders", true, true, true))
			{
				NoiseDebugUI.Clear();
			}
			if (Widgets.ButtonText(new Rect(200f, 40f, 200f, 30f), "Hide noise renders", true, true, true))
			{
				DebugViewSettings.drawRecordedNoise = false;
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (NoiseDebugUI.planetNoises.Any<NoiseDebugUI.NoisePlanet>() && Widgets.ButtonText(new Rect(400f, 40f, 200f, 30f), "Next planet noise", true, true, true))
				{
					if (Event.current.button == 1)
					{
						if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.Count - 1];
						}
						else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == 0)
						{
							NoiseDebugUI.currentPlanetNoise = null;
						}
						else
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) - 1];
						}
					}
					else if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[0];
					}
					else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == NoiseDebugUI.planetNoises.Count - 1)
					{
						NoiseDebugUI.currentPlanetNoise = null;
					}
					else
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) + 1];
					}
				}
				if (NoiseDebugUI.currentPlanetNoise != null)
				{
					Rect rect = new Rect(605f, 40f, 300f, 30f);
					Text.Font = GameFont.Medium;
					Widgets.Label(rect, NoiseDebugUI.currentPlanetNoise.name);
					Text.Font = GameFont.Small;
				}
			}
			float num = 0f;
			float num2 = 90f;
			Text.Font = GameFont.Tiny;
			foreach (NoiseDebugUI.Noise2D noise2D in NoiseDebugUI.noises2D)
			{
				Texture2D texture = noise2D.Texture;
				if (num + (float)texture.width + 5f > (float)UI.screenWidth)
				{
					num = 0f;
					num2 += (float)(texture.height + 5 + 25);
				}
				GUI.DrawTexture(new Rect(num, num2, (float)texture.width, (float)texture.height), texture);
				Rect rect2 = new Rect(num, num2 - 15f, (float)texture.width, (float)texture.height);
				GUI.color = Color.black;
				Widgets.Label(rect2, noise2D.name);
				GUI.color = Color.white;
				Widgets.Label(new Rect(rect2.x + 1f, rect2.y + 1f, rect2.width, rect2.height), noise2D.name);
				num += (float)(texture.width + 5);
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000D3644 File Offset: 0x000D1844
		public static void RenderPlanetNoise()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (NoiseDebugUI.currentPlanetNoise == null)
			{
				return;
			}
			if (NoiseDebugUI.planetNoiseMesh == null)
			{
				List<int> triangles;
				SphereGenerator.Generate(6, 100.3f, Vector3.forward, 360f, out NoiseDebugUI.planetNoiseMeshVerts, out triangles);
				NoiseDebugUI.planetNoiseMesh = new Mesh();
				NoiseDebugUI.planetNoiseMesh.name = "NoiseDebugUI";
				NoiseDebugUI.planetNoiseMesh.SetVertices(NoiseDebugUI.planetNoiseMeshVerts);
				NoiseDebugUI.planetNoiseMesh.SetTriangles(triangles, 0);
				NoiseDebugUI.lastDrawnPlanetNoise = null;
			}
			if (NoiseDebugUI.lastDrawnPlanetNoise != NoiseDebugUI.currentPlanetNoise)
			{
				NoiseDebugUI.UpdatePlanetNoiseVertexColors();
				NoiseDebugUI.lastDrawnPlanetNoise = NoiseDebugUI.currentPlanetNoise;
			}
			Graphics.DrawMesh(NoiseDebugUI.planetNoiseMesh, Vector3.zero, Quaternion.identity, WorldMaterials.VertexColor, WorldCameraManager.WorldLayer);
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000D3704 File Offset: 0x000D1904
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000D374C File Offset: 0x000D194C
		public static void ClearPlanetNoises()
		{
			NoiseDebugUI.planetNoises.Clear();
			NoiseDebugUI.currentPlanetNoise = null;
			NoiseDebugUI.lastDrawnPlanetNoise = null;
			if (NoiseDebugUI.planetNoiseMesh != null)
			{
				Mesh localPlanetNoiseMesh = NoiseDebugUI.planetNoiseMesh;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					UnityEngine.Object.Destroy(localPlanetNoiseMesh);
				});
				NoiseDebugUI.planetNoiseMesh = null;
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000D37A4 File Offset: 0x000D19A4
		private static void UpdatePlanetNoiseVertexColors()
		{
			NoiseDebugUI.planetNoiseMeshColors.Clear();
			for (int i = 0; i < NoiseDebugUI.planetNoiseMeshVerts.Count; i++)
			{
				byte b = (byte)Mathf.Clamp((NoiseDebugUI.currentPlanetNoise.noise.GetValue(NoiseDebugUI.planetNoiseMeshVerts[i]) * 0.5f + 0.5f) * 255f, 0f, 255f);
				NoiseDebugUI.planetNoiseMeshColors.Add(new Color32(b, b, b, byte.MaxValue));
			}
			NoiseDebugUI.planetNoiseMesh.SetColors(NoiseDebugUI.planetNoiseMeshColors);
		}

		// Token: 0x0400152F RID: 5423
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x04001530 RID: 5424
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x04001531 RID: 5425
		private static Mesh planetNoiseMesh;

		// Token: 0x04001532 RID: 5426
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x04001533 RID: 5427
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x04001534 RID: 5428
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x04001535 RID: 5429
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x020016C0 RID: 5824
		private class Noise2D
		{
			// Token: 0x17001519 RID: 5401
			// (get) Token: 0x060085B8 RID: 34232 RVA: 0x002B33A3 File Offset: 0x002B15A3
			public Texture2D Texture
			{
				get
				{
					if (this.tex == null)
					{
						this.tex = NoiseRenderer.NoiseRendered(this.noise);
					}
					return this.tex;
				}
			}

			// Token: 0x060085B9 RID: 34233 RVA: 0x002B33CA File Offset: 0x002B15CA
			public Noise2D(Texture2D tex, string name)
			{
				this.tex = tex;
				this.name = name;
			}

			// Token: 0x060085BA RID: 34234 RVA: 0x002B33E0 File Offset: 0x002B15E0
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x0400573C RID: 22332
			public string name;

			// Token: 0x0400573D RID: 22333
			private Texture2D tex;

			// Token: 0x0400573E RID: 22334
			private ModuleBase noise;
		}

		// Token: 0x020016C1 RID: 5825
		private class NoisePlanet
		{
			// Token: 0x060085BB RID: 34235 RVA: 0x002B33F6 File Offset: 0x002B15F6
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}

			// Token: 0x0400573F RID: 22335
			public string name;

			// Token: 0x04005740 RID: 22336
			public ModuleBase noise;
		}
	}
}
