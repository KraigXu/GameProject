﻿using System;
using UnityEngine;

namespace Verse
{
	
	public class Graphic_MotePulse : Graphic_Mote
	{
		
		
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_MotePulse(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
