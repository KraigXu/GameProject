using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F67 RID: 3943
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x0600606E RID: 24686 RVA: 0x00216EAE File Offset: 0x002150AE
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		// Token: 0x0400375F RID: 14175
		public static BodyDef Human;

		// Token: 0x04003760 RID: 14176
		public static BodyDef MechanicalCentipede;
	}
}
