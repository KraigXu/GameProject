﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009FF RID: 2559
	public class IncidentWorker_TravelerGroup : IncidentWorker_NeutralGroup
	{
		// Token: 0x06003CE1 RID: 15585 RVA: 0x001420A8 File Offset: 0x001402A8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!base.TryResolveParms(parms))
			{
				return false;
			}
			IntVec3 travelDest;
			if (!RCellFinder.TryFindTravelDestFrom(parms.spawnCenter, map, out travelDest))
			{
				Log.Warning("Failed to do traveler incident from " + parms.spawnCenter + ": Couldn't find anywhere for the traveler to go.", false);
				return false;
			}
			List<Pawn> list = base.SpawnPawns(parms);
			if (list.Count == 0)
			{
				return false;
			}
			string text;
			if (list.Count == 1)
			{
				text = "SingleTravelerPassing".Translate(list[0].story.Title, parms.faction.Name, list[0].Name.ToStringFull, list[0].Named("PAWN"));
				text = text.AdjustedFor(list[0], "PAWN", true);
			}
			else
			{
				text = "GroupTravelersPassing".Translate(parms.faction.Name);
			}
			Messages.Message(text, list[0], MessageTypeDefOf.NeutralEvent, true);
			LordJob_TravelAndExit lordJob = new LordJob_TravelAndExit(travelDest);
			LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			return true;
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x0014220D File Offset: 0x0014040D
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			if (parms.points >= 0f)
			{
				return;
			}
			parms.points = Rand.ByCurve(IncidentWorker_TravelerGroup.PointsCurve);
		}

		// Token: 0x04002395 RID: 9109
		private static readonly SimpleCurve PointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(40f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(200f, 0.5f),
				true
			},
			{
				new CurvePoint(300f, 0.1f),
				true
			},
			{
				new CurvePoint(500f, 0f),
				true
			}
		};
	}
}
