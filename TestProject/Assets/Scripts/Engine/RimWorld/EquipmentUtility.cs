using System;
using Verse;

namespace RimWorld
{
	
	public static class EquipmentUtility
	{
		
		public static bool CanEquip(Thing thing, Pawn pawn)
		{
			string text;
			return EquipmentUtility.CanEquip(thing, pawn, out text);
		}

		
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

		
		public static bool IsBiocoded(Thing thing)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.Biocoded;
		}

		
		public static bool IsBiocodedFor(Thing thing, Pawn pawn)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.CodedPawn == pawn;
		}

		
		public static bool QuestLodgerCanEquip(Thing thing, Pawn pawn)
		{
			return EquipmentUtility.IsBiocodedFor(thing, pawn) || (thing.def.IsWeapon && pawn.equipment.Primary == null);
		}
	}
}
