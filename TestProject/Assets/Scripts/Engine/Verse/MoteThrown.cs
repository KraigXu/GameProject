using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000309 RID: 777
	public class MoteThrown : Mote
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x0007EFBA File Offset: 0x0007D1BA
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x0007EFC9 File Offset: 0x0007D1C9
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x0007EFE2 File Offset: 0x0007D1E2
		// (set) Token: 0x060015DA RID: 5594 RVA: 0x0007EFEA File Offset: 0x0007D1EA
		public Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
			set
			{
				this.velocity = value;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x0007EFF3 File Offset: 0x0007D1F3
		// (set) Token: 0x060015DC RID: 5596 RVA: 0x0007F000 File Offset: 0x0007D200
		public float MoveAngle
		{
			get
			{
				return this.velocity.AngleFlat();
			}
			set
			{
				this.SetVelocity(value, this.Speed);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x0007F00F File Offset: 0x0007D20F
		// (set) Token: 0x060015DE RID: 5598 RVA: 0x0007F01C File Offset: 0x0007D21C
		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0f)
				{
					this.velocity = Vector3.zero;
					return;
				}
				if (this.velocity == Vector3.zero)
				{
					this.velocity = new Vector3(value, 0f, 0f);
					return;
				}
				this.velocity = this.velocity.normalized * value;
			}
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0007F080 File Offset: 0x0007D280
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			if (!this.Flying && !this.Skidding)
			{
				return;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != base.Position)
			{
				if (!intVec.InBounds(base.Map))
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				if (this.def.mote.collide && intVec.Filled(base.Map))
				{
					this.WallHit();
					return;
				}
			}
			base.Position = intVec;
			this.exactPosition = vector;
			if (this.def.mote.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.exactRotation = this.velocity.AngleFlat();
			}
			else
			{
				this.exactRotation += this.rotationRate * deltaTime;
			}
			this.velocity += this.def.mote.acceleration * deltaTime;
			if (this.def.mote.speedPerTime != 0f)
			{
				this.Speed = Mathf.Max(this.Speed + this.def.mote.speedPerTime * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.def.mote.landSound.NullOrUndefined())
				{
					this.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0007F297 File Offset: 0x0007D497
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0007F2B0 File Offset: 0x0007D4B0
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0007F2D3 File Offset: 0x0007D4D3
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x04000E4A RID: 3658
		public float airTimeLeft = 999999f;

		// Token: 0x04000E4B RID: 3659
		protected Vector3 velocity = Vector3.zero;
	}
}
