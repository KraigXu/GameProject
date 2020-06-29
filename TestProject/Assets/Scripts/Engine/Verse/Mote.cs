﻿using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class Mote : Thing
	{
		
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x0007E3B5 File Offset: 0x0007C5B5
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		
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

		
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0007E428 File Offset: 0x0007C628
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0007E430 File Offset: 0x0007C630
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		
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

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		
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

		
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		
		public Vector3 exactPosition;

		
		public float exactRotation;

		
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		
		public float rotationRate;

		
		public Color instanceColor = Color.white;

		
		private int lastMaintainTick;

		
		public float solidTimeOverride = -1f;

		
		public int spawnTick;

		
		public float spawnRealTime;

		
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		
		protected const float MinSpeed = 0.02f;
	}
}
