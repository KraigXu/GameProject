using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class TerrainDef : BuildableDef
	{
		
		
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		
		
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		
		
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		
		
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		
		
		public bool IsFine
		{
			get
			{
				return this.HasTag("FineFloor");
			}
		}

		
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

		
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
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

		
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{


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

		
		[NoTranslate]
		public string texturePath;

		
		public TerrainDef.TerrainEdgeType edgeType;

		
		[NoTranslate]
		public string waterDepthShader;

		
		public List<ShaderParameter> waterDepthShaderParameters;

		
		public int renderPrecedence;

		
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		
		public bool layerable;

		
		[NoTranslate]
		public string scatterType;

		
		public bool takeFootprints;

		
		public bool takeSplashes;

		
		public bool avoidWander;

		
		public bool changeable = true;

		
		public TerrainDef smoothedTerrain;

		
		public bool holdSnow = true;

		
		public bool extinguishesFire;

		
		public Color color = Color.white;

		
		public TerrainDef driesTo;

		
		[NoTranslate]
		public List<string> tags;

		
		public TerrainDef burnedDef;

		
		public List<Tool> tools;

		
		public float extraDeteriorationFactor;

		
		public float destroyOnBombDamageThreshold = -1f;

		
		public bool destroyBuildingsOnDestroyed;

		
		public ThoughtDef traversedThought;

		
		public int extraDraftedPerceivedPathCost;

		
		public int extraNonDraftedPerceivedPathCost;

		
		public EffecterDef destroyEffect;

		
		public EffecterDef destroyEffectWater;

		
		public ThingDef generatedFilth;

		
		public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

		
		[Unsaved(false)]
		public Material waterDepthMaterial;

		
		public enum TerrainEdgeType : byte
		{
			
			Hard,
			
			Fade,
			
			FadeRough,
			
			Water
		}
	}
}
