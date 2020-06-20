using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001076 RID: 4214
	public class HediffCompProperties_DissolveGearOnDeath : HediffCompProperties
	{
		// Token: 0x0600641A RID: 25626 RVA: 0x0022AE30 File Offset: 0x00229030
		public HediffCompProperties_DissolveGearOnDeath()
		{
			this.compClass = typeof(HediffComp_DissolveGearOnDeath);
		}

		// Token: 0x04003CE5 RID: 15589
		public ThingDef mote;

		// Token: 0x04003CE6 RID: 15590
		public int moteCount = 3;

		// Token: 0x04003CE7 RID: 15591
		public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x04003CE8 RID: 15592
		public ThingDef filth;

		// Token: 0x04003CE9 RID: 15593
		public int filthCount = 4;

		// Token: 0x04003CEA RID: 15594
		public HediffDef injuryCreatedOnDeath;

		// Token: 0x04003CEB RID: 15595
		public IntRange injuryCount;

		// Token: 0x04003CEC RID: 15596
		public SoundDef sound;
	}
}
