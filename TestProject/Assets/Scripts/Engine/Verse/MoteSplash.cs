using System;
using UnityEngine;

namespace Verse
{
	
	public class MoteSplash : Mote
	{
		
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0007EDA9 File Offset: 0x0007CFA9
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0007EDC4 File Offset: 0x0007CFC4
		public override float Alpha
		{
			get
			{
				Mathf.Clamp01(base.AgeSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - base.AgeSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float scale = base.AgeSecs * this.velocity;
			base.Scale = scale;
			this.exactPosition += base.Map.waterInfo.GetWaterMovement(this.exactPosition) * deltaTime;
		}

		
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		
		public const float VelocityFootstep = 1.5f;

		
		public const float SizeFootstep = 2f;

		
		public const float VelocityGunfire = 4f;

		
		public const float SizeGunfire = 1f;

		
		public const float VelocityExplosion = 20f;

		
		public const float SizeExplosion = 6f;

		
		private float targetSize;

		
		private float velocity;
	}
}
