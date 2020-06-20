using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020007F9 RID: 2041
	public struct ThoughtState
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x0011E347 File Offset: 0x0011C547
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x0011E359 File Offset: 0x0011C559
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x0011E361 File Offset: 0x0011C561
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x0011E369 File Offset: 0x0011C569
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x0011E374 File Offset: 0x0011C574
		public static ThoughtState Inactive
		{
			get
			{
				return new ThoughtState
				{
					stageIndex = -99999
				};
			}
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0011E398 File Offset: 0x0011C598
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0011E3B8 File Offset: 0x0011C5B8
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x0011E3E0 File Offset: 0x0011C5E0
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x0011E3FC File Offset: 0x0011C5FC
		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0011E40C File Offset: 0x0011C60C
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			if (!this.Active)
			{
				return false;
			}
			int num = this.StageIndexFor(thoughtDef);
			return num >= 0 && thoughtDef.stages[num] != null;
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0011E440 File Offset: 0x0011C640
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		// Token: 0x04001BAE RID: 7086
		private int stageIndex;

		// Token: 0x04001BAF RID: 7087
		private string reason;

		// Token: 0x04001BB0 RID: 7088
		private const int InactiveIndex = -99999;
	}
}
