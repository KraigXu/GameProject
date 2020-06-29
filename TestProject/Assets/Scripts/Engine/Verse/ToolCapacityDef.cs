using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	
	public class ToolCapacityDef : Def
	{
		
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
