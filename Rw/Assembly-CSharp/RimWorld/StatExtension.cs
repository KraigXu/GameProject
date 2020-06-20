using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF1 RID: 4081
	public static class StatExtension
	{
		// Token: 0x060061DB RID: 25051 RVA: 0x0021FF43 File Offset: 0x0021E143
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x0021FF52 File Offset: 0x0021E152
		public static float GetStatValueForPawn(this Thing thing, StatDef stat, Pawn pawn, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, pawn, applyPostProcess);
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x0021FF62 File Offset: 0x0021E162
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x0021FF71 File Offset: 0x0021E171
		public static float GetStatValueAbstract(this AbilityDef def, StatDef stat)
		{
			return stat.Worker.GetValueAbstract(def);
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x0021FF80 File Offset: 0x0021E180
		public static bool StatBaseDefined(this BuildableDef def, StatDef stat)
		{
			if (def.statBases == null)
			{
				return false;
			}
			for (int i = 0; i < def.statBases.Count; i++)
			{
				if (def.statBases[i].stat == stat)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x0021FFC4 File Offset: 0x0021E1C4
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}
