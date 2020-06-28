using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4F RID: 2895
	public static class PawnRelationUtility
	{
		// Token: 0x06004409 RID: 17417 RVA: 0x001700F4 File Offset: 0x0016E2F4
		public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
		{
			if (me == other)
			{
				yield break;
			}
			if (!me.RaceProps.IsFlesh || !other.RaceProps.IsFlesh)
			{
				yield break;
			}
			if (!me.relations.RelatedToAnyoneOrAnyoneRelatedToMe || !other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				yield break;
			}
			bool anyNonKinFamilyByBloodRelation = false;
			List<PawnRelationDef> defs = DefDatabase<PawnRelationDef>.AllDefsListForReading;
			int i = 0;
			int count = defs.Count;
			while (i < count)
			{
				PawnRelationDef pawnRelationDef = defs[i];
				if (pawnRelationDef != PawnRelationDefOf.Kin && pawnRelationDef.Worker.InRelation(me, other))
				{
					if (pawnRelationDef.familyByBloodRelation)
					{
						anyNonKinFamilyByBloodRelation = true;
					}
					yield return pawnRelationDef;
				}
				int num = i;
				i = num + 1;
			}
			if (!anyNonKinFamilyByBloodRelation && PawnRelationDefOf.Kin.Worker.InRelation(me, other))
			{
				yield return PawnRelationDefOf.Kin;
			}
			defs = null;
			yield break;
			yield break;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x0017010C File Offset: 0x0016E30C
		public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
		{
			PawnRelationDef pawnRelationDef = null;
			foreach (PawnRelationDef pawnRelationDef2 in me.GetRelations(other))
			{
				if (pawnRelationDef == null || pawnRelationDef2.importance > pawnRelationDef.importance)
				{
					pawnRelationDef = pawnRelationDef2;
				}
			}
			return pawnRelationDef;
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0017016C File Offset: 0x0016E36C
		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			bool flag = false;
			foreach (Pawn pawn in seenPawns)
			{
				if (pawn.RaceProps.IsFlesh && (informEvenIfSeenBefore || !pawn.relations.everSeenByPlayer))
				{
					pawn.relations.everSeenByPlayer = true;
					bool flag2 = false;
					foreach (Pawn pawn2 in enumerable)
					{
						if (pawn != pawn2)
						{
							PawnRelationDef mostImportantRelation = pawn2.GetMostImportantRelation(pawn);
							if (mostImportantRelation != null)
							{
								if (!flag2)
								{
									flag2 = true;
									if (flag)
									{
										stringBuilder.AppendLine();
									}
									if (writeSeenPawnsNames)
									{
										stringBuilder.AppendLine(pawn.KindLabel.CapitalizeFirst() + " " + pawn.NameShortColored.Resolve() + ":");
									}
								}
								flag = true;
								stringBuilder.AppendLine("  - " + "Relationship".Translate(mostImportantRelation.GetGenderSpecificLabelCap(pawn), pawn2.KindLabel + " " + pawn2.NameShortColored.Resolve(), pawn2));
							}
						}
					}
				}
			}
			if (flag)
			{
				pawnRelationsInfo = stringBuilder.ToString().TrimEndNewlines();
				return;
			}
			pawnRelationsInfo = null;
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x00170344 File Offset: 0x0016E544
		public static void Notify_PawnsSeenByPlayer_Letter(IEnumerable<Pawn> seenPawns, ref TaggedString letterLabel, ref TaggedString letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(seenPawns, out text, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				if (letterLabel.NullOrEmpty())
				{
					letterLabel = "LetterLabelNoticedRelatedPawns".Translate();
				}
				else
				{
					letterLabel += ": " + "RelationshipAppendedLetterSuffix".Translate().CapitalizeFirst();
				}
				if (!letterText.NullOrEmpty())
				{
					letterText += "\n\n";
				}
				letterText += relationsInfoHeader + "\n\n" + text;
			}
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x001703EC File Offset: 0x0016E5EC
		public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			TaggedString label = "";
			TaggedString text = "";
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(seenPawns, ref label, ref text, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				Pawn pawn = null;
				foreach (Pawn pawn2 in seenPawns)
				{
					if (PawnRelationUtility.GetMostImportantColonyRelative(pawn2) != null)
					{
						pawn = pawn2;
						break;
					}
				}
				if (pawn == null)
				{
					pawn = seenPawns.FirstOrDefault<Pawn>();
				}
				Find.LetterStack.ReceiveLetter(label, text, letterDef, pawn, null, null, null, null);
			}
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x0017048C File Offset: 0x0016E68C
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, Pawn pawn)
		{
			TaggedString taggedString = null;
			return PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref taggedString, pawn);
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x001704AC File Offset: 0x0016E6AC
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, ref TaggedString title, Pawn pawn)
		{
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative == null)
			{
				return false;
			}
			if (title != null)
			{
				title += " (" + "RelationshipAppendedLetterSuffix".Translate() + ")";
			}
			string genderSpecificLabel = mostImportantColonyRelative.GetMostImportantRelation(pawn).GetGenderSpecificLabel(pawn);
			if (mostImportantColonyRelative.IsColonist)
			{
				text += "\n\n" + "RelationshipAppendedLetterTextColonist".Translate(mostImportantColonyRelative.LabelShort, genderSpecificLabel, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			else
			{
				text += "\n\n" + "RelationshipAppendedLetterTextPrisoner".Translate(mostImportantColonyRelative.LabelShort, genderSpecificLabel, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			return true;
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x001705D0 File Offset: 0x0016E7D0
		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return null;
			}
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			float num = 0f;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in enumerable)
			{
				PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(pawn3);
				if (mostImportantRelation != null && (pawn2 == null || mostImportantRelation.importance > num))
				{
					num = mostImportantRelation.importance;
					pawn2 = pawn3;
				}
			}
			return pawn2;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x00170680 File Offset: 0x0016E880
		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			if (num < 0f)
			{
				return -1f;
			}
			return num;
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x001706A6 File Offset: 0x0016E8A6
		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}
	}
}
