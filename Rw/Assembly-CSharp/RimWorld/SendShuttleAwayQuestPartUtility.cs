using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098D RID: 2445
	public static class SendShuttleAwayQuestPartUtility
	{
		// Token: 0x060039E0 RID: 14816 RVA: 0x00133694 File Offset: 0x00131894
		public static void SendAway(Thing shuttle, bool dropEverything)
		{
			CompShuttle compShuttle = shuttle.TryGetComp<CompShuttle>();
			CompTransporter compTransporter = shuttle.TryGetComp<CompTransporter>();
			if (shuttle.Spawned)
			{
				if (dropEverything && compTransporter.LoadingInProgressOrReadyToLaunch)
				{
					compTransporter.CancelLoad();
				}
				if (!compTransporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(compTransporter));
				}
				compShuttle.Send();
				return;
			}
			if (shuttle.ParentHolder is Thing && ((Thing)shuttle.ParentHolder).def == ThingDefOf.ShuttleIncoming)
			{
				compShuttle.leaveASAP = true;
			}
		}
	}
}
