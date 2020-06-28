using System;

namespace Verse
{
	// Token: 0x02000279 RID: 633
	public static class HediffMaker
	{
		// Token: 0x060010F6 RID: 4342 RVA: 0x0005FE44 File Offset: 0x0005E044
		public static Hediff MakeHediff(HediffDef def, Pawn pawn, BodyPartRecord partRecord = null)
		{
			if (pawn == null)
			{
				Log.Error("Cannot make hediff " + def + " for null pawn.", false);
				return null;
			}
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.pawn = pawn;
			hediff.Part = partRecord;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0005FEA7 File Offset: 0x0005E0A7
		public static Hediff Debug_MakeConcreteExampleHediff(HediffDef def)
		{
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}
	}
}
