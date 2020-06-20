using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D33 RID: 3379
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005210 RID: 21008 RVA: 0x001B6A92 File Offset: 0x001B4C92
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005211 RID: 21009 RVA: 0x001B6A9F File Offset: 0x001B4C9F
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005212 RID: 21010 RVA: 0x001B6AB2 File Offset: 0x001B4CB2
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005213 RID: 21011 RVA: 0x001B6AC1 File Offset: 0x001B4CC1
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x001B6AD4 File Offset: 0x001B4CD4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x001B6B33 File Offset: 0x001B4D33
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x001B6B60 File Offset: 0x001B4D60
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x001B6B6F File Offset: 0x001B4D6F
		public override void CompTick()
		{
			base.CompTick();
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
				if (this.TicksLeft < this.fadeOutDuration)
				{
					this.sustainer.End();
					this.sustainer = null;
				}
			}
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x001B6BAC File Offset: 0x001B4DAC
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.TicksLeft <= 0)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			float num = ((float)this.parent.Map.Size.z - drawPos.z) * 1.41421354f;
			Vector3 a = Vector3Utility.FromAngleFlat(this.angle - 90f);
			Vector3 a2 = drawPos + a * num * 0.5f;
			a2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			float num2 = Mathf.Min((float)this.TicksPassed / 10f, 1f);
			Vector3 b = a * ((1f - num2) * num);
			float num3 = 0.975f + Mathf.Sin((float)this.TicksPassed * 0.3f) * 0.025f;
			if (this.TicksLeft < this.fadeOutDuration)
			{
				num3 *= (float)this.TicksLeft / (float)this.fadeOutDuration;
			}
			Color color = this.Props.color;
			color.a *= num3;
			CompOrbitalBeam.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(a2 + a * this.BeamEndHeight * 0.5f + b, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompOrbitalBeam.BeamMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
			Vector3 pos = drawPos + b;
			pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Matrix4x4 matrix2 = default(Matrix4x4);
			matrix2.SetTRS(pos, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, this.BeamEndHeight));
			Graphics.DrawMesh(MeshPool.plane10, matrix2, CompOrbitalBeam.BeamEndMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x001B6DB1 File Offset: 0x001B4FB1
		private void CheckSpawnSustainer()
		{
			if (this.TicksLeft >= this.fadeOutDuration && this.Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
				});
			}
		}

		// Token: 0x04002D3B RID: 11579
		private int startTick;

		// Token: 0x04002D3C RID: 11580
		private int totalDuration;

		// Token: 0x04002D3D RID: 11581
		private int fadeOutDuration;

		// Token: 0x04002D3E RID: 11582
		private float angle;

		// Token: 0x04002D3F RID: 11583
		private Sustainer sustainer;

		// Token: 0x04002D40 RID: 11584
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x04002D41 RID: 11585
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x04002D42 RID: 11586
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x04002D43 RID: 11587
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04002D44 RID: 11588
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04002D45 RID: 11589
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
	}
}
