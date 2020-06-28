using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B0 RID: 176
	public class SubEffecterDef
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x0001B259 File Offset: 0x00019459
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x04000386 RID: 902
		public Type subEffecterClass;

		// Token: 0x04000387 RID: 903
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x04000388 RID: 904
		public int ticksBetweenMotes = 40;

		// Token: 0x04000389 RID: 905
		public float chancePerTick = 0.1f;

		// Token: 0x0400038A RID: 906
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x0400038B RID: 907
		public float positionLerpFactor = 0.5f;

		// Token: 0x0400038C RID: 908
		public float positionRadius;

		// Token: 0x0400038D RID: 909
		public ThingDef moteDef;

		// Token: 0x0400038E RID: 910
		public Color color = Color.white;

		// Token: 0x0400038F RID: 911
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x04000390 RID: 912
		public bool absoluteAngle;

		// Token: 0x04000391 RID: 913
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x04000392 RID: 914
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x04000393 RID: 915
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x04000394 RID: 916
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x04000395 RID: 917
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04000396 RID: 918
		public SoundDef soundDef;

		// Token: 0x04000397 RID: 919
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04000398 RID: 920
		public int ticksBeforeSustainerStart;
	}
}
