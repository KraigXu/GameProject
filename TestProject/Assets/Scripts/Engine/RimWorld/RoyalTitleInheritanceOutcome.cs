using System;
using Verse;

namespace RimWorld
{
	
	public struct RoyalTitleInheritanceOutcome
	{
		
		// (get) Token: 0x06006340 RID: 25408 RVA: 0x00227EE4 File Offset: 0x002260E4
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		
		// (get) Token: 0x06006341 RID: 25409 RVA: 0x00227EEF File Offset: 0x002260EF
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
