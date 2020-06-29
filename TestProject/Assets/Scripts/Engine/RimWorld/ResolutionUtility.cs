using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class ResolutionUtility
	{
		
		// (get) Token: 0x0600594B RID: 22859 RVA: 0x001DDEA0 File Offset: 0x001DC0A0
		public static Resolution NativeResolution
		{
			get
			{
				Resolution[] resolutions = Screen.resolutions;
				if (resolutions.Length == 0)
				{
					return Screen.currentResolution;
				}
				Resolution result = resolutions[0];
				for (int i = 1; i < resolutions.Length; i++)
				{
					if (resolutions[i].width > result.width || (resolutions[i].width == result.width && resolutions[i].height > result.height))
					{
						result = resolutions[i];
					}
				}
				return result;
			}
		}

		
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width == res.width && Screen.height == res.height)
			{
				return;
			}
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			ResolutionUtility.SetResolutionRaw(res.width, res.height, Screen.fullScreen);
			Prefs.ScreenWidth = res.width;
			Prefs.ScreenHeight = res.height;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
		}

		
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen == fullScreen)
			{
				return;
			}
			bool fullScreen2 = Screen.fullScreen;
			Screen.fullScreen = fullScreen;
			Prefs.FullScreen = fullScreen;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
		}

		
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale == newScale)
			{
				return;
			}
			float uiscale = Prefs.UIScale;
			Prefs.UIScale = newScale;
			GenUI.ClearLabelWidthCache();
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
		}

		
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		
		public static void SetResolutionRaw(int w, int h, bool fullScreen)
		{
			if (Application.isBatchMode)
			{
				return;
			}
			if (w <= 0 || h <= 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to set resolution to ",
					w,
					"x",
					h
				}), false);
				return;
			}
			if (Screen.width != w || Screen.height != h || Screen.fullScreen != fullScreen)
			{
				Screen.SetResolution(w, h, fullScreen);
			}
		}

		
		public static void SetNativeResolutionRaw()
		{
			Resolution nativeResolution = ResolutionUtility.NativeResolution;
			ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, true);
		}

		
		public static float GetRecommendedUIScale(int screenWidth, int screenHeight)
		{
			if (screenWidth == 0 || screenHeight == 0)
			{
				Resolution nativeResolution = ResolutionUtility.NativeResolution;
				screenWidth = nativeResolution.width;
				screenHeight = nativeResolution.height;
			}
			if (screenWidth <= 1024 || screenHeight <= 768)
			{
				return 1f;
			}
			for (int i = Dialog_Options.UIScales.Length - 1; i >= 0; i--)
			{
				int num = Mathf.FloorToInt((float)screenWidth / Dialog_Options.UIScales[i]);
				int num2 = Mathf.FloorToInt((float)screenHeight / Dialog_Options.UIScales[i]);
				if (num >= 1700 && num2 >= 910)
				{
					return Dialog_Options.UIScales[i];
				}
			}
			return 1f;
		}

		
		public static void Update()
		{
			if (RealTime.frameCount % 30 == 0 && !LongEventHandler.AnyEventNowOrWaiting && !Screen.fullScreen)
			{
				bool flag = false;
				if (Screen.width != Prefs.ScreenWidth)
				{
					Prefs.ScreenWidth = Screen.width;
					flag = true;
				}
				if (Screen.height != Prefs.ScreenHeight)
				{
					Prefs.ScreenHeight = Screen.height;
					flag = true;
				}
				if (flag)
				{
					Prefs.Save();
				}
			}
		}

		
		public const int MinResolutionWidth = 1024;

		
		public const int MinResolutionHeight = 768;

		
		public const int MinRecommendedResolutionWidth = 1700;

		
		public const int MinRecommendedResolutionHeight = 910;
	}
}
