using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E4 RID: 740
	public class Graphic
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0007B59C File Offset: 0x0007979C
		public Shader Shader
		{
			get
			{
				Material matSingle = this.MatSingle;
				if (matSingle != null)
				{
					return matSingle.shader;
				}
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0007B5C5 File Offset: 0x000797C5
		public Graphic_Shadow ShadowGraphic
		{
			get
			{
				if (this.cachedShadowGraphicInt == null && this.data != null && this.data.shadowData != null)
				{
					this.cachedShadowGraphicInt = new Graphic_Shadow(this.data.shadowData);
				}
				return this.cachedShadowGraphicInt;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0007B600 File Offset: 0x00079800
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0007B608 File Offset: 0x00079808
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0007B610 File Offset: 0x00079810
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0007B61F File Offset: 0x0007981F
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0007B634 File Offset: 0x00079834
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0007B64B File Offset: 0x0007984B
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928, false);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x0007B670 File Offset: 0x00079870
		public virtual Material MatAt(Rot4 rot, Thing thing = null)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.MatNorth;
			case 1:
				return this.MatEast;
			case 2:
				return this.MatSouth;
			case 3:
				return this.MatWest;
			default:
				return BaseContent.BadMat;
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0007B6C0 File Offset: 0x000798C0
		public virtual Mesh MeshAt(Rot4 rot)
		{
			Vector2 vector = this.drawSize;
			if (rot.IsHorizontal && !this.ShouldDrawRotated)
			{
				vector = vector.Rotated();
			}
			if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
			{
				return MeshPool.GridPlaneFlip(vector);
			}
			return MeshPool.GridPlane(vector);
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0007B723 File Offset: 0x00079923
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0007B73F File Offset: 0x0007993F
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0007B752 File Offset: 0x00079952
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0007B760 File Offset: 0x00079960
		public virtual void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			Quaternion quaternion = this.QuatFromRot(rot);
			if (extraRotation != 0f)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			loc += this.DrawOffset(rot);
			Material mat = this.MatAt(rot, thing);
			this.DrawMeshInt(mesh, loc, quaternion, mat);
			if (this.ShadowGraphic != null)
			{
				this.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0007B7DB File Offset: 0x000799DB
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0007B7E8 File Offset: 0x000799E8
		public virtual void Print(SectionLayer layer, Thing thing)
		{
			Vector2 size;
			bool flag;
			if (this.ShouldDrawRotated)
			{
				size = this.drawSize;
				flag = false;
			}
			else
			{
				if (!thing.Rotation.IsHorizontal)
				{
					size = this.drawSize;
				}
				else
				{
					size = this.drawSize.Rotated();
				}
				flag = ((thing.Rotation == Rot4.West && this.WestFlipped) || (thing.Rotation == Rot4.East && this.EastFlipped));
			}
			float num = this.AngleFromRot(thing.Rotation);
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Vector3 center = thing.TrueCenter() + this.DrawOffset(thing.Rotation);
			Printer_Plane.PrintPlane(layer, center, size, this.MatAt(thing.Rotation, thing), num, flag, null, null, 0.01f, 0f);
			if (this.ShadowGraphic != null && thing != null)
			{
				this.ShadowGraphic.Print(layer, thing);
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0007B8E0 File Offset: 0x00079AE0
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300, false);
			return BaseContent.BadGraphic;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0007B907 File Offset: 0x00079B07
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, this.Shader, newDrawSize, this.color, this.colorTwo);
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0007B930 File Offset: 0x00079B30
		public virtual Graphic GetShadowlessGraphic()
		{
			if (this.data == null || this.data.shadowData == null)
			{
				return this;
			}
			if (this.cachedShadowlessGraphicInt == null)
			{
				GraphicData graphicData = new GraphicData();
				graphicData.CopyFrom(this.data);
				graphicData.shadowData = null;
				this.cachedShadowlessGraphicInt = graphicData.Graphic;
			}
			return this.cachedShadowlessGraphicInt;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0007B988 File Offset: 0x00079B88
		protected float AngleFromRot(Rot4 rot)
		{
			if (this.ShouldDrawRotated)
			{
				float num = rot.AsAngle;
				num += this.DrawRotatedExtraAngleOffset;
				if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
				{
					num += 180f;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0007B9E8 File Offset: 0x00079BE8
		protected Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x04000DE5 RID: 3557
		public GraphicData data;

		// Token: 0x04000DE6 RID: 3558
		public string path;

		// Token: 0x04000DE7 RID: 3559
		public Color color = Color.white;

		// Token: 0x04000DE8 RID: 3560
		public Color colorTwo = Color.white;

		// Token: 0x04000DE9 RID: 3561
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04000DEA RID: 3562
		private Graphic_Shadow cachedShadowGraphicInt;

		// Token: 0x04000DEB RID: 3563
		private Graphic cachedShadowlessGraphicInt;
	}
}
