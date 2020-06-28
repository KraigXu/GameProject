using System;

namespace Verse
{
	// Token: 0x02000433 RID: 1075
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06001FF6 RID: 8182 RVA: 0x000C375A File Offset: 0x000C195A
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000C3798 File Offset: 0x000C1998
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = this.def.chancePerTick;
			if (this.def.spawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
			{
				num *= (float)(B.Thing.def.size.x * B.Thing.def.size.z);
			}
			if (Rand.Value < num)
			{
				base.MakeMote(A, B);
			}
		}
	}
}
