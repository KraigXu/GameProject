using System;
using Verse;

namespace RimWorld
{
	
	public struct RoyalTitleInheritanceOutcome
	{
		
		
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		
		
		public bool HeirHasTitle
		{
			get
			{
				return this.heirCurrentTitle != null;
			}
		}

		
		public Pawn heir;

		
		public RoyalTitleDef heirCurrentTitle;

		
		public bool heirTitleHigher;
	}
}
