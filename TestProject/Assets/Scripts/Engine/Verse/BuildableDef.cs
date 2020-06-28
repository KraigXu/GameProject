using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200007B RID: 123
	public abstract class BuildableDef : Def
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00004A30 File Offset: 0x00002C30
		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0001753E File Offset: 0x0001573E
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001754E File Offset: 0x0001574E
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00017559 File Offset: 0x00015759
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00017570 File Offset: 0x00015770
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001757D File Offset: 0x0001577D
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00017590 File Offset: 0x00015790
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00017610 File Offset: 0x00015810
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

		// Token: 0x06000496 RID: 1174 RVA: 0x00017654 File Offset: 0x00015854
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

		// Token: 0x06000497 RID: 1175 RVA: 0x00017698 File Offset: 0x00015898
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

		// Token: 0x06000498 RID: 1176 RVA: 0x000176B4 File Offset: 0x000158B4
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

		// Token: 0x06000499 RID: 1177 RVA: 0x00017744 File Offset: 0x00015944
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

		// Token: 0x0600049A RID: 1178 RVA: 0x000177A8 File Offset: 0x000159A8
		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000177B0 File Offset: 0x000159B0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.useStuffTerrainAffordance && !this.MadeFromStuff)
			{
				yield return "useStuffTerrainAffordance is true but it's not made from stuff";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000177C0 File Offset: 0x000159C0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__1(req))
			{
				yield return statDrawEntry;
			}
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

		// Token: 0x0600049D RID: 1181 RVA: 0x00016DBF File Offset: 0x00014FBF
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00016DC7 File Offset: 0x00014FC7
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x040001B2 RID: 434
		public List<StatModifier> statBases;

		// Token: 0x040001B3 RID: 435
		public Traversability passability;

		// Token: 0x040001B4 RID: 436
		public int pathCost;

		// Token: 0x040001B5 RID: 437
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x040001B6 RID: 438
		public float fertility = -1f;

		// Token: 0x040001B7 RID: 439
		public List<ThingDefCountClass> costList;

		// Token: 0x040001B8 RID: 440
		public int costStuffCount;

		// Token: 0x040001B9 RID: 441
		public List<StuffCategoryDef> stuffCategories;

		// Token: 0x040001BA RID: 442
		public int placingDraggableDimensions;

		// Token: 0x040001BB RID: 443
		public bool clearBuildingArea = true;

		// Token: 0x040001BC RID: 444
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x040001BD RID: 445
		public float resourcesFractionWhenDeconstructed = 0.75f;

		// Token: 0x040001BE RID: 446
		public bool useStuffTerrainAffordance;

		// Token: 0x040001BF RID: 447
		public TerrainAffordanceDef terrainAffordanceNeeded;

		// Token: 0x040001C0 RID: 448
		public List<ThingDef> buildingPrerequisites;

		// Token: 0x040001C1 RID: 449
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040001C2 RID: 450
		public int constructionSkillPrerequisite;

		// Token: 0x040001C3 RID: 451
		public int artisticSkillPrerequisite;

		// Token: 0x040001C4 RID: 452
		public TechLevel minTechLevelToBuild;

		// Token: 0x040001C5 RID: 453
		public TechLevel maxTechLevelToBuild;

		// Token: 0x040001C6 RID: 454
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x040001C7 RID: 455
		public EffecterDef repairEffect;

		// Token: 0x040001C8 RID: 456
		public EffecterDef constructEffect;

		// Token: 0x040001C9 RID: 457
		public List<ColorForStuff> colorPerStuff;

		// Token: 0x040001CA RID: 458
		public bool menuHidden;

		// Token: 0x040001CB RID: 459
		public float specialDisplayRadius;

		// Token: 0x040001CC RID: 460
		public List<Type> placeWorkers;

		// Token: 0x040001CD RID: 461
		public DesignationCategoryDef designationCategory;

		// Token: 0x040001CE RID: 462
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x040001CF RID: 463
		public KeyBindingDef designationHotKey;

		// Token: 0x040001D0 RID: 464
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x040001D1 RID: 465
		public Vector2 uiIconOffset;

		// Token: 0x040001D2 RID: 466
		public Color uiIconColor = Color.white;

		// Token: 0x040001D3 RID: 467
		public int uiIconForStackCount = -1;

		// Token: 0x040001D4 RID: 468
		[Unsaved(false)]
		public ThingDef blueprintDef;

		// Token: 0x040001D5 RID: 469
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		// Token: 0x040001D6 RID: 470
		[Unsaved(false)]
		public ThingDef frameDef;

		// Token: 0x040001D7 RID: 471
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		// Token: 0x040001D8 RID: 472
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x040001D9 RID: 473
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x040001DA RID: 474
		[Unsaved(false)]
		public float uiIconAngle;
	}
}
