using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018B RID: 395
	public class Section
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0003D7C4 File Offset: 0x0003B9C4
		public CellRect CellRect
		{
			get
			{
				if (!this.foundRect)
				{
					this.calculatedRect = new CellRect(this.botLeft.x, this.botLeft.z, 17, 17);
					this.calculatedRect.ClipInsideMap(this.map);
					this.foundRect = true;
				}
				return this.calculatedRect;
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0003D820 File Offset: 0x0003BA20
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				this.layers.Add((SectionLayer)Activator.CreateInstance(type, new object[]
				{
					this
				}));
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003D8B8 File Offset: 0x0003BAB8
		public void DrawSection(bool drawSunShadowsOnly)
		{
			int count = this.layers.Count;
			for (int i = 0; i < count; i++)
			{
				if (!drawSunShadowsOnly || this.layers[i] is SectionLayer_SunShadows)
				{
					this.layers[i].DrawLayer();
				}
			}
			if (!drawSunShadowsOnly && DebugViewSettings.drawSectionEdges)
			{
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(0f, 0f, 17f));
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(17f, 0f, 0f));
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0003D978 File Offset: 0x0003BB78
		public void RegenerateAllLayers()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					try
					{
						this.layers[i].Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							this.layers[i].ToStringSafe<SectionLayer>(),
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0003DA10 File Offset: 0x0003BC10
		public void RegenerateLayers(MapMeshFlag changeType)
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				SectionLayer sectionLayer = this.layers[i];
				if ((sectionLayer.relevantChangeTypes & changeType) != MapMeshFlag.None)
				{
					try
					{
						sectionLayer.Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							sectionLayer.ToStringSafe<SectionLayer>(),
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0003DA94 File Offset: 0x0003BC94
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}

		// Token: 0x04000929 RID: 2345
		public IntVec3 botLeft;

		// Token: 0x0400092A RID: 2346
		public Map map;

		// Token: 0x0400092B RID: 2347
		public MapMeshFlag dirtyFlags;

		// Token: 0x0400092C RID: 2348
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x0400092D RID: 2349
		private bool foundRect;

		// Token: 0x0400092E RID: 2350
		private CellRect calculatedRect;

		// Token: 0x0400092F RID: 2351
		public const int Size = 17;
	}
}
