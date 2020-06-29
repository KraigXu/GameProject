using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	
	public static class HediffGiverUtility
	{
		
		public static bool TryApply(Pawn pawn, HediffDef hediff, IEnumerable<BodyPartDef> partsToAffect, bool canAffectAnyLivePart = false, int countToAffect = 1, List<Hediff> outAddedHediffs = null)
		{
			if (canAffectAnyLivePart || partsToAffect != null)
			{
				bool result = false;

				for (int i = 0; i < countToAffect; i++)
				{
					IEnumerable<BodyPartRecord> enumerable = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
					if (partsToAffect != null)
					{
						IEnumerable<BodyPartRecord> source = enumerable;
						Func<BodyPartRecord, bool> predicate = ((BodyPartRecord p) => partsToAffect.Contains(p.def)); 

						enumerable = source.Where(predicate);
					}
					if (canAffectAnyLivePart)
					{
						enumerable = from p in enumerable
						where p.def.alive
						select p;
					}
					IEnumerable<BodyPartRecord> source2 = enumerable;
					Func<BodyPartRecord, bool> predicate2= ((BodyPartRecord p) => !pawn.health.hediffSet.HasHediff(hediff, p, false) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)); ;

					enumerable = source2.Where(predicate2);
					if (!enumerable.Any<BodyPartRecord>())
					{
						break;
					}
					BodyPartRecord partRecord = enumerable.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					Hediff hediff2 = HediffMaker.MakeHediff(hediff, pawn, partRecord);
					pawn.health.AddHediff(hediff2, null, null, null);
					if (outAddedHediffs != null)
					{
						outAddedHediffs.Add(hediff2);
					}
					result = true;
				}
				return result;
			}
			if (!pawn.health.hediffSet.HasHediff(hediff, false))
			{
				Hediff hediff3 = HediffMaker.MakeHediff(hediff, pawn, null);
				pawn.health.AddHediff(hediff3, null, null, null);
				if (outAddedHediffs != null)
				{
					outAddedHediffs.Add(hediff3);
				}
				return true;
			}
			return false;
		}
	}
}
