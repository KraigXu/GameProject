using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B41 RID: 2881
	public class InteractionWorker_Breakup : InteractionWorker
	{
		// Token: 0x060043C4 RID: 17348 RVA: 0x0016D298 File Offset: 0x0016B498
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
			{
				return 0f;
			}
			float num = Mathf.InverseLerp(100f, -100f, (float)initiator.relations.OpinionOf(recipient));
			float num2 = 1f;
			if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Spouse, recipient))
			{
				num2 = 0.4f;
			}
			return 0.02f * num * num2;
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0016D2FC File Offset: 0x0016B4FC
		public Thought RandomBreakupReason(Pawn initiator, Pawn recipient)
		{
			if (initiator.needs.mood == null)
			{
				return null;
			}
			List<Thought_Memory> list = (from m in initiator.needs.mood.thoughts.memories.Memories
			where m != null && m.otherPawn == recipient && m.CurStage != null && m.CurStage.baseOpinionOffset < 0f
			select m).ToList<Thought_Memory>();
			if (list.Count == 0)
			{
				return null;
			}
			float worstMemoryOpinionOffset = list.Max((Thought_Memory m) => -m.CurStage.baseOpinionOffset);
			Thought_Memory result = null;
			(from m in list
			where -m.CurStage.baseOpinionOffset >= worstMemoryOpinionOffset / 2f
			select m).TryRandomElementByWeight((Thought_Memory m) => -m.CurStage.baseOpinionOffset, out result);
			return result;
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x0016D3C8 File Offset: 0x0016B5C8
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			Thought thought = this.RandomBreakupReason(initiator, recipient);
			bool flag = false;
			bool flag2 = false;
			if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Spouse, recipient))
			{
				initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, recipient);
				if (recipient.needs.mood != null)
				{
					recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DivorcedMe, initiator);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
					recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, initiator);
				}
				if (recipient.needs.mood != null)
				{
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
					initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, recipient);
				}
				flag = SpouseRelationUtility.ChangeNameAfterDivorce(initiator, -1f);
				flag2 = SpouseRelationUtility.ChangeNameAfterDivorce(recipient, -1f);
			}
			else
			{
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.Fiance, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
				if (recipient.needs.mood != null)
				{
					recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BrokeUpWithMe, initiator);
				}
			}
			if (initiator.ownership.OwnedBed != null && initiator.ownership.OwnedBed == recipient.ownership.OwnedBed)
			{
				((Rand.Value < 0.5f) ? initiator : recipient).ownership.UnclaimBed();
			}
			TaleRecorder.RecordTale(TaleDefOf.Breakup, new object[]
			{
				initiator,
				recipient
			});
			if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator.LabelShort, recipient.LabelShort, initiator.Named("PAWN1"), recipient.Named("PAWN2")));
				stringBuilder.AppendLine();
				if (flag)
				{
					stringBuilder.Append("LetterNoLongerLovers_BackToBirthName".Translate(initiator.Named("PAWN")));
				}
				if (flag2)
				{
					if (flag)
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append("LetterNoLongerLovers_BackToBirthName".Translate(recipient.Named("PAWN")));
				}
				if (flag || flag2)
				{
					stringBuilder.AppendLine();
				}
				if (thought != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("FinalStraw".Translate(thought.LabelCap));
				}
				letterLabel = "LetterLabelBreakup".Translate();
				letterText = stringBuilder.ToString().TrimEndNewlines();
				letterDef = LetterDefOf.NegativeEvent;
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

		// Token: 0x040026CB RID: 9931
		private const float BaseChance = 0.02f;

		// Token: 0x040026CC RID: 9932
		private const float SpouseRelationChanceFactor = 0.4f;
	}
}
