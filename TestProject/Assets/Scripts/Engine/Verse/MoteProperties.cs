using System;
using UnityEngine;

namespace Verse
{
	
	public class MoteProperties
	{
		
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00018BF0 File Offset: 0x00016DF0
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		
		public bool realTime;

		
		public float fadeInTime;

		
		public float solidTime = 1f;

		
		public float fadeOutTime;

		
		public Vector3 acceleration = Vector3.zero;

		
		public float speedPerTime;

		
		public float growthRate;

		
		public bool collide;

		
		public SoundDef landSound;

		
		public Vector3 attachedDrawOffset;

		
		public bool needsMaintenance;

		
		public bool rotateTowardsTarget;

		
		public bool rotateTowardsMoveDirection;

		
		public bool scaleToConnectTargets;

		
		public bool attachedToHead;
	}
}
