using System;
using Verse;

namespace RimWorld
{
	
	public interface IActiveDropPod : IThingHolder
	{
		
		// (get) Token: 0x06004EF8 RID: 20216
		ActiveDropPodInfo Contents { get; }
	}
}
