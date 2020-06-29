﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public struct IndividualThoughtToAdd
	{
		
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

		
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		
		public Thought_Memory thought;

		
		public Pawn addTo;

		
		private Pawn otherPawn;
	}
}
