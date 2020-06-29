using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class Graphic
	{
		
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

		
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0007B600 File Offset: 0x00079800
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0007B608 File Offset: 0x00079808
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0007B610 File Offset: 0x00079810
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0007B617 File Offset: 0x00079817
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0007B61F File Offset: 0x0007981F
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0007B634 File Offset: 0x00079834
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928, false);
		}

		
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

		
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		
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

		
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		
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

		
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300, false);
			return BaseContent.BadGraphic;
		}

		
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, this.Shader, newDrawSize, this.color, this.colorTwo);
		}

		
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

		
		protected Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		
		public GraphicData data;

		
		public string path;

		
		public Color color = Color.white;

		
		public Color colorTwo = Color.white;

		
		public Vector2 drawSize = Vector2.one;

		
		private Graphic_Shadow cachedShadowGraphicInt;

		
		private Graphic cachedShadowlessGraphicInt;
	}
}
