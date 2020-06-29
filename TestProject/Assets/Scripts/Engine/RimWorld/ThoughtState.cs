using System;
using UnityEngine;

namespace RimWorld
{
	
	public struct ThoughtState
	{
		
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x0011E347 File Offset: 0x0011C547
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x0011E359 File Offset: 0x0011C559
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x0011E361 File Offset: 0x0011C561
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x0011E369 File Offset: 0x0011C569
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		
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

		
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		
		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			if (!this.Active)
			{
				return false;
			}
			int num = this.StageIndexFor(thoughtDef);
			return num >= 0 && thoughtDef.stages[num] != null;
		}

		
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		
		private int stageIndex;

		
		private string reason;

		
		private const int InactiveIndex = -99999;
	}
}
