using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103A RID: 4154
	public struct RoyalTitleInheritanceOutcome
	{
		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x06006340 RID: 25408 RVA: 0x00227EE4 File Offset: 0x002260E4
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x06006341 RID: 25409 RVA: 0x00227EEF File Offset: 0x002260EF
		public bool HeirHasTitle
		{
			get
			{
				return this.heirCurrentTitle != null;
			}
		}

		// Token: 0x04003C62 RID: 15458
		public Pawn heir;

		// Token: 0x04003C63 RID: 15459
		public RoyalTitleDef heirCurrentTitle;

		// Token: 0x04003C64 RID: 15460
		public bool heirTitleHigher;
	}
}
