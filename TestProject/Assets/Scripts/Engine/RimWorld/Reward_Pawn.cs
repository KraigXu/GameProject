using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Reward_Pawn : Reward
	{
		
		
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.pawn == null)
				{
					yield break;
				}
				foreach (GenUI.AnonymousStackElement anonymousStackElement in QuestPartUtility.GetRewardStackElementsForThings(Gen.YieldSingle<Pawn>(this.pawn), this.detailsHidden))
				{
					yield return anonymousStackElement;
				}
				IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				yield break;
				yield break;
			}
		}

		
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.SpaceRefugee, null);
			this.arrivalMode = (Rand.Bool ? Reward_Pawn.ArrivalMode.WalkIn : Reward_Pawn.ArrivalMode.DropPod);
			valueActuallyUsed = rewardValue;
		}

		
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			//Slate slate = QuestGen.slate;
		//	QuestGen.AddToGeneratedPawns(this.pawn);
			if (!this.pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(this.pawn, PawnDiscardDecideMode.Decide);
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					//inSignal = slate.Get<string>("inSignal", null, false),
					Things = Gen.YieldSingle<Pawn>(this.pawn)
				};
			}
			else
			{
				QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
				//pawnsArrive.inSignal = slate.Get<string>("inSignal", null, false);
				pawnsArrive.pawns.Add(this.pawn);
				pawnsArrive.arrivalMode = ((this.arrivalMode == Reward_Pawn.ArrivalMode.DropPod) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeWalkIn);
				pawnsArrive.joinPlayer = true;
				//pawnsArrive.mapParent = slate.Get<Map>("map", null, false).Parent;
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					//QuestGen.AddTextRequest("root", delegate(string x)
					//{
					//	pawnsArrive.customLetterLabel = x;
					//}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					//QuestGen.AddTextRequest("root", delegate(string x)
					//{
					//	pawnsArrive.customLetterText = x;
					//}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				yield return pawnsArrive;
			}
			yield break;
		}

		
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Pawn_Caravan".Translate(this.pawn);
			}
			Reward_Pawn.ArrivalMode arrivalMode = this.arrivalMode;
			if (arrivalMode == Reward_Pawn.ArrivalMode.WalkIn)
			{
				return "Reward_Pawn_WalkIn".Translate(this.pawn);
			}
			if (arrivalMode != Reward_Pawn.ArrivalMode.DropPod)
			{
				throw new Exception("Unknown arrival mode: " + this.arrivalMode);
			}
			return "Reward_Pawn_DropPod".Translate(this.pawn);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" (",
				this.pawn.MarketValue.ToStringMoney(null),
				" pawn=",
				this.pawn.ToStringSafe<Pawn>(),
				", arrivalMode=",
				this.arrivalMode,
				")"
			});
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Values.Look<Reward_Pawn.ArrivalMode>(ref this.arrivalMode, "arrivalMode", Reward_Pawn.ArrivalMode.WalkIn, false);
			Scribe_Values.Look<bool>(ref this.detailsHidden, "detailsHidden", false, false);
		}

		
		public Pawn pawn;

		
		public Reward_Pawn.ArrivalMode arrivalMode;

		
		public bool detailsHidden;

		
		private const string RootSymbol = "root";

		
		public enum ArrivalMode
		{
			
			WalkIn,
			
			DropPod
		}
	}
}
