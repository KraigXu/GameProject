using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E3 RID: 227
	public class RoomStatDef : Def
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001DADF File Offset: 0x0001BCDF
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

		// Token: 0x06000637 RID: 1591 RVA: 0x0001DB11 File Offset: 0x0001BD11
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001DB34 File Offset: 0x0001BD34
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

		// Token: 0x06000639 RID: 1593 RVA: 0x0001DB88 File Offset: 0x0001BD88
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		// Token: 0x0400054C RID: 1356
		public Type workerClass;

		// Token: 0x0400054D RID: 1357
		public float updatePriority;

		// Token: 0x0400054E RID: 1358
		public bool displayRounded;

		// Token: 0x0400054F RID: 1359
		public bool isHidden;

		// Token: 0x04000550 RID: 1360
		public float roomlessScore;

		// Token: 0x04000551 RID: 1361
		public List<RoomStatScoreStage> scoreStages;

		// Token: 0x04000552 RID: 1362
		public RoomStatDef inputStat;

		// Token: 0x04000553 RID: 1363
		public SimpleCurve curve;

		// Token: 0x04000554 RID: 1364
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}
