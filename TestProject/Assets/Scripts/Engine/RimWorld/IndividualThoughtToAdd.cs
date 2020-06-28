using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B0D RID: 2829
	public struct IndividualThoughtToAdd
	{
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600429D RID: 17053 RVA: 0x00163DA4 File Offset: 0x00161FA4
		public string LabelCap
		{
			get
			{
				string text = this.thought.LabelCap;
				float num = this.thought.MoodOffset();
				if (num != 0f)
				{
					text = text + " " + Mathf.RoundToInt(num).ToStringWithSign();
				}
				return text;
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x00163DEC File Offset: 0x00161FEC
		public IndividualThoughtToAdd(ThoughtDef thoughtDef, Pawn addTo, Pawn otherPawn = null, float moodPowerFactor = 1f, float opinionOffsetFactor = 1f)
		{
			this.addTo = addTo;
			this.otherPawn = otherPawn;
			this.thought = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			this.thought.moodPowerFactor = moodPowerFactor;
			this.thought.otherPawn = otherPawn;
			this.thought.pawn = addTo;
			Thought_MemorySocial thought_MemorySocial = this.thought as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= opinionOffsetFactor;
			}
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00163E5C File Offset: 0x0016205C
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		// Token: 0x04002646 RID: 9798
		public Thought_Memory thought;

		// Token: 0x04002647 RID: 9799
		public Pawn addTo;

		// Token: 0x04002648 RID: 9800
		private Pawn otherPawn;
	}
}
