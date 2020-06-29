﻿using System;
using UnityEngine;

namespace Verse
{
	
	public class Graphic_Multi : Graphic
	{
		
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x0007CB48 File Offset: 0x0007AD48
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x0007CB50 File Offset: 0x0007AD50
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x0007CB58 File Offset: 0x0007AD58
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x0007CB62 File Offset: 0x0007AD62
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0007CB6C File Offset: 0x0007AD6C
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x0007CB76 File Offset: 0x0007AD76
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0007CB80 File Offset: 0x0007AD80
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x0007CB88 File Offset: 0x0007AD88
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x0007CB90 File Offset: 0x0007AD90
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x0007CBCF File Offset: 0x0007ADCF
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
			array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
			array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
			if (array[0] == null)
			{
				if (array[2] != null)
				{
					array[0] = array[2];
					this.drawRotatedExtraAngleOffset = 180f;
				}
				else if (array[1] != null)
				{
					array[0] = array[1];
					this.drawRotatedExtraAngleOffset = -90f;
				}
				else if (array[3] != null)
				{
					array[0] = array[3];
					this.drawRotatedExtraAngleOffset = 90f;
				}
				else
				{
					array[0] = ContentFinder<Texture2D>.Get(req.path, false);
				}
			}
			if (array[0] == null)
			{
				Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>(), false);
				return;
			}
			if (array[2] == null)
			{
				array[2] = array[0];
			}
			if (array[1] == null)
			{
				if (array[3] != null)
				{
					array[1] = array[3];
					this.eastFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[1] = array[0];
				}
			}
			if (array[3] == null)
			{
				if (array[1] != null)
				{
					array[3] = array[1];
					this.westFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[3] = array[0];
				}
			}
			Texture2D[] array2 = new Texture2D[this.mats.Length];
			if (req.shader.SupportsMaskTex())
			{
				array2[0] = ContentFinder<Texture2D>.Get(req.path + "_northm", false);
				array2[1] = ContentFinder<Texture2D>.Get(req.path + "_eastm", false);
				array2[2] = ContentFinder<Texture2D>.Get(req.path + "_southm", false);
				array2[3] = ContentFinder<Texture2D>.Get(req.path + "_westm", false);
				if (array2[0] == null)
				{
					if (array2[2] != null)
					{
						array2[0] = array2[2];
					}
					else if (array2[1] != null)
					{
						array2[0] = array2[1];
					}
					else if (array2[3] != null)
					{
						array2[0] = array2[3];
					}
				}
				if (array2[2] == null)
				{
					array2[2] = array2[0];
				}
				if (array2[1] == null)
				{
					if (array2[3] != null)
					{
						array2[1] = array2[3];
					}
					else
					{
						array2[1] = array2[0];
					}
				}
				if (array2[3] == null)
				{
					if (array2[1] != null)
					{
						array2[3] = array2[1];
					}
					else
					{
						array2[3] = array2[0];
					}
				}
			}
			for (int i = 0; i < this.mats.Length; i++)
			{
				MaterialRequest req2 = default(MaterialRequest);
				req2.mainTex = array[i];
				req2.shader = req.shader;
				req2.color = this.color;
				req2.colorTwo = this.colorTwo;
				req2.maskTex = array2[i];
				req2.shaderParameters = req.shaderParameters;
				this.mats[i] = MaterialPool.MatFrom(req2);
			}
		}

		
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Multi(initPath=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<string>(0, this.path), this.color), this.colorTwo);
		}

		
		private Material[] mats = new Material[4];

		
		private bool westFlipped;

		
		private bool eastFlipped;

		
		private float drawRotatedExtraAngleOffset;
	}
}
