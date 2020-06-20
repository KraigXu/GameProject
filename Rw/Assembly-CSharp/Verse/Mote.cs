using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000302 RID: 770
	public abstract class Mote : Thing
	{
		// Token: 0x17000463 RID: 1123
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x0007E3B5 File Offset: 0x0007C5B5
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0007E3C9 File Offset: 0x0007C5C9
		public float AgeSecs
		{
			get
			{
				if (this.def.mote.realTime)
				{
					return Time.realtimeSinceStartup - this.spawnRealTime;
				}
				return (float)(Find.TickManager.TicksGame - this.spawnTick) / 60f;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x0007E402 File Offset: 0x0007C602
		protected float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.mote.solidTime;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0007E428 File Offset: 0x0007C628
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0007E430 File Offset: 0x0007C630
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x0007E450 File Offset: 0x0007C650
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				if (ageSecs <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return ageSecs / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (ageSecs <= this.def.mote.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.mote.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.mote.fadeInTime + this.SolidTime, this.def.mote.fadeInTime + this.SolidTime + this.def.mote.fadeOutTime, ageSecs);
					}
					return 1f;
				}
			}
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0007E530 File Offset: 0x0007C730
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0007E596 File Offset: 0x0007C796
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0007E5BA File Offset: 0x0007C7BA
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0007E5D9 File Offset: 0x0007C7D9
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0007E5F8 File Offset: 0x0007C7F8
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame - 1 > this.lastMaintainTick)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.mote.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.mote.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0007E6F7 File Offset: 0x0007C8F7
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0007E70F File Offset: 0x0007C90F
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0007E723 File Offset: 0x0007C923
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0007E735 File Offset: 0x0007C935
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0007E743 File Offset: 0x0007C943
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x04000E2A RID: 3626
		public Vector3 exactPosition;

		// Token: 0x04000E2B RID: 3627
		public float exactRotation;

		// Token: 0x04000E2C RID: 3628
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04000E2D RID: 3629
		public float rotationRate;

		// Token: 0x04000E2E RID: 3630
		public Color instanceColor = Color.white;

		// Token: 0x04000E2F RID: 3631
		private int lastMaintainTick;

		// Token: 0x04000E30 RID: 3632
		public float solidTimeOverride = -1f;

		// Token: 0x04000E31 RID: 3633
		public int spawnTick;

		// Token: 0x04000E32 RID: 3634
		public float spawnRealTime;

		// Token: 0x04000E33 RID: 3635
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x04000E34 RID: 3636
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x04000E35 RID: 3637
		protected const float MinSpeed = 0.02f;
	}
}
