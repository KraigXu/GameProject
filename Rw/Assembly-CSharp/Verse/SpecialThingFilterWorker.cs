using System;

namespace Verse
{
	// Token: 0x0200047D RID: 1149
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x060021E4 RID: 8676
		public abstract bool Matches(Thing t);

		// Token: 0x060021E5 RID: 8677 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
