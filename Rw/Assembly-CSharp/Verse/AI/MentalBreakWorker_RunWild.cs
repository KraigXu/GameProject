using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000542 RID: 1346
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06002683 RID: 9859 RVA: 0x000E2ED4 File Offset: 0x000E10D4
		public override bool BreakCanOccur(Pawn pawn)
		{
			if (!pawn.IsColonistPlayerControlled || pawn.Downed || !pawn.Spawned || !base.BreakCanOccur(pawn))
			{
				return false;
			}
			if (pawn.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
			{
				return false;
			}
			float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(pawn.Map.Tile);
			return seasonalTemp >= pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 7f && seasonalTemp <= pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + 7f;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000E2F70 File Offset: 0x000E1170
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null);
			if (pawn.Spawned && !pawn.Downed)
			{
				pawn.jobs.StopAll(false, true);
			}
			return true;
		}
	}
}
