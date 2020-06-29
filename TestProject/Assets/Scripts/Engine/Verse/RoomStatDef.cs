﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class RoomStatDef : Def
	{
		
		
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		
		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			int num = 0;
			while (num < this.scoreStages.Count && score >= this.scoreStages[num].minScore)
			{
				result = num;
				num++;
			}
			return result;
		}

		
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		
		public Type workerClass;

		
		public float updatePriority;

		
		public bool displayRounded;

		
		public bool isHidden;

		
		public float roomlessScore;

		
		public List<RoomStatScoreStage> scoreStages;

		
		public RoomStatDef inputStat;

		
		public SimpleCurve curve;

		
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}
