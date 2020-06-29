using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		
		// (get) Token: 0x06005D11 RID: 23825
		protected abstract bool AnythingInClipboard { get; }

		
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

		
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		
		protected abstract void CopyFrom(Pawn p);

		
		protected abstract void PasteTo(Pawn p);
	}
}
