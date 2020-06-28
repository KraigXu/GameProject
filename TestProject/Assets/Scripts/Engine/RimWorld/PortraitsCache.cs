using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E13 RID: 3603
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x0600571D RID: 22301 RVA: 0x001CF834 File Offset: 0x001CDA34
		public static RenderTexture Get(Pawn pawn, Vector2 size, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f, bool supersample = true, bool compensateForUIScale = true)
		{
			if (supersample)
			{
				size *= 1.25f;
			}
			if (compensateForUIScale)
			{
				size *= Prefs.UIScale;
			}
			Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.GetOrCreateCachedPortraitsWithParams(size, cameraOffset, cameraZoom).CachedPortraits;
			PortraitsCache.CachedPortrait cachedPortrait;
			if (dictionary.TryGetValue(pawn, out cachedPortrait))
			{
				if (!cachedPortrait.RenderTexture.IsCreated())
				{
					cachedPortrait.RenderTexture.Create();
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom);
				}
				else if (cachedPortrait.Dirty)
				{
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom);
				}
				dictionary.Remove(pawn);
				dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				return cachedPortrait.RenderTexture;
			}
			RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
			PortraitsCache.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
			dictionary.Add(pawn, new PortraitsCache.CachedPortrait(renderTexture, false, Time.time));
			return renderTexture;
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x001CF910 File Offset: 0x001CDB10
		public static void SetDirty(Pawn pawn)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.CachedPortrait cachedPortrait;
				if (dictionary.TryGetValue(pawn, out cachedPortrait) && !cachedPortrait.Dirty)
				{
					dictionary.Remove(pawn);
					dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
			}
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x001CF97D File Offset: 0x001CDB7D
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x001CF98C File Offset: 0x001CDB8C
		public static void Clear()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in PortraitsCache.cachedPortraits[i].CachedPortraits)
				{
					PortraitsCache.DestroyRenderTexture(keyValuePair.Value.RenderTexture);
				}
			}
			PortraitsCache.cachedPortraits.Clear();
			for (int j = 0; j < PortraitsCache.renderTexturesPool.Count; j++)
			{
				PortraitsCache.DestroyRenderTexture(PortraitsCache.renderTexturesPool[j]);
			}
			PortraitsCache.renderTexturesPool.Clear();
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x001CFA50 File Offset: 0x001CDC50
		private static PortraitsCache.CachedPortraitsWithParams GetOrCreateCachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				if (PortraitsCache.cachedPortraits[i].Size == size && PortraitsCache.cachedPortraits[i].CameraOffset == cameraOffset && PortraitsCache.cachedPortraits[i].CameraZoom == cameraZoom)
				{
					return PortraitsCache.cachedPortraits[i];
				}
			}
			PortraitsCache.CachedPortraitsWithParams cachedPortraitsWithParams = new PortraitsCache.CachedPortraitsWithParams(size, cameraOffset, cameraZoom);
			PortraitsCache.cachedPortraits.Add(cachedPortraitsWithParams);
			return cachedPortraitsWithParams;
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x001CFAE0 File Offset: 0x001CDCE0
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x001CFAF0 File Offset: 0x001CDCF0
		private static void RemoveExpiredCachedPortraits()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toRemove.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in dictionary)
				{
					if (keyValuePair.Value.Expired)
					{
						PortraitsCache.toRemove.Add(keyValuePair.Key);
						PortraitsCache.renderTexturesPool.Add(keyValuePair.Value.RenderTexture);
					}
				}
				for (int j = 0; j < PortraitsCache.toRemove.Count; j++)
				{
					dictionary.Remove(PortraitsCache.toRemove[j]);
				}
				PortraitsCache.toRemove.Clear();
			}
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001CFBE4 File Offset: 0x001CDDE4
		private static void SetAnimatedPortraitsDirty()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toSetDirty.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in dictionary)
				{
					if (PortraitsCache.IsAnimated(keyValuePair.Key) && !keyValuePair.Value.Dirty)
					{
						PortraitsCache.toSetDirty.Add(keyValuePair.Key);
					}
				}
				for (int j = 0; j < PortraitsCache.toSetDirty.Count; j++)
				{
					PortraitsCache.CachedPortrait cachedPortrait = dictionary[PortraitsCache.toSetDirty[j]];
					dictionary.Remove(PortraitsCache.toSetDirty[j]);
					dictionary.Add(PortraitsCache.toSetDirty[j], new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
				PortraitsCache.toSetDirty.Clear();
			}
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x001CFD08 File Offset: 0x001CDF08
		private static RenderTexture NewRenderTexture(Vector2 size)
		{
			int num = PortraitsCache.renderTexturesPool.FindLastIndex((RenderTexture x) => x.width == (int)size.x && x.height == (int)size.y);
			if (num != -1)
			{
				RenderTexture result = PortraitsCache.renderTexturesPool[num];
				PortraitsCache.renderTexturesPool.RemoveAt(num);
				return result;
			}
			return new RenderTexture((int)size.x, (int)size.y, 24)
			{
				filterMode = FilterMode.Bilinear
			};
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x001CFD7A File Offset: 0x001CDF7A
		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001CFD8A File Offset: 0x001CDF8A
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Token: 0x04002F8C RID: 12172
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04002F8D RID: 12173
		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		// Token: 0x04002F8E RID: 12174
		private const float SupersampleScale = 1.25f;

		// Token: 0x04002F8F RID: 12175
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x04002F90 RID: 12176
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x02001CF2 RID: 7410
		private struct CachedPortrait
		{
			// Token: 0x17001983 RID: 6531
			// (get) Token: 0x0600A3CD RID: 41933 RVA: 0x0030B411 File Offset: 0x00309611
			// (set) Token: 0x0600A3CE RID: 41934 RVA: 0x0030B419 File Offset: 0x00309619
			public RenderTexture RenderTexture { get; private set; }

			// Token: 0x17001984 RID: 6532
			// (get) Token: 0x0600A3CF RID: 41935 RVA: 0x0030B422 File Offset: 0x00309622
			// (set) Token: 0x0600A3D0 RID: 41936 RVA: 0x0030B42A File Offset: 0x0030962A
			public bool Dirty { get; private set; }

			// Token: 0x17001985 RID: 6533
			// (get) Token: 0x0600A3D1 RID: 41937 RVA: 0x0030B433 File Offset: 0x00309633
			// (set) Token: 0x0600A3D2 RID: 41938 RVA: 0x0030B43B File Offset: 0x0030963B
			public float LastUseTime { get; private set; }

			// Token: 0x17001986 RID: 6534
			// (get) Token: 0x0600A3D3 RID: 41939 RVA: 0x0030B444 File Offset: 0x00309644
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}

			// Token: 0x0600A3D4 RID: 41940 RVA: 0x0030B459 File Offset: 0x00309659
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x04006DC2 RID: 28098
			private const float CacheDuration = 1f;
		}

		// Token: 0x02001CF3 RID: 7411
		private struct CachedPortraitsWithParams
		{
			// Token: 0x17001987 RID: 6535
			// (get) Token: 0x0600A3D5 RID: 41941 RVA: 0x0030B477 File Offset: 0x00309677
			// (set) Token: 0x0600A3D6 RID: 41942 RVA: 0x0030B47F File Offset: 0x0030967F
			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits { get; private set; }

			// Token: 0x17001988 RID: 6536
			// (get) Token: 0x0600A3D7 RID: 41943 RVA: 0x0030B488 File Offset: 0x00309688
			// (set) Token: 0x0600A3D8 RID: 41944 RVA: 0x0030B490 File Offset: 0x00309690
			public Vector2 Size { get; private set; }

			// Token: 0x17001989 RID: 6537
			// (get) Token: 0x0600A3D9 RID: 41945 RVA: 0x0030B499 File Offset: 0x00309699
			// (set) Token: 0x0600A3DA RID: 41946 RVA: 0x0030B4A1 File Offset: 0x003096A1
			public Vector3 CameraOffset { get; private set; }

			// Token: 0x1700198A RID: 6538
			// (get) Token: 0x0600A3DB RID: 41947 RVA: 0x0030B4AA File Offset: 0x003096AA
			// (set) Token: 0x0600A3DC RID: 41948 RVA: 0x0030B4B2 File Offset: 0x003096B2
			public float CameraZoom { get; private set; }

			// Token: 0x0600A3DD RID: 41949 RVA: 0x0030B4BB File Offset: 0x003096BB
			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}
		}
	}
}
