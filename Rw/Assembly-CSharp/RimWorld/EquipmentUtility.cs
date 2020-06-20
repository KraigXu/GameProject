using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB9 RID: 4025
	public static class EquipmentUtility
	{
		// Token: 0x060060C0 RID: 24768 RVA: 0x00217520 File Offset: 0x00215720
		public static bool CanEquip(Thing thing, Pawn pawn)
		{
			string text;
			return EquipmentUtility.CanEquip(thing, pawn, out text);
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x00217538 File Offset: 0x00215738
		public static bool CanEquip(Thing thing, Pawn pawn, out string cantReason)
		{
			cantReason = null;
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.bondedPawn != null && compBladelinkWeapon.bondedPawn != pawn)
			{
				cantReason = "BladelinkBondedToSomeoneElse".Translate();
				return false;
			}
			if (EquipmentUtility.IsBiocoded(thing) && !EquipmentUtility.IsBiocodedFor(thing, pawn))
			{
				cantReason = "BiocodedCodedForSomeoneElse".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x0021759C File Offset: 0x0021579C
		public static bool IsBiocoded(Thing thing)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.Biocoded;
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x002175BC File Offset: 0x002157BC
		public static bool IsBiocodedFor(Thing thing, Pawn pawn)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.CodedPawn == pawn;
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x002175DE File Offset: 0x002157DE
		public static bool QuestLodgerCanEquip(Thing thing, Pawn pawn)
		{
			return EquipmentUtility.IsBiocodedFor(thing, pawn) || (thing.def.IsWeapon && pawn.equipment.Primary == null);
		}
	}
}
