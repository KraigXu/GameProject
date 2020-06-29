﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompRadialPulse : ThingComp
	{
		
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x001B8D7C File Offset: 0x001B6F7C
		private CompProperties_RadialPulse Props
		{
			get
			{
				return (CompProperties_RadialPulse)this.props;
			}
		}

		
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x001B8D89 File Offset: 0x001B6F89
		private float RingLerpFactor
		{
			get
			{
				return (float)(Find.TickManager.TicksGame % this.Props.ticksBetweenPulses) / (float)this.Props.ticksPerPulse;
			}
		}

		
		// (get) Token: 0x0600526D RID: 21101 RVA: 0x001B8DAF File Offset: 0x001B6FAF
		private float RingScale
		{
			get
			{
				return this.Props.radius * Mathf.Lerp(0f, 2f, this.RingLerpFactor) * 1.16015625f;
			}
		}

		
		// (get) Token: 0x0600526E RID: 21102 RVA: 0x001B8DD8 File Offset: 0x001B6FD8
		private bool ParentIsActive
		{
			get
			{
				CompSendSignalOnPawnProximity comp = this.parent.GetComp<CompSendSignalOnPawnProximity>();
				return comp != null && comp.Sent;
			}
		}

		
		public override void PostDraw()
		{
			if (this.ParentIsActive)
			{
				return;
			}
			Vector3 pos = this.parent.Position.ToVector3Shifted();
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Color color = this.Props.color;
			color.a = Mathf.Lerp(this.Props.color.a, 0f, this.RingLerpFactor);
			CompRadialPulse.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, Quaternion.identity, new Vector3(this.RingScale, 1f, this.RingScale));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompRadialPulse.RingMat, 0, null, 0, CompRadialPulse.MatPropertyBlock);
		}

		
		private static readonly Material RingMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		
		private const float TextureActualRingSizeFactor = 1.16015625f;
	}
}
