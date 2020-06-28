using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA2 RID: 3490
	public class CompProperties_UseEffectInstallImplant : CompProperties_Usable
	{
		// Token: 0x060054D1 RID: 21713 RVA: 0x001C44E7 File Offset: 0x001C26E7
		public CompProperties_UseEffectInstallImplant()
		{
			this.compClass = typeof(CompUseEffect_InstallImplant);
		}

		// Token: 0x04002E83 RID: 11907
		public HediffDef hediffDef;

		// Token: 0x04002E84 RID: 11908
		public BodyPartDef bodyPart;

		// Token: 0x04002E85 RID: 11909
		public bool canUpgrade;

		// Token: 0x04002E86 RID: 11910
		public bool allowNonColonists;
	}
}
