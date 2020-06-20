using System;
using System.IO;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000474 RID: 1140
	public static class ScreenshotTaker
	{
		// Token: 0x060021AF RID: 8623 RVA: 0x000CD22C File Offset: 0x000CB42C
		public static void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (KeyBindingDefOf.TakeScreenshot.JustPressed || ScreenshotTaker.takeScreenshot)
			{
				ScreenshotTaker.TakeShot();
				ScreenshotTaker.takeScreenshot = false;
			}
			if (Time.frameCount == ScreenshotTaker.lastShotFrame + 1)
			{
				if (ScreenshotTaker.suppressMessage)
				{
					ScreenshotTaker.suppressMessage = false;
					return;
				}
				Messages.Message("MessageScreenshotSavedAs".Translate(ScreenshotTaker.lastShotFilePath), MessageTypeDefOf.TaskCompletion, false);
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000CD29F File Offset: 0x000CB49F
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000CD2B0 File Offset: 0x000CB4B0
		private static void TakeShot()
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				try
				{
					SteamScreenshots.TriggerScreenshot();
					return;
				}
				catch (Exception arg)
				{
					Log.Warning("Could not take Steam screenshot. Steam offline? Taking normal screenshot. Exception: " + arg, false);
					ScreenshotTaker.TakeNonSteamShot();
					return;
				}
			}
			ScreenshotTaker.TakeNonSteamShot();
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000CD300 File Offset: 0x000CB500
		private static void TakeNonSteamShot()
		{
			string screenshotFolderPath = GenFilePaths.ScreenshotFolderPath;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(screenshotFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				string text;
				do
				{
					ScreenshotTaker.screenshotCount++;
					text = string.Concat(new object[]
					{
						screenshotFolderPath,
						Path.DirectorySeparatorChar.ToString(),
						"screenshot",
						ScreenshotTaker.screenshotCount,
						".png"
					});
				}
				while (File.Exists(text));
				ScreenCapture.CaptureScreenshot(text);
				ScreenshotTaker.lastShotFrame = Time.frameCount;
				ScreenshotTaker.lastShotFilePath = text;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to save screenshot in " + screenshotFolderPath + "\nException follows:\n" + ex.ToString(), false);
			}
		}

		// Token: 0x0400149F RID: 5279
		private static int lastShotFrame = -999;

		// Token: 0x040014A0 RID: 5280
		private static int screenshotCount = 0;

		// Token: 0x040014A1 RID: 5281
		private static string lastShotFilePath;

		// Token: 0x040014A2 RID: 5282
		private static bool suppressMessage;

		// Token: 0x040014A3 RID: 5283
		private static bool takeScreenshot;
	}
}
