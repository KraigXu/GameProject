using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EF1 RID: 3825
	public abstract class PawnColumnWorker_Text : PawnColumnWorker
	{
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x06005DCC RID: 24012 RVA: 0x002065F0 File Offset: 0x002047F0
		protected virtual int Width
		{
			get
			{
				return this.def.width;
			}
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x00203F72 File Offset: 0x00202172
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x00206600 File Offset: 0x00204800
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, 30f));
			string textFor = this.GetTextFor(pawn);
			if (textFor != null)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Text.WordWrap = false;
				Widgets.Label(rect2, textFor);
				Text.WordWrap = true;
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect2))
				{
					string tip = this.GetTip(pawn);
					if (!tip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, tip);
					}
				}
			}
		}

		// Token: 0x06005DCF RID: 24015 RVA: 0x0020668E File Offset: 0x0020488E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06005DD0 RID: 24016 RVA: 0x002066A2 File Offset: 0x002048A2
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnColumnWorker_Text.comparer.Compare(this.GetTextFor(a), this.GetTextFor(a));
		}

		// Token: 0x06005DD1 RID: 24017
		protected abstract string GetTextFor(Pawn pawn);

		// Token: 0x06005DD2 RID: 24018 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x040032D4 RID: 13012
		private static NumericStringComparer comparer = new NumericStringComparer();
	}
}
