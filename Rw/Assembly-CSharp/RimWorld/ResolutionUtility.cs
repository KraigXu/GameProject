using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E65 RID: 3685
	public static class ResolutionUtility
	{
		// Token: 0x17001012 RID: 4114
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

		// Token: 0x0600594C RID: 22860 RVA: 0x001DDF1C File Offset: 0x001DC11C
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

		// Token: 0x0600594D RID: 22861 RVA: 0x001DDF98 File Offset: 0x001DC198
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

		// Token: 0x0600594E RID: 22862 RVA: 0x001DDFD0 File Offset: 0x001DC1D0
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

		// Token: 0x0600594F RID: 22863 RVA: 0x001DE007 File Offset: 0x001DC207
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x001DE024 File Offset: 0x001DC224
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

		// Token: 0x06005951 RID: 22865 RVA: 0x001DE098 File Offset: 0x001DC298
		public static void SetNativeResolutionRaw()
		{
			Resolution nativeResolution = ResolutionUtility.NativeResolution;
			ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, true);
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x001DE0C0 File Offset: 0x001DC2C0
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

		// Token: 0x06005953 RID: 22867 RVA: 0x001DE150 File Offset: 0x001DC350
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

		// Token: 0x04003054 RID: 12372
		public const int MinResolutionWidth = 1024;

		// Token: 0x04003055 RID: 12373
		public const int MinResolutionHeight = 768;

		// Token: 0x04003056 RID: 12374
		public const int MinRecommendedResolutionWidth = 1700;

		// Token: 0x04003057 RID: 12375
		public const int MinRecommendedResolutionHeight = 910;
	}
}
