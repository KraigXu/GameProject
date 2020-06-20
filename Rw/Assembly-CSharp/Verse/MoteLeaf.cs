using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000306 RID: 774
	public class MoteLeaf : Mote
	{
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x0007EB39 File Offset: 0x0007CD39
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0007EB6B File Offset: 0x0007CD6B
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0007EB80 File Offset: 0x0007CD80
		public override float Alpha
		{
			get
			{
				float num = base.AgeSecs;
				if (num <= this.spawnDelay)
				{
					return 0f;
				}
				num -= this.spawnDelay;
				if (num <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return num / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.FallTime + base.SolidTime)
					{
						return 1f;
					}
					num -= this.FallTime + base.SolidTime;
					if (num <= this.def.mote.fadeOutTime)
					{
						return 1f - Mathf.InverseLerp(0f, this.def.mote.fadeOutTime, num);
					}
					num -= this.def.mote.fadeOutTime;
					return 0f;
				}
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0007EC62 File Offset: 0x0007CE62
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0007EC8C File Offset: 0x0007CE8C
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float ageSecs = base.AgeSecs;
			this.exactPosition = this.startSpatialPosition;
			if (ageSecs > this.spawnDelay)
			{
				this.exactPosition.y = this.exactPosition.y - MoteLeaf.FallSpeed * (ageSecs - this.spawnDelay);
			}
			this.exactPosition.y = Mathf.Max(this.exactPosition.y, 0f);
			this.currentSpatialPosition = this.exactPosition;
			this.exactPosition.z = this.exactPosition.z + this.exactPosition.y;
			this.exactPosition.y = 0f;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0007ED38 File Offset: 0x0007CF38
		public override void Draw()
		{
			base.Draw(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x04000E39 RID: 3641
		private Vector3 startSpatialPosition;

		// Token: 0x04000E3A RID: 3642
		private Vector3 currentSpatialPosition;

		// Token: 0x04000E3B RID: 3643
		private float spawnDelay;

		// Token: 0x04000E3C RID: 3644
		private bool front;

		// Token: 0x04000E3D RID: 3645
		private float treeHeight;

		// Token: 0x04000E3E RID: 3646
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
