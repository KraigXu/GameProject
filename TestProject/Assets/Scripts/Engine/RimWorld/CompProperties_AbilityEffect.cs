using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC6 RID: 2758
	public class CompProperties_AbilityEffect : AbilityCompProperties
	{
		// Token: 0x040025EE RID: 9710
		public int goodwillImpact;

		// Token: 0x040025EF RID: 9711
		public bool psychic;

		// Token: 0x040025F0 RID: 9712
		public bool applicableToMechs = true;

		// Token: 0x040025F1 RID: 9713
		public bool applyGoodwillImpactToLodgers = true;

		// Token: 0x040025F2 RID: 9714
		public ClamorDef clamorType;

		// Token: 0x040025F3 RID: 9715
		public int clamorRadius;

		// Token: 0x040025F4 RID: 9716
		public float screenShakeIntensity;

		// Token: 0x040025F5 RID: 9717
		public SoundDef sound;

		// Token: 0x040025F6 RID: 9718
		public string customLetterLabel;

		// Token: 0x040025F7 RID: 9719
		public string customLetterText;

		// Token: 0x040025F8 RID: 9720
		public bool sendLetter = true;

		// Token: 0x040025F9 RID: 9721
		public string message;

		// Token: 0x040025FA RID: 9722
		public MessageTypeDef messageType;

		// Token: 0x040025FB RID: 9723
		public float weight = 1f;

		// Token: 0x040025FC RID: 9724
		public bool availableWhenTargetIsWounded = true;
	}
}
