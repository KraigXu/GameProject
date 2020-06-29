using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class TurretTop
	{
		
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

		
		public void SetRotationFromOrientation()
		{
			this.CurRotation = this.parentTurret.Rotation.AsAngle;
		}

		
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		
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

		
		public void DrawTurret()
		{
			Vector3 b = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y).RotatedBy(this.CurRotation);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.parentTurret.DrawPos + Altitudes.AltIncVect + b, (this.CurRotation + (float)TurretTop.ArtworkRotation).ToQuat(), new Vector3(turretTopDrawSize, 1f, turretTopDrawSize));
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.parentTurret.def.building.turretTopMat, 0);
		}

		
		private Building_Turret parentTurret;

		
		private float curRotationInt;

		
		private int ticksUntilIdleTurn;

		
		private int idleTurnTicksLeft;

		
		private bool idleTurnClockwise;

		
		private const float IdleTurnDegreesPerTick = 0.26f;

		
		private const int IdleTurnDuration = 140;

		
		private const int IdleTurnIntervalMin = 150;

		
		private const int IdleTurnIntervalMax = 350;

		
		public static readonly int ArtworkRotation = -90;
	}
}
