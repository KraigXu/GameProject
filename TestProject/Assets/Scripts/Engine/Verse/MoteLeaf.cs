using System;
using UnityEngine;

namespace Verse
{
	
	public class MoteLeaf : Mote
	{
		
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x0007EB39 File Offset: 0x0007CD39
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0007EB6B File Offset: 0x0007CD6B
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		
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

		
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		
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

		
		public override void Draw()
		{
			base.Draw(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		
		private Vector3 startSpatialPosition;

		
		private Vector3 currentSpatialPosition;

		
		private float spawnDelay;

		
		private bool front;

		
		private float treeHeight;

		
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
