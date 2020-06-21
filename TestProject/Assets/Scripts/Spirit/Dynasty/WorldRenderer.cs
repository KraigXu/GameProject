using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011F9 RID: 4601
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06006A67 RID: 27239 RVA: 0x00251B00 File Offset: 0x0024FD00
		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				for (int i = 0; i < this.layers.Count; i++)
				{
					if (this.layers[i].Dirty && this.layers[i] is WorldLayer_Terrain)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06006A68 RID: 27240 RVA: 0x00251B4C File Offset: 0x0024FD4C
		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x00251BC4 File Offset: 0x0024FDC4
		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		// Token: 0x06006A6A RID: 27242 RVA: 0x00251BF8 File Offset: 0x0024FDF8
		public void SetDirty<T>() where T : WorldLayer
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i] is T)
				{
					this.layers[i].SetDirty();
				}
			}
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x00251C40 File Offset: 0x0024FE40
		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x00251C74 File Offset: 0x0024FE74
		private IEnumerable RegenerateDirtyLayersNow_Async()
		{
			int num;
			for (int i = 0; i < this.layers.Count; i = num + 1)
			{
				if (this.layers[i].Dirty)
				{
					using (IEnumerator enumerator = this.layers[i].Regenerate().GetEnumerator())
					{
						for (;;)
						{
							try
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
							}
							catch (Exception arg)
							{
								Log.Error("Could not regenerate WorldLayer: " + arg, false);
								break;
							}
							yield return enumerator.Current;
						}
					}
					yield return null;
					IEnumerator enumerator = null;
				}
				num = i;
			}
			this.asynchronousRegenerationActive = false;
			yield break;
			yield break;
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x00251C84 File Offset: 0x0024FE84
		public void Notify_StaticWorldObjectPosChanged()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				WorldLayer_WorldObjects worldLayer_WorldObjects = this.layers[i] as WorldLayer_WorldObjects;
				if (worldLayer_WorldObjects != null)
				{
					worldLayer_WorldObjects.SetDirty();
				}
			}
		}

		// Token: 0x06006A6E RID: 27246 RVA: 0x00251CC2 File Offset: 0x0024FEC2
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x00251CD8 File Offset: 0x0024FED8
		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.", false);
				return;
			}
			if (this.ShouldRegenerateDirtyLayersInLongEvent)
			{
				this.asynchronousRegenerationActive = true;
				LongEventHandler.QueueLongEvent(this.RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null, false);
				return;
			}
			WorldRendererUtility.UpdateWorldShadersParams();
			for (int i = 0; i < this.layers.Count; i++)
			{
				try
				{
					this.layers[i].Render();
				}
				catch (Exception arg)
				{
					Log.Error("Error drawing WorldLayer: " + arg, false);
				}
			}
		}

		// Token: 0x06006A70 RID: 27248 RVA: 0x00251D70 File Offset: 0x0024FF70
		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.layers.Count;
			while (i < count)
			{
				WorldLayer_Terrain worldLayer_Terrain = this.layers[i] as WorldLayer_Terrain;
				if (worldLayer_Terrain != null)
				{
					return worldLayer_Terrain.GetTileIDFromRayHit(hit);
				}
				i++;
			}
			return -1;
		}

		// Token: 0x04004269 RID: 17001
		private List<WorldLayer> layers = new List<WorldLayer>();

		// Token: 0x0400426A RID: 17002
		public WorldRenderMode wantedMode;

		// Token: 0x0400426B RID: 17003
		private bool asynchronousRegenerationActive;
	}
}
