using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000EF RID: 239
	public class TerrainDef : BuildableDef
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001E3BF File Offset: 0x0001C5BF
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001E3C7 File Offset: 0x0001C5C7
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0001E3E3 File Offset: 0x0001C5E3
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001E3FD File Offset: 0x0001C5FD
		public bool IsFine
		{
			get
			{
				return this.HasTag("FineFloor");
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001E40C File Offset: 0x0001C60C
		public override void PostLoad()
		{
			this.placingDraggableDimensions = 2;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Shader shader = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					shader = ShaderDatabase.TerrainHard;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					shader = ShaderDatabase.TerrainFade;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					shader = ShaderDatabase.TerrainFadeRough;
					break;
				case TerrainDef.TerrainEdgeType.Water:
					shader = ShaderDatabase.TerrainWater;
					break;
				}
				this.graphic = GraphicDatabase.Get<Graphic_Terrain>(this.texturePath, shader, Vector2.one, this.color, 2000 + this.renderPrecedence);
				if (shader == ShaderDatabase.TerrainFadeRough || shader == ShaderDatabase.TerrainWater)
				{
					this.graphic.MatSingle.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
				}
				if (!this.waterDepthShader.NullOrEmpty())
				{
					this.waterDepthMaterial = MaterialAllocator.Create(ShaderDatabase.LoadShader(this.waterDepthShader));
					this.waterDepthMaterial.renderQueue = 2000 + this.renderPrecedence;
					this.waterDepthMaterial.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
					if (this.waterDepthShaderParameters != null)
					{
						for (int j = 0; j < this.waterDepthShaderParameters.Count; j++)
						{
							this.waterDepthShaderParameters[j].Apply(this.waterDepthMaterial);
						}
					}
				}
			});
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			base.PostLoad();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001E46D File Offset: 0x0001C66D
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001E481 File Offset: 0x0001C681
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.texturePath.NullOrEmpty())
			{
				yield return "missing texturePath";
			}
			if (this.fertility < 0f)
			{
				yield return "Terrain Def " + this + " has no fertility value set.";
			}
			if (this.renderPrecedence > 400)
			{
				yield return "Render order " + this.renderPrecedence + " is out of range (must be < 400)";
			}
			if (this.generatedFilth != null && (this.filthAcceptanceMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None)
			{
				yield return this.defName + " makes terrain filth and also accepts it.";
			}
			if (this.Flammable() && this.burnedDef == null && !this.layerable)
			{
				yield return "flammable but burnedDef is null and not layerable";
			}
			if (this.burnedDef != null && this.burnedDef.Flammable())
			{
				yield return "burnedDef is flammable";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001E491 File Offset: 0x0001C691
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001E49A File Offset: 0x0001C69A
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001E4B2 File Offset: 0x0001C6B2
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__1(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			string[] array = (from ta in this.affordances.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (array.Length != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Supports".Translate(), array.ToCommaList(false).CapitalizeFirst(), "Stat_Thing_Terrain_Supports_Desc".Translate(), 2000, null, null, false);
			}
			if (this.IsFine)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Terrain_Fine_Name".Translate(), "Stat_Thing_Terrain_Fine_Value".Translate(), "Stat_Thing_Terrain_Fine_Desc".Translate(), 2000, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04000589 RID: 1417
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400058A RID: 1418
		public TerrainDef.TerrainEdgeType edgeType;

		// Token: 0x0400058B RID: 1419
		[NoTranslate]
		public string waterDepthShader;

		// Token: 0x0400058C RID: 1420
		public List<ShaderParameter> waterDepthShaderParameters;

		// Token: 0x0400058D RID: 1421
		public int renderPrecedence;

		// Token: 0x0400058E RID: 1422
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x0400058F RID: 1423
		public bool layerable;

		// Token: 0x04000590 RID: 1424
		[NoTranslate]
		public string scatterType;

		// Token: 0x04000591 RID: 1425
		public bool takeFootprints;

		// Token: 0x04000592 RID: 1426
		public bool takeSplashes;

		// Token: 0x04000593 RID: 1427
		public bool avoidWander;

		// Token: 0x04000594 RID: 1428
		public bool changeable = true;

		// Token: 0x04000595 RID: 1429
		public TerrainDef smoothedTerrain;

		// Token: 0x04000596 RID: 1430
		public bool holdSnow = true;

		// Token: 0x04000597 RID: 1431
		public bool extinguishesFire;

		// Token: 0x04000598 RID: 1432
		public Color color = Color.white;

		// Token: 0x04000599 RID: 1433
		public TerrainDef driesTo;

		// Token: 0x0400059A RID: 1434
		[NoTranslate]
		public List<string> tags;

		// Token: 0x0400059B RID: 1435
		public TerrainDef burnedDef;

		// Token: 0x0400059C RID: 1436
		public List<Tool> tools;

		// Token: 0x0400059D RID: 1437
		public float extraDeteriorationFactor;

		// Token: 0x0400059E RID: 1438
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x0400059F RID: 1439
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x040005A0 RID: 1440
		public ThoughtDef traversedThought;

		// Token: 0x040005A1 RID: 1441
		public int extraDraftedPerceivedPathCost;

		// Token: 0x040005A2 RID: 1442
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x040005A3 RID: 1443
		public EffecterDef destroyEffect;

		// Token: 0x040005A4 RID: 1444
		public EffecterDef destroyEffectWater;

		// Token: 0x040005A5 RID: 1445
		public ThingDef generatedFilth;

		// Token: 0x040005A6 RID: 1446
		public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

		// Token: 0x040005A7 RID: 1447
		[Unsaved(false)]
		public Material waterDepthMaterial;

		// Token: 0x0200135E RID: 4958
		public enum TerrainEdgeType : byte
		{
			// Token: 0x04004980 RID: 18816
			Hard,
			// Token: 0x04004981 RID: 18817
			Fade,
			// Token: 0x04004982 RID: 18818
			FadeRough,
			// Token: 0x04004983 RID: 18819
			Water
		}
	}
}
