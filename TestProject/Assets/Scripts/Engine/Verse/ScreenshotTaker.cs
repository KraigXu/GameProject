using System;
using System.IO;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	
	public static class ScreenshotTaker
	{
		
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

		
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		
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

		
		private static int lastShotFrame = -999;

		
		private static int screenshotCount = 0;

		
		private static string lastShotFilePath;

		
		private static bool suppressMessage;

		
		private static bool takeScreenshot;
	}
}
