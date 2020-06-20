using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000989 RID: 2441
	public abstract class QuestPart_RequirementsToAccept : QuestPart
	{
		// Token: 0x060039CB RID: 14795
		public abstract AcceptanceReport CanAccept();

		// Token: 0x060039CC RID: 14796 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanPawnAccept(Pawn p)
		{
			return true;
		}
	}
}
