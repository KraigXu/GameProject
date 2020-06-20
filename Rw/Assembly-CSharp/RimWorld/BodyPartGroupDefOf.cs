using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6C RID: 3948
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x06006073 RID: 24691 RVA: 0x00216F03 File Offset: 0x00215103
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}

		// Token: 0x04003790 RID: 14224
		public static BodyPartGroupDef Torso;

		// Token: 0x04003791 RID: 14225
		public static BodyPartGroupDef Legs;

		// Token: 0x04003792 RID: 14226
		public static BodyPartGroupDef LeftHand;

		// Token: 0x04003793 RID: 14227
		public static BodyPartGroupDef RightHand;

		// Token: 0x04003794 RID: 14228
		public static BodyPartGroupDef FullHead;

		// Token: 0x04003795 RID: 14229
		public static BodyPartGroupDef UpperHead;

		// Token: 0x04003796 RID: 14230
		public static BodyPartGroupDef Eyes;
	}
}
