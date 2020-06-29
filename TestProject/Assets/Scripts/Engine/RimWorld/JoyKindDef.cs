using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class JoyKindDef : Def
	{
		
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

		
		public List<RoyalTitleDef> titleRequiredAny;

		
		public bool needsThing = true;
	}
}
