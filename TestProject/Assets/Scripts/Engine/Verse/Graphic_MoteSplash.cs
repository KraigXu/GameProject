﻿using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class Graphic_MoteSplash : Graphic_Mote
	{
		
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteSplash moteSplash = (MoteSplash)thing;
			float alpha = moteSplash.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, alpha));
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, moteSplash.CalculatedShockwaveSpan());
			base.DrawMoteInternal(loc, rot, thingDef, thing, SubcameraDefOf.WaterDepth.LayerId);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"MoteSplash(path=",
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
