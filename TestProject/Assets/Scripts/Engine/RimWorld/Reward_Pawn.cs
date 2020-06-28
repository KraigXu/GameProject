using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD8 RID: 4056
	public class Reward_Pawn : Reward
	{
		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x0600616D RID: 24941 RVA: 0x0021D1F3 File Offset: 0x0021B3F3
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

		// Token: 0x0600616E RID: 24942 RVA: 0x0021D203 File Offset: 0x0021B403
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.SpaceRefugee, null);
			this.arrivalMode = (Rand.Bool ? Reward_Pawn.ArrivalMode.WalkIn : Reward_Pawn.ArrivalMode.DropPod);
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x0021D22A File Offset: 0x0021B42A
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			QuestGen.AddToGeneratedPawns(this.pawn);
			if (!this.pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(this.pawn, PawnDiscardDecideMode.Decide);
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					inSignal = slate.Get<string>("inSignal", null, false),
					Things = Gen.YieldSingle<Pawn>(this.pawn)
				};
			}
			else
			{
				QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
				pawnsArrive.inSignal = slate.Get<string>("inSignal", null, false);
				pawnsArrive.pawns.Add(this.pawn);
				pawnsArrive.arrivalMode = ((this.arrivalMode == Reward_Pawn.ArrivalMode.DropPod) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeWalkIn);
				pawnsArrive.joinPlayer = true;
				pawnsArrive.mapParent = slate.Get<Map>("map", null, false).Parent;
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						pawnsArrive.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						pawnsArrive.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				yield return pawnsArrive;
			}
			yield break;
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x0021D260 File Offset: 0x0021B460
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

		// Token: 0x06006171 RID: 24945 RVA: 0x0021D2F0 File Offset: 0x0021B4F0
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

		// Token: 0x06006172 RID: 24946 RVA: 0x0021D366 File Offset: 0x0021B566
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Values.Look<Reward_Pawn.ArrivalMode>(ref this.arrivalMode, "arrivalMode", Reward_Pawn.ArrivalMode.WalkIn, false);
			Scribe_Values.Look<bool>(ref this.detailsHidden, "detailsHidden", false, false);
		}

		// Token: 0x04003B43 RID: 15171
		public Pawn pawn;

		// Token: 0x04003B44 RID: 15172
		public Reward_Pawn.ArrivalMode arrivalMode;

		// Token: 0x04003B45 RID: 15173
		public bool detailsHidden;

		// Token: 0x04003B46 RID: 15174
		private const string RootSymbol = "root";

		// Token: 0x02001E7B RID: 7803
		public enum ArrivalMode
		{
			// Token: 0x040072A7 RID: 29351
			WalkIn,
			// Token: 0x040072A8 RID: 29352
			DropPod
		}
	}
}
