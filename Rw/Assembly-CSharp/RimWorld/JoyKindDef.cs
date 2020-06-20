using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008DC RID: 2268
	public class JoyKindDef : Def
	{
		// Token: 0x0600365C RID: 13916 RVA: 0x00126A88 File Offset: 0x00124C88
		public bool PawnCanDo(Pawn pawn)
		{
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.conceited && royalTitle.def.JoyKindDisabled(this))
					{
						return false;
					}
				}
				if (this.titleRequiredAny == null)
				{
					return true;
				}
				bool flag = false;
				foreach (RoyalTitle royalTitle2 in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (this.titleRequiredAny.Contains(royalTitle2.def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x04001EC8 RID: 7880
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04001EC9 RID: 7881
		public bool needsThing = true;
	}
}
