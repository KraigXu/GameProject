using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E7 RID: 2535
	public static class ManhunterPackIncidentUtility
	{
		// Token: 0x06003C61 RID: 15457 RVA: 0x0013F014 File Offset: 0x0013D214
		public static float ManhunterAnimalWeight(PawnKindDef animal, float points)
		{
			points = Mathf.Max(points, 70f);
			if (animal.combatPower * 2f > points)
			{
				return 0f;
			}
			int num = Mathf.RoundToInt(points / animal.combatPower);
			return Mathf.Clamp01(Mathf.InverseLerp(100f, 10f, (float)num));
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x0013F068 File Offset: 0x0013D268
		public static bool TryFindManhunterAnimalKind(float points, int tile, out PawnKindDef animalKind)
		{
			IEnumerable<PawnKindDef> source = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter && (tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k;
			if (source.TryRandomElementByWeight((PawnKindDef a) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(a, points), out animalKind))
			{
				return true;
			}
			if (points > source.Min((PawnKindDef a) => a.combatPower) * 2f)
			{
				animalKind = source.MaxBy((PawnKindDef a) => a.combatPower);
				return true;
			}
			animalKind = null;
			return false;
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x0013F117 File Offset: 0x0013D317
		public static int GetAnimalsCount(PawnKindDef animalKind, float points)
		{
			return Mathf.Max(Mathf.RoundToInt(points / animalKind.combatPower), 2);
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0013F12C File Offset: 0x0013D32C
		[Obsolete("Obsolete, only used to avoid error when patching")]
		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile, float points)
		{
			return ManhunterPackIncidentUtility.GenerateAnimals_NewTmp(animalKind, tile, points, 0);
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x0013F138 File Offset: 0x0013D338
		public static List<Pawn> GenerateAnimals_NewTmp(PawnKindDef animalKind, int tile, float points, int animalCount = 0)
		{
			List<Pawn> list = new List<Pawn>();
			int num = (animalCount > 0) ? animalCount : ManhunterPackIncidentUtility.GetAnimalsCount(animalKind, points);
			for (int i = 0; i < num; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0013F1D4 File Offset: 0x0013D3D4
		[DebugOutput]
		public static void ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter
			orderby -k.combatPower
			select k).ToList<PawnKindDef>();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add(20f * Mathf.Pow(1.25f, (float)i));
			}
			DebugTables.MakeTablesDialog<float, PawnKindDef>(list, (float points) => points.ToString("F0") + " pts", candidates, (PawnKindDef candidate) => candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")", delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points));
				float num2 = ManhunterPackIncidentUtility.ManhunterAnimalWeight(candidate, points);
				if (num2 == 0f)
				{
					return "0%";
				}
				return string.Format("{0}%, {1}", (num2 * 100f / num).ToString("F0"), Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
			}, "");
		}

		// Token: 0x0400238B RID: 9099
		public const int MinAnimalCount = 2;

		// Token: 0x0400238C RID: 9100
		public const float MinPoints = 70f;
	}
}
