using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class PawnGraphicSet
	{
		
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0005692A File Offset: 0x00054B2A
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		
		public List<Material> MatsBodyBaseAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh)
		{
			int num = facing.AsInt + 1000 * (int)bodyCondition;
			if (num != this.cachedMatsBodyBaseHash)
			{
				this.cachedMatsBodyBase.Clear();
				this.cachedMatsBodyBaseHash = num;
				if (bodyCondition == RotDrawMode.Fresh)
				{
					this.cachedMatsBodyBase.Add(this.nakedGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Rotting || this.dessicatedGraphic == null)
				{
					this.cachedMatsBodyBase.Add(this.rottingGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Dessicated)
				{
					this.cachedMatsBodyBase.Add(this.dessicatedGraphic.MatAt(facing, null));
				}
				for (int i = 0; i < this.apparelGraphics.Count; i++)
				{
					if (this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Shell && this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Overhead)
					{
						this.cachedMatsBodyBase.Add(this.apparelGraphics[i].graphic.MatAt(facing, null));
					}
				}
			}
			return this.cachedMatsBodyBase;
		}

		
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x00056A64 File Offset: 0x00054C64
		public GraphicMeshSet HairMeshSet
		{
			get
			{
				if (this.pawn.story.crownType == CrownType.Average)
				{
					return MeshPool.humanlikeHairSetAverage;
				}
				if (this.pawn.story.crownType == CrownType.Narrow)
				{
					return MeshPool.humanlikeHairSetNarrow;
				}
				Log.Error("Unknown crown type: " + this.pawn.story.crownType, false);
				return MeshPool.humanlikeHairSetAverage;
			}
		}

		
		public Material HeadMatAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false)
		{
			Material material = null;
			if (bodyCondition == RotDrawMode.Fresh)
			{
				if (stump)
				{
					material = this.headStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.headGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Rotting)
			{
				if (stump)
				{
					material = this.desiccatedHeadStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.desiccatedHeadGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Dessicated && !stump)
			{
				material = this.skullGraphic.MatAt(facing, null);
			}
			if (material != null)
			{
				if (this.pawn.IsInvisible())
				{
					material = InvisibilityMatPool.GetInvisibleMat(material);
				}
				material = this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		
		public Material HairMatAt(Rot4 facing)
		{
			Material baseMat = this.hairGraphic.MatAt(facing, null);
			if (this.pawn.IsInvisible())
			{
				baseMat = InvisibilityMatPool.GetInvisibleMat(baseMat);
			}
			return this.flasher.GetDamagedMat(baseMat);
		}

		
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		
		public void ResolveAllGraphics()
		{
			this.ClearCache();
			if (this.pawn.RaceProps.Humanlike)
			{
				this.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, this.pawn.story.SkinColor);
				this.rottingGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, PawnGraphicSet.RottingColor);
				this.dessicatedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyDessicatedGraphicPath, ShaderDatabase.Cutout);
				this.headGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, this.pawn.story.SkinColor);
				this.desiccatedHeadGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, PawnGraphicSet.RottingColor);
				this.skullGraphic = GraphicDatabaseHeadRecords.GetSkull();
				this.headStumpGraphic = GraphicDatabaseHeadRecords.GetStump(this.pawn.story.SkinColor);
				this.desiccatedHeadStumpGraphic = GraphicDatabaseHeadRecords.GetStump(PawnGraphicSet.RottingColor);
				this.hairGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.hairDef.texPath, ShaderDatabase.Cutout, Vector2.one, this.pawn.story.hairColor);
				this.ResolveApparelGraphics();
				return;
			}
			PawnKindLifeStage curKindLifeStage = this.pawn.ageTracker.CurKindLifeStage;
			if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
			{
				this.nakedGraphic = curKindLifeStage.bodyGraphicData.Graphic;
			}
			else
			{
				this.nakedGraphic = curKindLifeStage.femaleGraphicData.Graphic;
			}
			if (this.pawn.RaceProps.packAnimal)
			{
				this.packGraphic = GraphicDatabase.Get<Graphic_Multi>(this.nakedGraphic.path + "Pack", ShaderDatabase.Cutout, this.nakedGraphic.drawSize, Color.white);
			}
			this.rottingGraphic = this.nakedGraphic.GetColoredVersion(ShaderDatabase.CutoutSkin, PawnGraphicSet.RottingColor, PawnGraphicSet.RottingColor);
			if (curKindLifeStage.dessicatedBodyGraphicData != null)
			{
				if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
				{
					this.dessicatedGraphic = curKindLifeStage.dessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
				else
				{
					this.dessicatedGraphic = curKindLifeStage.femaleDessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
			}
			if (!this.pawn.kindDef.alternateGraphics.NullOrEmpty<AlternateGraphic>())
			{
				Rand.PushState(this.pawn.thingIDNumber ^ 46101);
				if (Rand.Value <= this.pawn.kindDef.alternateGraphicChance)
				{
					this.nakedGraphic = this.pawn.kindDef.alternateGraphics.RandomElementByWeight((AlternateGraphic x) => x.Weight).GetGraphic(this.nakedGraphic);
				}
				Rand.PopState();
			}
		}

		
		public void SetAllGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveAllGraphics();
			}
		}

		
		public void ResolveApparelGraphics()
		{
			this.ClearCache();
			this.apparelGraphics.Clear();
			using (List<Apparel>.Enumerator enumerator = this.pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApparelGraphicRecord item;
					if (ApparelGraphicRecordGetter.TryGetGraphicApparel(enumerator.Current, this.pawn.story.bodyType, out item))
					{
						this.apparelGraphics.Add(item);
					}
				}
			}
		}

		
		public void SetApparelGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveApparelGraphics();
			}
		}

		
		public Pawn pawn;

		
		public Graphic nakedGraphic;

		
		public Graphic rottingGraphic;

		
		public Graphic dessicatedGraphic;

		
		public Graphic packGraphic;

		
		public DamageFlasher flasher;

		
		public Graphic headGraphic;

		
		public Graphic desiccatedHeadGraphic;

		
		public Graphic skullGraphic;

		
		public Graphic headStumpGraphic;

		
		public Graphic desiccatedHeadStumpGraphic;

		
		public Graphic hairGraphic;

		
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		
		private List<Material> cachedMatsBodyBase = new List<Material>();

		
		private int cachedMatsBodyBaseHash = -1;

		
		public static readonly Color RottingColor = new Color(0.34f, 0.32f, 0.3f);
	}
}
