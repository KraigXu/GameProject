using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public abstract class BuildableDef : Def
	{
		
		
		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		
		
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		
		
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		
		
		public Material DrawMatSingle
		{
			get
			{
				if (this.graphic == null)
				{
					return null;
				}
				return this.graphic.MatSingle;
			}
		}

		
		
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		
		
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		
		
		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				if (this.placeWorkers == null)
				{
					return null;
				}
				this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
				foreach (Type type in this.placeWorkers)
				{
					this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
				}
				return this.placeWorkersInstantiatedInt;
			}
		}

		
		
		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		
		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			if (this.PlaceWorkers == null)
			{
				return false;
			}
			for (int i = 0; i < this.PlaceWorkers.Count; i++)
			{
				if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
				{
					return true;
				}
			}
			return false;
		}

		
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
					return;
				}
				this.ResolveIcon();
			});
		}

		
		protected virtual void ResolveIcon()
		{
			if (this.graphic != null && this.graphic != BaseContent.BadGraphic)
			{
				Graphic outerGraphic = this.graphic;
				if (this.uiIconForStackCount >= 1 && this is ThingDef)
				{
					Graphic_StackCount graphic_StackCount = this.graphic as Graphic_StackCount;
					if (graphic_StackCount != null)
					{
						outerGraphic = graphic_StackCount.SubGraphicForStackCount(this.uiIconForStackCount, (ThingDef)this);
					}
				}
				Material material = outerGraphic.ExtractInnerGraphicFor(null).MatAt(this.defaultPlacingRot, null);
				this.uiIcon = (Texture2D)material.mainTexture;
				this.uiIconColor = material.color;
			}
		}

		
		public Color GetColorForStuff(ThingDef stuff)
		{
			if (this.colorPerStuff.NullOrEmpty<ColorForStuff>())
			{
				return stuff.stuffProps.color;
			}
			for (int i = 0; i < this.colorPerStuff.Count; i++)
			{
				ColorForStuff colorForStuff = this.colorPerStuff[i];
				if (colorForStuff.Stuff == stuff)
				{
					return colorForStuff.Color;
				}
			}
			return stuff.stuffProps.color;
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.useStuffTerrainAffordance && !this.MadeFromStuff)
			{
				yield return "useStuffTerrainAffordance is true but it's not made from stuff";
			}
			yield break;
			yield break;
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{


			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.BuildableByPlayer)
			{
				IEnumerable<TerrainAffordanceDef> enumerable = Enumerable.Empty<TerrainAffordanceDef>();
				if (this.PlaceWorkers != null)
				{
					enumerable = enumerable.Concat(this.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
				}
				TerrainAffordanceDef terrainAffordanceNeed = this.GetTerrainAffordanceNeed(req.StuffDef);
				if (terrainAffordanceNeed != null)
				{
					enumerable = enumerable.Concat(terrainAffordanceNeed);
				}
				string[] array = (from ta in enumerable.Distinct<TerrainAffordanceDef>()
				orderby ta.order
				select ta.label).ToArray<string>();
				if (array.Length != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), array.ToCommaList(false).CapitalizeFirst(), "Stat_Thing_TerrainRequirement_Desc".Translate(), 1101, null, null, false);
				}
			}
			yield break;
			yield break;
		}

		
		public override string ToString()
		{
			return this.defName;
		}

		
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		
		public List<StatModifier> statBases;

		
		public Traversability passability;

		
		public int pathCost;

		
		public bool pathCostIgnoreRepeat = true;

		
		public float fertility = -1f;

		
		public List<ThingDefCountClass> costList;

		
		public int costStuffCount;

		
		public List<StuffCategoryDef> stuffCategories;

		
		public int placingDraggableDimensions;

		
		public bool clearBuildingArea = true;

		
		public Rot4 defaultPlacingRot = Rot4.North;

		
		public float resourcesFractionWhenDeconstructed = 0.75f;

		
		public bool useStuffTerrainAffordance;

		
		public TerrainAffordanceDef terrainAffordanceNeeded;

		
		public List<ThingDef> buildingPrerequisites;

		
		public List<ResearchProjectDef> researchPrerequisites;

		
		public int constructionSkillPrerequisite;

		
		public int artisticSkillPrerequisite;

		
		public TechLevel minTechLevelToBuild;

		
		public TechLevel maxTechLevelToBuild;

		
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		
		public EffecterDef repairEffect;

		
		public EffecterDef constructEffect;

		
		public List<ColorForStuff> colorPerStuff;

		
		public bool menuHidden;

		
		public float specialDisplayRadius;

		
		public List<Type> placeWorkers;

		
		public DesignationCategoryDef designationCategory;

		
		public DesignatorDropdownGroupDef designatorDropdown;

		
		public KeyBindingDef designationHotKey;

		
		[NoTranslate]
		public string uiIconPath;

		
		public Vector2 uiIconOffset;

		
		public Color uiIconColor = Color.white;

		
		public int uiIconForStackCount = -1;

		
		[Unsaved(false)]
		public ThingDef blueprintDef;

		
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		
		[Unsaved(false)]
		public ThingDef frameDef;

		
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		
		[Unsaved(false)]
		public float uiIconAngle;
	}
}
