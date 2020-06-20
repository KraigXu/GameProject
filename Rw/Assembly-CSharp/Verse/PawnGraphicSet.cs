using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200021F RID: 543
	public class PawnGraphicSet
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0005692A File Offset: 0x00054B2A
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00056938 File Offset: 0x00054B38
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

		// Token: 0x170002F7 RID: 759
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

		// Token: 0x06000F33 RID: 3891 RVA: 0x00056AD0 File Offset: 0x00054CD0
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

		// Token: 0x06000F34 RID: 3892 RVA: 0x00056B6C File Offset: 0x00054D6C
		public Material HairMatAt(Rot4 facing)
		{
			Material baseMat = this.hairGraphic.MatAt(facing, null);
			if (this.pawn.IsInvisible())
			{
				baseMat = InvisibilityMatPool.GetInvisibleMat(baseMat);
			}
			return this.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00056BA7 File Offset: 0x00054DA7
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00056BDF File Offset: 0x00054DDF
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00056BE8 File Offset: 0x00054DE8
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

		// Token: 0x06000F38 RID: 3896 RVA: 0x00056EEE File Offset: 0x000550EE
		public void SetAllGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveAllGraphics();
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00056F00 File Offset: 0x00055100
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

		// Token: 0x06000F3A RID: 3898 RVA: 0x00056F8C File Offset: 0x0005518C
		public void SetApparelGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveApparelGraphics();
			}
		}

		// Token: 0x04000B4E RID: 2894
		public Pawn pawn;

		// Token: 0x04000B4F RID: 2895
		public Graphic nakedGraphic;

		// Token: 0x04000B50 RID: 2896
		public Graphic rottingGraphic;

		// Token: 0x04000B51 RID: 2897
		public Graphic dessicatedGraphic;

		// Token: 0x04000B52 RID: 2898
		public Graphic packGraphic;

		// Token: 0x04000B53 RID: 2899
		public DamageFlasher flasher;

		// Token: 0x04000B54 RID: 2900
		public Graphic headGraphic;

		// Token: 0x04000B55 RID: 2901
		public Graphic desiccatedHeadGraphic;

		// Token: 0x04000B56 RID: 2902
		public Graphic skullGraphic;

		// Token: 0x04000B57 RID: 2903
		public Graphic headStumpGraphic;

		// Token: 0x04000B58 RID: 2904
		public Graphic desiccatedHeadStumpGraphic;

		// Token: 0x04000B59 RID: 2905
		public Graphic hairGraphic;

		// Token: 0x04000B5A RID: 2906
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		// Token: 0x04000B5B RID: 2907
		private List<Material> cachedMatsBodyBase = new List<Material>();

		// Token: 0x04000B5C RID: 2908
		private int cachedMatsBodyBaseHash = -1;

		// Token: 0x04000B5D RID: 2909
		public static readonly Color RottingColor = new Color(0.34f, 0.32f, 0.3f);
	}
}
