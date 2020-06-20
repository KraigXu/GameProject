using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1D RID: 2845
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		// Token: 0x060042EF RID: 17135 RVA: 0x001682D0 File Offset: 0x001664D0
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
		{
			return (from x in groupMaker.options
			where x.kind.isFighter
			select x).Min((PawnGenOption g) => g.Cost);
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x0016832B File Offset: 0x0016652B
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any<PawnGenOption>();
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x00168358 File Offset: 0x00166558
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error(string.Concat(new object[]
					{
						"Cannot generate pawns for ",
						parms.faction,
						" with ",
						parms.points,
						". Defaulting to a single random cheap group."
					}), false);
				}
				return;
			}
			bool allowFood = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
			Predicate<Pawn> validatorPostGear = (parms.raidStrategy != null) ? ((Pawn p) => parms.raidStrategy.Worker.CanUsePawn(p, outPawns)) : null;
			bool flag = false;
			foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnGenOption.kind, parms.faction, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, false, true, true, 1f, false, true, allowFood, true, parms.inhabitants, false, false, false, 0f, null, 1f, null, validatorPostGear, null, null, null, null, null, null, null, null, null, null));
				if (parms.forceOneIncap && !flag)
				{
					pawn.health.forceIncap = true;
					pawn.mindState.canFleeIndividual = false;
					flag = true;
				}
				outPawns.Add(pawn);
			}
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x00168560 File Offset: 0x00166760
		public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				yield return pawnGenOption.kind;
			}
			IEnumerator<PawnGenOption> enumerator = null;
			yield break;
			yield break;
		}
	}
}
