using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020009A3 RID: 2467
	public interface IIncidentMakerQuestPart
	{
		// Token: 0x06003A9A RID: 15002
		IEnumerable<FiringIncident> MakeIntervalIncidents();
	}
}
