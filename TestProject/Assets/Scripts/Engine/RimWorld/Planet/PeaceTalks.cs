using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	
	public class PeaceTalks : WorldObject
	{
		
		
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		
		public void Notify_CaravanArrived(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn == null)
			{
				Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), caravan, MessageTypeDefOf.NegativeEvent, false);
				return;
			}
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(pawn);
			float num = 1f / badOutcomeWeightFactor;
			PeaceTalks.tmpPossibleOutcomes.Clear();
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
			{
				this.Outcome_Disaster(caravan);
			}, 0.05f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
			{
				this.Outcome_Backfire(caravan);
			}, 0.1f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
			{
				this.Outcome_TalksFlounder(caravan);
			}, 0.2f));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
			{
				this.Outcome_Success(caravan);
			}, 0.55f * num));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
			{
				this.Outcome_Triumph(caravan);
			}, 0.1f * num));
			PeaceTalks.tmpPossibleOutcomes.RandomElementByWeight((Pair<Action, float> x) => x.Second).First();
			pawn.skills.Learn(SkillDefOf.Social, 6000f, true);
			QuestUtility.SendQuestTargetSignals(this.questTags, "Resolved", this.Named("SUBJECT"));
			this.Destroy();
		}

		
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{

			{
				
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitPeaceTalks.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		private void Outcome_Disaster(Caravan caravan)
		{
			LongEventHandler.QueueLongEvent(delegate
			{
				FactionRelationKind playerRelationKind = this.Faction.PlayerRelationKind;
				int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
				this.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
				this.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, caravan);
				incidentParms.faction = this.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list, false);
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(this.Faction, true, true, false, false, true), map, list);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				GlobalTargetInfo target = list.Any<Pawn>() ? new GlobalTargetInfo(list[0].Position, map, false) : GlobalTargetInfo.Invalid;
				TaggedString label = "LetterLabelPeaceTalks_Disaster".Translate();
				TaggedString text = this.GetLetterText("LetterPeaceTalks_Disaster".Translate(this.Faction.def.pawnsPlural.CapitalizeFirst(), this.Faction.NameColored, Mathf.RoundToInt((float)randomInRange)), caravan, playerRelationKind, 0);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, target, this.Faction, null, null, null);
			}, "GeneratingMapForNewEncounter", false, null, true);
		}

		
		private void Outcome_Backfire(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksBackfireRange.RandomInRange;
			base.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), this.GetLetterText("LetterPeaceTalks_Backfire".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, 0), LetterDefOf.NegativeEvent, caravan, base.Faction, null, null, null);
		}

		
		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(base.Faction.NameColored), caravan, base.Faction.PlayerRelationKind, 0), LetterDefOf.NeutralEvent, caravan, base.Faction, null, null, null);
		}

		
		private void Outcome_Success(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksSuccessRange.RandomInRange;
			base.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), this.GetLetterText("LetterPeaceTalks_Success".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null);
		}

		
		private void Outcome_Triumph(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksTriumphRange.RandomInRange;
			base.Faction.TryAffectGoodwillWith(Faction.OfPlayer, randomInRange, false, false, null, null);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.makingFaction = base.Faction;
			parms.techLevel = new TechLevel?(base.Faction.def.techLevel);
			parms.maxTotalMass = new float?((float)20);
			parms.totalMarketValueRange = new FloatRange?(new FloatRange(500f, 1200f));
			parms.tile = new int?(base.Tile);
			List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				caravan.AddPawnOrItem(list[i], true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), this.GetLetterText("LetterPeaceTalks_Triumph".Translate(base.Faction.NameColored, randomInRange, GenLabel.ThingsLabel(list, "  - ")), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null);
		}

		
		private int TryGainRoyalFavor(Caravan caravan)
		{
			int num = 0;
			if (base.Faction.def.HasRoyalTitles)
			{
				num = DiplomacyTuning.RoyalFavor_PeaceTalksSuccessRange.RandomInRange;
				Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
				if (pawn != null)
				{
					pawn.royalty.GainFavor(base.Faction, num);
				}
			}
			return num;
		}

		
		private string GetLetterText(string baseText, Caravan caravan, FactionRelationKind previousRelationKind, int royalFavorGained = 0)
		{
			TaggedString taggedString = baseText;
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn != null)
			{
				taggedString += "\n\n" + "PeaceTalksSocialXPGain".Translate(pawn.LabelShort, 6000f.ToString("F0"), pawn.Named("PAWN"));
				if (royalFavorGained > 0)
				{
					taggedString += "\n\n" + "PeaceTalksRoyalFavorGain".Translate(pawn.LabelShort, royalFavorGained.ToString(), base.Faction.Named("FACTION"), pawn.Named("PAWN"));
				}
			}
			base.Faction.TryAppendRelationKindChangedInfo(ref taggedString, previousRelationKind, base.Faction.PlayerRelationKind, null);
			return taggedString;
		}

		
		private static float GetBadOutcomeWeightFactor(Pawn diplomat)
		{
			return PeaceTalks.GetBadOutcomeWeightFactor(diplomat.GetStatValue(StatDefOf.NegotiationAbility, true));
		}

		
		private static float GetBadOutcomeWeightFactor(float negotationAbility)
		{
			return PeaceTalks.BadOutcomeChanceFactorByNegotiationAbility.Evaluate(negotationAbility);
		}

		
		[DebugOutput("Incidents", false)]
		private static void PeaceTalksChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString(), false);
		}

		
		private static void AppendDebugChances(StringBuilder sb, float negotiationAbility)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			sb.AppendLine("--- NegotiationAbility = " + negotiationAbility.ToStringPercent() + " ---");
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(negotiationAbility);
			float num = 1f / badOutcomeWeightFactor;
			sb.AppendLine("Bad outcome weight factor: " + badOutcomeWeightFactor.ToString("0.##"));
			float num2 = 0.05f * badOutcomeWeightFactor;
			float num3 = 0.1f * badOutcomeWeightFactor;
			float num4 = 0.2f;
			float num5 = 0.55f * num;
			float num6 = 0.1f * num;
			float num7 = num2 + num3 + num4 + num5 + num6;
			sb.AppendLine("Disaster: " + (num2 / num7).ToStringPercent());
			sb.AppendLine("Backfire: " + (num3 / num7).ToStringPercent());
			sb.AppendLine("Talks flounder: " + (num4 / num7).ToStringPercent());
			sb.AppendLine("Success: " + (num5 / num7).ToStringPercent());
			sb.AppendLine("Triumph: " + (num6 / num7).ToStringPercent());
		}

		
		private Material cachedMat;

		
		private static readonly SimpleCurve BadOutcomeChanceFactorByNegotiationAbility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 4f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.4f),
				true
			}
		};

		
		private const float BaseWeight_Disaster = 0.05f;

		
		private const float BaseWeight_Backfire = 0.1f;

		
		private const float BaseWeight_TalksFlounder = 0.2f;

		
		private const float BaseWeight_Success = 0.55f;

		
		private const float BaseWeight_Triumph = 0.1f;

		
		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();
	}
}
