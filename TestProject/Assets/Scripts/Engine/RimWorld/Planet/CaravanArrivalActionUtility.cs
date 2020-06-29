using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public static class CaravanArrivalActionUtility
	{
		
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Caravan caravan, int pathDestination, WorldObject revalidateWorldClickTarget, Action<Action> confirmActionProxy = null) where T : CaravanArrivalAction
		{
			//CaravanArrivalActionUtility.c__DisplayClass0_0<T> c__DisplayClass0_ = new CaravanArrivalActionUtility.c__DisplayClass0_0<T>();
			//c__DisplayClass0_.acceptanceReportGetter = acceptanceReportGetter;
			//c__DisplayClass0_.caravan = caravan;
			//c__DisplayClass0_.pathDestination = pathDestination;
			//c__DisplayClass0_.arrivalActionGetter = arrivalActionGetter;
			//c__DisplayClass0_.confirmActionProxy = confirmActionProxy;
			//FloatMenuAcceptanceReport floatMenuAcceptanceReport = c__DisplayClass0_.acceptanceReportGetter();
			//if (floatMenuAcceptanceReport.Accepted || !floatMenuAcceptanceReport.FailReason.NullOrEmpty() || !floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			//{
			//	if (!floatMenuAcceptanceReport.FailReason.NullOrEmpty())
			//	{
			//		yield return new FloatMenuOption(label + " (" + floatMenuAcceptanceReport.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			//	}
			//	else
			//	{
			//		CaravanArrivalActionUtility.c__DisplayClass0_1<T> c__DisplayClass0_2 = new CaravanArrivalActionUtility.c__DisplayClass0_1<T>();
			//		c__DisplayClass0_2.CS$8__locals1 = c__DisplayClass0_;
			//		c__DisplayClass0_2.action = delegate
			//		{
			//			FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = c__DisplayClass0_2.CS$8__locals1.acceptanceReportGetter();
			//			if (floatMenuAcceptanceReport2.Accepted)
			//			{
			//				c__DisplayClass0_2.CS$8__locals1.caravan.pather.StartPath(c__DisplayClass0_2.CS$8__locals1.pathDestination, c__DisplayClass0_2.CS$8__locals1.arrivalActionGetter(), true, true);
			//				return;
			//			}
			//			if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
			//			{
			//				Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(c__DisplayClass0_2.CS$8__locals1.pathDestination), MessageTypeDefOf.RejectInput, false);
			//			}
			//		};
			//		yield return new FloatMenuOption(label, (c__DisplayClass0_2.CS$8__locals1.confirmActionProxy == null) ? c__DisplayClass0_2.action : delegate
			//		{
			//			c__DisplayClass0_2.CS$8__locals1.confirmActionProxy(c__DisplayClass0_2.action);
			//		}, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
			//		if (Prefs.DevMode)
			//		{
			//			yield return new FloatMenuOption(label + " (Dev: instantly)", delegate
			//			{
			//				FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = c__DisplayClass0_2.CS$8__locals1.acceptanceReportGetter();
			//				if (floatMenuAcceptanceReport2.Accepted)
			//				{
			//					c__DisplayClass0_2.CS$8__locals1.caravan.Tile = c__DisplayClass0_2.CS$8__locals1.pathDestination;
			//					c__DisplayClass0_2.CS$8__locals1.caravan.pather.StopDead();
			//					c__DisplayClass0_2.CS$8__locals1.arrivalActionGetter().Arrived(c__DisplayClass0_2.CS$8__locals1.caravan);
			//					return;
			//				}
			//				if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
			//				{
			//					Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(c__DisplayClass0_2.CS$8__locals1.pathDestination), MessageTypeDefOf.RejectInput, false);
			//				}
			//			}, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
			//		}
			//		c__DisplayClass0_2 = null;
			//	}
			//}
			yield break;
		}
	}
}
