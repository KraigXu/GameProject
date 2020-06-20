using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000091 RID: 145
	public class GraphicData
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001887D File Offset: 0x00016A7D
		public bool Linked
		{
			get
			{
				return this.linkType > LinkDrawerType.None;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00018888 File Offset: 0x00016A88
		public Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.Init();
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000188A0 File Offset: 0x00016AA0
		public void CopyFrom(GraphicData other)
		{
			this.texPath = other.texPath;
			this.graphicClass = other.graphicClass;
			this.shaderType = other.shaderType;
			this.color = other.color;
			this.colorTwo = other.colorTwo;
			this.drawSize = other.drawSize;
			this.drawOffset = other.drawOffset;
			this.drawOffsetNorth = other.drawOffsetNorth;
			this.drawOffsetEast = other.drawOffsetEast;
			this.drawOffsetSouth = other.drawOffsetSouth;
			this.drawOffsetWest = other.drawOffsetSouth;
			this.onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
			this.drawRotated = other.drawRotated;
			this.allowFlip = other.allowFlip;
			this.flipExtraRotation = other.flipExtraRotation;
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
			this.cachedGraphic = null;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00018998 File Offset: 0x00016B98
		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
				return;
			}
			ShaderTypeDef cutout = this.shaderType;
			if (cutout == null)
			{
				cutout = ShaderTypeDefOf.Cutout;
			}
			Shader shader = cutout.Shader;
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters);
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00018A46 File Offset: 0x00016C46
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00018A5C File Offset: 0x00016C5C
		public Vector3 DrawOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.drawOffsetNorth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.drawOffsetEast;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.drawOffsetSouth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector = this.drawOffsetWest;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.drawOffset;
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00018B0C File Offset: 0x00016D0C
		public Graphic GraphicColoredFor(Thing t)
		{
			if (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00018B72 File Offset: 0x00016D72
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.graphicClass == null)
			{
				yield return "graphicClass is null";
			}
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is null or empty";
			}
			if (thingDef != null)
			{
				if (thingDef.drawerType == DrawerType.RealtimeOnly && this.Linked)
				{
					yield return "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
				}
				if (!thingDef.rotatable && (this.drawOffsetNorth != null || this.drawOffsetEast != null || this.drawOffsetSouth != null || this.drawOffsetWest != null))
				{
					yield return "not rotatable but has rotational draw offset(s).";
				}
			}
			if ((this.shaderType == ShaderTypeDefOf.Cutout || this.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
			{
				yield return "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
			}
			yield break;
		}

		// Token: 0x04000261 RID: 609
		[NoTranslate]
		public string texPath;

		// Token: 0x04000262 RID: 610
		public Type graphicClass;

		// Token: 0x04000263 RID: 611
		public ShaderTypeDef shaderType;

		// Token: 0x04000264 RID: 612
		public List<ShaderParameter> shaderParameters;

		// Token: 0x04000265 RID: 613
		public Color color = Color.white;

		// Token: 0x04000266 RID: 614
		public Color colorTwo = Color.white;

		// Token: 0x04000267 RID: 615
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04000268 RID: 616
		public Vector3 drawOffset = Vector3.zero;

		// Token: 0x04000269 RID: 617
		public Vector3? drawOffsetNorth;

		// Token: 0x0400026A RID: 618
		public Vector3? drawOffsetEast;

		// Token: 0x0400026B RID: 619
		public Vector3? drawOffsetSouth;

		// Token: 0x0400026C RID: 620
		public Vector3? drawOffsetWest;

		// Token: 0x0400026D RID: 621
		public float onGroundRandomRotateAngle;

		// Token: 0x0400026E RID: 622
		public bool drawRotated = true;

		// Token: 0x0400026F RID: 623
		public bool allowFlip = true;

		// Token: 0x04000270 RID: 624
		public float flipExtraRotation;

		// Token: 0x04000271 RID: 625
		public ShadowData shadowData;

		// Token: 0x04000272 RID: 626
		public DamageGraphicData damageData;

		// Token: 0x04000273 RID: 627
		public LinkDrawerType linkType;

		// Token: 0x04000274 RID: 628
		public LinkFlags linkFlags;

		// Token: 0x04000275 RID: 629
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
