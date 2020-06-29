﻿using System;
using UnityEngine;

namespace Verse
{
	
	public class Graphic_RandomRotated : Graphic
	{
		
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0007D0EE File Offset: 0x0007B2EE
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
		}

		
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Material matSingle = this.subGraphic.MatSingle;
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), matSingle, 0, null, 0);
		}

		
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data
			};
		}

		
		private Graphic subGraphic;

		
		private float maxAngle;
	}
}
