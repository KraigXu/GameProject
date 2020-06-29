using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Thought_DecreeUnmet : Thought_Situational
	{
		
		// (get) Token: 0x060034C9 RID: 13513 RVA: 0x00120E40 File Offset: 0x0011F040
		public override string LabelCap
		{
			get
			{
				string text = base.LabelCap;
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					int num = this.TicksSinceQuestUnmet(questPart_SituationalThought);
					if (num > 0)
					{
						text = text + " (" + num.ToStringTicksToDays("F0") + ")";
					}
				}
				return text;
			}
		}

		
		// (get) Token: 0x060034CA RID: 13514 RVA: 0x00120E9C File Offset: 0x0011F09C
		public override string Description
		{
			get
			{
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					return base.Description.Formatted("(" + questPart_SituationalThought.quest.name + ")");
				}
				return "";
			}
		}

		
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
			if (questPart_SituationalThought == null)
			{
				return 0f;
			}
			float x = (float)this.TicksSinceQuestUnmet(questPart_SituationalThought) / 60000f;
			return (float)Mathf.RoundToInt(Thought_DecreeUnmet.MoodOffsetFromUnmetDaysCurve.Evaluate(x));
		}

		
		private int TicksSinceQuestUnmet(QuestPart_SituationalThought questPart)
		{
			return questPart.quest.TicksSinceAccepted - questPart.delayTicks;
		}

		
		private static readonly SimpleCurve MoodOffsetFromUnmetDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, -5f),
				true
			},
			{
				new CurvePoint(15f, -15f),
				true
			}
		};
	}
}
