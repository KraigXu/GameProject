using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED5 RID: 3797
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06005D11 RID: 23825
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x06005D12 RID: 23826 RVA: 0x002043A8 File Offset: 0x002025A8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			if (this.AnythingInClipboard)
			{
				pasteAction = delegate
				{
					this.PasteTo(pawn);
				};
			}
			CopyPasteUI.DoCopyPasteButtons(new Rect(rect.x, rect.y, 36f, 30f), delegate
			{
				this.CopyFrom(pawn);
			}, pasteAction);
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x0020440F File Offset: 0x0020260F
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x0020405E File Offset: 0x0020225E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06005D15 RID: 23829
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x06005D16 RID: 23830
		protected abstract void PasteTo(Pawn p);
	}
}
