using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000845 RID: 2117
	public class ThoughtWorker_PsychicDrone : ThoughtWorker
	{
		// Token: 0x0600349A RID: 13466 RVA: 0x0012048C File Offset: 0x0011E68C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
			if (p.Map != null)
			{
				PsychicDroneLevel highestPsychicDroneLevelFor = p.Map.gameConditionManager.GetHighestPsychicDroneLevelFor(p.gender);
				if (highestPsychicDroneLevelFor > psychicDroneLevel)
				{
					psychicDroneLevel = highestPsychicDroneLevelFor;
				}
			}
			else if (p.IsCaravanMember())
			{
				foreach (Site site in Find.World.worldObjects.Sites)
				{
					foreach (SitePart sitePart in site.parts)
					{
						if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.def.Worker is SitePartWorker_ConditionCauser_PsychicDroner)
						{
							CompCauseGameCondition_PsychicEmanation compCauseGameCondition_PsychicEmanation = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicEmanation>();
							if (compCauseGameCondition_PsychicEmanation.ConditionDef.conditionClass == typeof(GameCondition_PsychicEmanation) && compCauseGameCondition_PsychicEmanation.InAoE(p.GetCaravan().Tile) && compCauseGameCondition_PsychicEmanation.gender == p.gender && compCauseGameCondition_PsychicEmanation.Level > psychicDroneLevel)
							{
								psychicDroneLevel = compCauseGameCondition_PsychicEmanation.Level;
							}
						}
					}
				}
				foreach (Map map in Find.Maps)
				{
					foreach (GameCondition gameCondition in map.gameConditionManager.ActiveConditions)
					{
						CompCauseGameCondition_PsychicEmanation compCauseGameCondition_PsychicEmanation2 = gameCondition.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicEmanation>();
						if (compCauseGameCondition_PsychicEmanation2 != null && compCauseGameCondition_PsychicEmanation2.InAoE(p.GetCaravan().Tile) && compCauseGameCondition_PsychicEmanation2.gender == p.gender && compCauseGameCondition_PsychicEmanation2.Level > psychicDroneLevel)
						{
							psychicDroneLevel = compCauseGameCondition_PsychicEmanation2.Level;
						}
					}
				}
			}
			switch (psychicDroneLevel)
			{
			case PsychicDroneLevel.None:
				return false;
			case PsychicDroneLevel.GoodMedium:
				return ThoughtState.ActiveAtStage(0);
			case PsychicDroneLevel.BadLow:
				return ThoughtState.ActiveAtStage(1);
			case PsychicDroneLevel.BadMedium:
				return ThoughtState.ActiveAtStage(2);
			case PsychicDroneLevel.BadHigh:
				return ThoughtState.ActiveAtStage(3);
			case PsychicDroneLevel.BadExtreme:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
