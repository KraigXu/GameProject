using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200053B RID: 1339
	public static class JobUtility
	{
		// Token: 0x06002653 RID: 9811 RVA: 0x000E1D74 File Offset: 0x000DFF74
		public static void TryStartErrorRecoverJob(Pawn pawn, string message, Exception exception = null, JobDriver concreteDriver = null)
		{
			string text = message;
			JobUtility.AppendVarsInfoToDebugMessage(pawn, ref text, concreteDriver);
			if (exception != null)
			{
				text = text + "\n" + exception;
			}
			Log.Error(text, false);
			if (pawn.jobs != null)
			{
				if (pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Errored, false, true);
				}
				if (JobUtility.startingErrorRecoverJob)
				{
					Log.Error("An error occurred while starting an error recover job. We have to stop now to avoid infinite loops. This means that the pawn is now jobless which can cause further bugs. pawn=" + pawn.ToStringSafe<Pawn>(), false);
					return;
				}
				JobUtility.startingErrorRecoverJob = true;
				try
				{
					pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 150, false), JobCondition.None, null, false, true, null, null, false, false);
				}
				finally
				{
					JobUtility.startingErrorRecoverJob = false;
				}
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000E1E30 File Offset: 0x000E0030
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000E1E43 File Offset: 0x000E0043
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, b, LocalTargetInfo.Invalid);
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000E1E54 File Offset: 0x000E0054
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b, LocalTargetInfo c)
		{
			string aText;
			object aObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(a, out aText, out aObj);
			string bText;
			object bObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(b, out bText, out bObj);
			string cText;
			object cObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(c, out cText, out cObj);
			return JobUtility.GetResolvedJobReportRaw(baseText, aText, aObj, bText, bObj, cText, cObj);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000E1E90 File Offset: 0x000E0090
		public static string GetResolvedJobReportRaw(string baseText, string aText, object aObj, string bText, object bObj, string cText, object cObj)
		{
			baseText = baseText.Formatted(aObj.Named("TargetA"), bObj.Named("TargetB"), cObj.Named("TargetC"));
			baseText = baseText.Replace("TargetA", aText);
			baseText = baseText.Replace("TargetB", bText);
			baseText = baseText.Replace("TargetC", cText);
			return baseText;
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000E1EFC File Offset: 0x000E00FC
		private static void AppendVarsInfoToDebugMessage(Pawn pawn, ref string msg, JobDriver concreteDriver)
		{
			if (concreteDriver != null)
			{
				msg = string.Concat(new object[]
				{
					msg,
					" driver=",
					concreteDriver.GetType().Name,
					" (toilIndex=",
					concreteDriver.CurToilIndex,
					")"
				});
				if (concreteDriver.job != null)
				{
					msg = msg + " driver.job=(" + concreteDriver.job.ToStringSafe<Job>() + ")";
					return;
				}
			}
			else if (pawn.jobs != null)
			{
				if (pawn.jobs.curDriver != null)
				{
					msg = string.Concat(new object[]
					{
						msg,
						" curDriver=",
						pawn.jobs.curDriver.GetType().Name,
						" (toilIndex=",
						pawn.jobs.curDriver.CurToilIndex,
						")"
					});
				}
				if (pawn.jobs.curJob != null)
				{
					msg = msg + " curJob=(" + pawn.jobs.curJob.ToStringSafe<Job>() + ")";
				}
			}
		}

		// Token: 0x0400171B RID: 5915
		private static bool startingErrorRecoverJob;
	}
}
