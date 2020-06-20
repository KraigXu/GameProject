using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5F RID: 3167
	public class TurretTop
	{
		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x00198888 File Offset: 0x00196A88
		// (set) Token: 0x06004BD3 RID: 19411 RVA: 0x00198890 File Offset: 0x00196A90
		private float CurRotation
		{
			get
			{
				return this.curRotationInt;
			}
			set
			{
				this.curRotationInt = value;
				if (this.curRotationInt > 360f)
				{
					this.curRotationInt -= 360f;
				}
				if (this.curRotationInt < 0f)
				{
					this.curRotationInt += 360f;
				}
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x001988E4 File Offset: 0x00196AE4
		public void SetRotationFromOrientation()
		{
			this.CurRotation = this.parentTurret.Rotation.AsAngle;
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x0019890A File Offset: 0x00196B0A
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x0019891C File Offset: 0x00196B1C
		public void TurretTopTick()
		{
			LocalTargetInfo currentTarget = this.parentTurret.CurrentTarget;
			if (currentTarget.IsValid)
			{
				float curRotation = (currentTarget.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
				this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				return;
			}
			if (this.ticksUntilIdleTurn > 0)
			{
				this.ticksUntilIdleTurn--;
				if (this.ticksUntilIdleTurn == 0)
				{
					if (Rand.Value < 0.5f)
					{
						this.idleTurnClockwise = true;
					}
					else
					{
						this.idleTurnClockwise = false;
					}
					this.idleTurnTicksLeft = 140;
					return;
				}
			}
			else
			{
				if (this.idleTurnClockwise)
				{
					this.CurRotation += 0.26f;
				}
				else
				{
					this.CurRotation -= 0.26f;
				}
				this.idleTurnTicksLeft--;
				if (this.idleTurnTicksLeft <= 0)
				{
					this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				}
			}
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00198A24 File Offset: 0x00196C24
		public void DrawTurret()
		{
			Vector3 b = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y).RotatedBy(this.CurRotation);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.parentTurret.DrawPos + Altitudes.AltIncVect + b, (this.CurRotation + (float)TurretTop.ArtworkRotation).ToQuat(), new Vector3(turretTopDrawSize, 1f, turretTopDrawSize));
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.parentTurret.def.building.turretTopMat, 0);
		}

		// Token: 0x04002ACA RID: 10954
		private Building_Turret parentTurret;

		// Token: 0x04002ACB RID: 10955
		private float curRotationInt;

		// Token: 0x04002ACC RID: 10956
		private int ticksUntilIdleTurn;

		// Token: 0x04002ACD RID: 10957
		private int idleTurnTicksLeft;

		// Token: 0x04002ACE RID: 10958
		private bool idleTurnClockwise;

		// Token: 0x04002ACF RID: 10959
		private const float IdleTurnDegreesPerTick = 0.26f;

		// Token: 0x04002AD0 RID: 10960
		private const int IdleTurnDuration = 140;

		// Token: 0x04002AD1 RID: 10961
		private const int IdleTurnIntervalMin = 150;

		// Token: 0x04002AD2 RID: 10962
		private const int IdleTurnIntervalMax = 350;

		// Token: 0x04002AD3 RID: 10963
		public static readonly int ArtworkRotation = -90;
	}
}
