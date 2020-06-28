using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000214 RID: 532
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x06000F07 RID: 3847 RVA: 0x0005575D File Offset: 0x0005395D
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0005576C File Offset: 0x0005396C
		public void RenderStatusOverlays(Vector3 bodyLoc, Quaternion quat, Mesh headMesh)
		{
			if (!this.pawn.IsColonistPlayerControlled)
			{
				return;
			}
			Vector3 headLoc = bodyLoc + new Vector3(0f, 0f, 0.32f);
			if (this.pawn.needs.mood != null && !this.pawn.Downed && this.pawn.HitPoints > 0)
			{
				if (this.pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
				{
					if (Time.time % 1.2f < 0.4f)
					{
						this.DrawHeadGlow(headLoc, PawnHeadOverlays.MentalStateImminentMat);
						return;
					}
				}
				else if (this.pawn.mindState.mentalBreaker.BreakExtremeIsApproaching && Time.time % 1.2f < 0.4f)
				{
					this.DrawHeadGlow(headLoc, PawnHeadOverlays.UnhappyMat);
				}
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0005583D File Offset: 0x00053A3D
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04000B1B RID: 2843
		private Pawn pawn;

		// Token: 0x04000B1C RID: 2844
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04000B1D RID: 2845
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04000B1E RID: 2846
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04000B1F RID: 2847
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
