using System;
using UnityEngine;

namespace Verse
{
	
	public class Graphic_Single : Graphic
	{
		
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0007D300 File Offset: 0x0007B500
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x0007D300 File Offset: 0x0007B500
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x0007D300 File Offset: 0x0007B500
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x0007D300 File Offset: 0x0007B500
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x0007D300 File Offset: 0x0007B500
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x0007D308 File Offset: 0x0007B508
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			MaterialRequest req2 = default(MaterialRequest);
			req2.mainTex = ContentFinder<Texture2D>.Get(req.path, true);
			req2.shader = req.shader;
			req2.color = this.color;
			req2.colorTwo = this.colorTwo;
			req2.renderQueue = req.renderQueue;
			req2.shaderParameters = req.shaderParameters;
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(req.path + Graphic_Single.MaskSuffix, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Single(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		
		protected Material mat;

		
		public static readonly string MaskSuffix = "_m";
	}
}
