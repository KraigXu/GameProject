using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103E RID: 4158
	public abstract class RoomRequirement
	{
		// Token: 0x06006361 RID: 25441
		public abstract bool Met(Room r, Pawn p = null);

		// Token: 0x06006362 RID: 25442 RVA: 0x002284C0 File Offset: 0x002266C0
		public virtual string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x06006363 RID: 25443 RVA: 0x002284D2 File Offset: 0x002266D2
		public string LabelCap(Room r = null)
		{
			return this.Label(r).CapitalizeFirst();
		}

		// Token: 0x06006364 RID: 25444 RVA: 0x002284E0 File Offset: 0x002266E0
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x06006365 RID: 25445 RVA: 0x002284E9 File Offset: 0x002266E9
		public virtual bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.GetType() == other.GetType();
		}

		// Token: 0x06006366 RID: 25446 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool PlayerHasResearched()
		{
			return true;
		}

		// Token: 0x04003C8D RID: 15501
		[NoTranslate]
		public string labelKey;
	}
}
