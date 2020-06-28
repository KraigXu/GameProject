using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEC RID: 2796
	[StaticConstructorOnStartup]
	public class PawnWoundDrawer
	{
		// Token: 0x0600420F RID: 16911 RVA: 0x0016100E File Offset: 0x0015F20E
		public PawnWoundDrawer(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00161030 File Offset: 0x0015F230
		public void RenderOverBody(Vector3 drawLoc, Mesh bodyMesh, Quaternion quat, bool forPortrait)
		{
			int num = 0;
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.displayWound)
				{
					Hediff_Injury hediff_Injury = hediffs[i] as Hediff_Injury;
					if (hediff_Injury == null || !hediff_Injury.IsPermanent())
					{
						num++;
					}
				}
			}
			int num2 = Mathf.CeilToInt((float)num / 2f);
			if (num2 > this.MaxDisplayWounds)
			{
				num2 = this.MaxDisplayWounds;
			}
			while (this.wounds.Count < num2)
			{
				this.wounds.Add(new PawnWoundDrawer.Wound(this.pawn));
				PortraitsCache.SetDirty(this.pawn);
			}
			while (this.wounds.Count > num2)
			{
				this.wounds.Remove(this.wounds.RandomElement<PawnWoundDrawer.Wound>());
				PortraitsCache.SetDirty(this.pawn);
			}
			for (int j = 0; j < this.wounds.Count; j++)
			{
				this.wounds[j].DrawWound(drawLoc, quat, this.pawn.Rotation, forPortrait);
			}
		}

		// Token: 0x0400262F RID: 9775
		protected Pawn pawn;

		// Token: 0x04002630 RID: 9776
		private List<PawnWoundDrawer.Wound> wounds = new List<PawnWoundDrawer.Wound>();

		// Token: 0x04002631 RID: 9777
		private int MaxDisplayWounds = 3;

		// Token: 0x02001AA0 RID: 6816
		private class Wound
		{
			// Token: 0x06009832 RID: 38962 RVA: 0x002EC8AC File Offset: 0x002EAAAC
			public Wound(Pawn pawn)
			{
				this.mat = pawn.RaceProps.FleshType.ChooseWoundOverlay();
				if (this.mat == null)
				{
					Log.ErrorOnce(string.Format("No wound graphics data available for flesh type {0}", pawn.RaceProps.FleshType), 76591733, false);
					this.mat = FleshTypeDefOf.Normal.ChooseWoundOverlay();
				}
				this.quat = Quaternion.AngleAxis((float)Rand.Range(0, 360), Vector3.up);
				for (int i = 0; i < 4; i++)
				{
					this.locsPerSide.Add(new Vector2(Rand.Value, Rand.Value));
				}
			}

			// Token: 0x06009833 RID: 38963 RVA: 0x002EC960 File Offset: 0x002EAB60
			public void DrawWound(Vector3 drawLoc, Quaternion bodyQuat, Rot4 bodyRot, bool forPortrait)
			{
				Vector2 vector = this.locsPerSide[bodyRot.AsInt];
				drawLoc += new Vector3((vector.x - 0.5f) * PawnWoundDrawer.Wound.WoundSpan.x, 0f, (vector.y - 0.5f) * PawnWoundDrawer.Wound.WoundSpan.y);
				drawLoc.z -= 0.3f;
				GenDraw.DrawMeshNowOrLater(MeshPool.plane025, drawLoc, this.quat, this.mat, forPortrait);
			}

			// Token: 0x04006522 RID: 25890
			private List<Vector2> locsPerSide = new List<Vector2>();

			// Token: 0x04006523 RID: 25891
			private Material mat;

			// Token: 0x04006524 RID: 25892
			private Quaternion quat;

			// Token: 0x04006525 RID: 25893
			private static readonly Vector2 WoundSpan = new Vector2(0.18f, 0.3f);
		}
	}
}
