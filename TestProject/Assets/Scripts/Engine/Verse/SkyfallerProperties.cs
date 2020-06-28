using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A3 RID: 163
	public class SkyfallerProperties
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001A235 File Offset: 0x00018435
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001A255 File Offset: 0x00018455
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x0400030A RID: 778
		public bool hitRoof = true;

		// Token: 0x0400030B RID: 779
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x0400030C RID: 780
		public bool reversed;

		// Token: 0x0400030D RID: 781
		public float explosionRadius = 3f;

		// Token: 0x0400030E RID: 782
		public DamageDef explosionDamage;

		// Token: 0x0400030F RID: 783
		public bool damageSpawnedThings;

		// Token: 0x04000310 RID: 784
		public float explosionDamageFactor = 1f;

		// Token: 0x04000311 RID: 785
		public IntRange metalShrapnelCountRange = IntRange.zero;

		// Token: 0x04000312 RID: 786
		public IntRange rubbleShrapnelCountRange = IntRange.zero;

		// Token: 0x04000313 RID: 787
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x04000314 RID: 788
		public SkyfallerMovementType movementType;

		// Token: 0x04000315 RID: 789
		public float speed = 1f;

		// Token: 0x04000316 RID: 790
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x04000317 RID: 791
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x04000318 RID: 792
		public float cameraShake;

		// Token: 0x04000319 RID: 793
		public SoundDef impactSound;

		// Token: 0x0400031A RID: 794
		public bool rotateGraphicTowardsDirection;

		// Token: 0x0400031B RID: 795
		public SoundDef anticipationSound;

		// Token: 0x0400031C RID: 796
		public int anticipationSoundTicks = 100;

		// Token: 0x0400031D RID: 797
		public int motesPerCell = 3;

		// Token: 0x0400031E RID: 798
		public float moteSpawnTime = float.MinValue;

		// Token: 0x0400031F RID: 799
		public SimpleCurve xPositionCurve;

		// Token: 0x04000320 RID: 800
		public SimpleCurve zPositionCurve;

		// Token: 0x04000321 RID: 801
		public SimpleCurve angleCurve;

		// Token: 0x04000322 RID: 802
		public SimpleCurve rotationCurve;

		// Token: 0x04000323 RID: 803
		public SimpleCurve speedCurve;
	}
}
