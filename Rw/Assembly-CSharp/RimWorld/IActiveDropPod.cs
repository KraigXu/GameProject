using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB9 RID: 3257
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06004EF8 RID: 20216
		ActiveDropPodInfo Contents { get; }
	}
}
