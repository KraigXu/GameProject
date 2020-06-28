using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000FA RID: 250
	public class ToolCapacityDef : Def
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001FCAF File Offset: 0x0001DEAF
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where x.requiredCapacity == this
				select x;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001FCC7 File Offset: 0x0001DEC7
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}
	}
}
