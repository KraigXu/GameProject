using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B47 RID: 2887
	public class InteractionWorker_MarriageProposal : InteractionWorker
	{
		// Token: 0x060043D2 RID: 17362 RVA: 0x0016D9A4 File Offset: 0x0016BBA4
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			DirectPawnRelation directRelation = initiator.relations.GetDirectRelation(PawnRelationDefOf.Lover, recipient);
			if (directRelation == null)
			{
				return 0f;
			}
			Pawn spouse = recipient.GetSpouse();
			Pawn spouse2 = initiator.GetSpouse();
			if ((spouse != null && !spouse.Dead) || (spouse2 != null && !spouse2.Dead))
			{
				return 0f;
			}
			float num = 0.4f;
			float value = (float)(Find.TickManager.TicksGame - directRelation.startTicks) / 60000f;
			num *= Mathf.InverseLerp(0f, 60f, value);
			num *= Mathf.InverseLerp(0f, 60f, (float)initiator.relations.OpinionOf(recipient));
			if (recipient.relations.OpinionOf(initiator) < 0)
			{
				num *= 0.3f;
			}
			if (initiator.gender == Gender.Female)
			{
				num *= 0.2f;
			}
			return num;
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0016DA74 File Offset: 0x0016BC74
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			float num = this.AcceptanceChance(initiator, recipient);
			bool flag = Rand.Value < num;
			bool flag2 = false;
			if (flag)
			{
				initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.Fiance, recipient);
				if (recipient.needs.mood != null)
				{
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposal, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposalMood, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.IRejectedTheirProposal, initiator);
				}
				if (initiator.needs.mood != null)
				{
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposalMood, recipient);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.IRejectedTheirProposal, recipient);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposal, recipient);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalAccepted);
			}
			else
			{
				if (initiator.needs.mood != null)
				{
					initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RejectedMyProposal, recipient);
				}
				if (recipient.needs.mood != null)
				{
					recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.IRejectedTheirProposal, initiator);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejected);
				if (Rand.Value < 0.4f)
				{
					initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
					initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
					flag2 = true;
					extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejectedBrokeUp);
				}
			}
			if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (flag)
				{
					letterLabel = "LetterLabelAcceptedProposal".Translate();
					letterDef = LetterDefOf.PositiveEvent;
					stringBuilder.AppendLine("LetterAcceptedProposal".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT")));
					if (initiator.relations.nextMarriageNameChange != MarriageNameChange.NoChange)
					{
						Pawn pawn;
						Pawn pawn2;
						SpouseRelationUtility.DetermineManAndWomanSpouses(initiator, recipient, out pawn, out pawn2);
						stringBuilder.AppendLine();
						if (initiator.relations.nextMarriageNameChange == MarriageNameChange.MansName)
						{
							stringBuilder.AppendLine("LetterAcceptedProposal_NameChange".Translate(pawn2.Named("PAWN"), (pawn.Name as NameTriple).Last));
						}
						else
						{
							stringBuilder.AppendLine("LetterAcceptedProposal_NameChange".Translate(pawn.Named("PAWN"), (pawn2.Name as NameTriple).Last));
						}
					}
				}
				else
				{
					letterLabel = "LetterLabelRejectedProposal".Translate();
					letterDef = LetterDefOf.NegativeEvent;
					stringBuilder.AppendLine("LetterRejectedProposal".Translate(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT")));
					if (flag2)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2")));
					}
				}
				letterText = stringBuilder.ToString().TrimEndNewlines();
				lookTargets = new LookTargets(new TargetInfo[]
				{
					initiator,
					recipient
				});
				return;
			}
			letterLabel = null;
			letterText = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0016DE16 File Offset: 0x0016C016
		public float AcceptanceChance(Pawn initiator, Pawn recipient)
		{
			return Mathf.Clamp01(0.9f * Mathf.Clamp01(GenMath.LerpDouble(-20f, 60f, 0f, 1f, (float)recipient.relations.OpinionOf(initiator))));
		}

		// Token: 0x040026D3 RID: 9939
		private const float BaseSelectionWeight = 0.4f;

		// Token: 0x040026D4 RID: 9940
		private const float BaseAcceptanceChance = 0.9f;

		// Token: 0x040026D5 RID: 9941
		private const float BreakupChanceOnRejection = 0.4f;
	}
}
