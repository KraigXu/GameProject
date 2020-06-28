using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED7 RID: 3799
	public class PawnColumnWorker_CopyPasteWorkPriorities : PawnColumnWorker_CopyPaste
	{
		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x06005D1D RID: 23837 RVA: 0x00204492 File Offset: 0x00202692
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteWorkPriorities.clipboard != null;
			}
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x0020449C File Offset: 0x0020269C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06005D1F RID: 23839 RVA: 0x002044C8 File Offset: 0x002026C8
		protected override void CopyFrom(Pawn p)
		{
			if (PawnColumnWorker_CopyPasteWorkPriorities.clipboard == null)
			{
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard = new DefMap<WorkTypeDef, int>();
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef] = ((!p.WorkTypeIsDisabled(workTypeDef)) ? p.workSettings.GetPriority(workTypeDef) : 3);
			}
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x00204528 File Offset: 0x00202728
		protected override void PasteTo(Pawn p)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (!p.WorkTypeIsDisabled(workTypeDef))
				{
					p.workSettings.SetPriority(workTypeDef, PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef]);
				}
			}
		}

		// Token: 0x040032BD RID: 12989
		private static DefMap<WorkTypeDef, int> clipboard;
	}
}
