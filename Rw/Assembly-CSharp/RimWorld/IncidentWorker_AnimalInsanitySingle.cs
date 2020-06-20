using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D5 RID: 2517
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x06003C1C RID: 15388 RVA: 0x0013D474 File Offset: 0x0013B674
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			Pawn pawn;
			return this.TryFindRandomAnimal(map, out pawn);
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x0013D4A4 File Offset: 0x0013B6A4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn;
			if (!this.TryFindRandomAnimal(map, out pawn))
			{
				return false;
			}
			IncidentWorker_AnimalInsanityMass.DriveInsane(pawn);
			string str = "AnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL"));
			base.SendStandardLetter("LetterLabelAnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")), str, LetterDefOf.ThreatSmall, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x0013D534 File Offset: 0x0013B734
		private bool TryFindRandomAnimal(Map map, out Pawn animal)
		{
			int maxPoints = 150;
			if (GenDate.DaysPassed < 7)
			{
				maxPoints = 40;
			}
			return (from p in map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.kindDef.combatPower <= (float)maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).TryRandomElement(out animal);
		}

		// Token: 0x04002377 RID: 9079
		private const int FixedPoints = 30;
	}
}
