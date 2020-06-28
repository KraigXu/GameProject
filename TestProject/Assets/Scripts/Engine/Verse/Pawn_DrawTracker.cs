using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000213 RID: 531
	public class Pawn_DrawTracker
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x000554B0 File Offset: 0x000536B0
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				Vector3 vector = this.tweener.TweenedPos;
				vector += this.jitterer.CurrentOffset;
				vector += this.leaner.LeanOffset;
				vector.y = this.pawn.def.Altitude;
				return vector;
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00055510 File Offset: 0x00053710
		public Pawn_DrawTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.tweener = new PawnTweener(pawn);
			this.jitterer = new JitterHandler();
			this.leaner = new PawnLeaner(pawn);
			this.renderer = new PawnRenderer(pawn);
			this.ui = new PawnUIOverlay(pawn);
			this.footprintMaker = new PawnFootprintMaker(pawn);
			this.breathMoteMaker = new PawnBreathMoteMaker(pawn);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00055580 File Offset: 0x00053780
		public void DrawTrackerTick()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (Current.ProgramState == ProgramState.Playing && !Find.CameraDriver.CurrentViewRect.ExpandedBy(3).Contains(this.pawn.Position))
			{
				return;
			}
			this.jitterer.JitterHandlerTick();
			this.footprintMaker.FootprintMakerTick();
			this.breathMoteMaker.BreathMoteMakerTick();
			this.leaner.LeanerTick();
			this.renderer.RendererTick();
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00055603 File Offset: 0x00053803
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00055611 File Offset: 0x00053811
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0005561E File Offset: 0x0005381E
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0005562D File Offset: 0x0005382D
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageApplied(dinfo);
			this.renderer.Notify_DamageApplied(dinfo);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00055655 File Offset: 0x00053855
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageDeflected(dinfo);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00055674 File Offset: 0x00053874
		public void Notify_MeleeAttackOn(Thing Target)
		{
			if (Target.Position != this.pawn.Position)
			{
				this.jitterer.AddOffset(0.5f, (Target.Position - this.pawn.Position).AngleFlat);
				return;
			}
			if (Target.DrawPos != this.pawn.DrawPos)
			{
				this.jitterer.AddOffset(0.25f, (Target.DrawPos - this.pawn.DrawPos).AngleFlat());
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0005570C File Offset: 0x0005390C
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				MoteMaker.ThrowAirPuffUp(this.pawn.DrawPos, this.pawn.Map);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x04000B12 RID: 2834
		private Pawn pawn;

		// Token: 0x04000B13 RID: 2835
		public PawnTweener tweener;

		// Token: 0x04000B14 RID: 2836
		private JitterHandler jitterer;

		// Token: 0x04000B15 RID: 2837
		public PawnLeaner leaner;

		// Token: 0x04000B16 RID: 2838
		public PawnRenderer renderer;

		// Token: 0x04000B17 RID: 2839
		public PawnUIOverlay ui;

		// Token: 0x04000B18 RID: 2840
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04000B19 RID: 2841
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04000B1A RID: 2842
		private const float MeleeJitterDistance = 0.5f;
	}
}
