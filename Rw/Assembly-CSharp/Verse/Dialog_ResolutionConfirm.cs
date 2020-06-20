using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DC RID: 988
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x000B44AE File Offset: 0x000B26AE
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x000B44C2 File Offset: 0x000B26C2
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000B44D3 File Offset: 0x000B26D3
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000B44FB File Offset: 0x000B26FB
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000B452A File Offset: 0x000B272A
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000B454F File Offset: 0x000B274F
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000B4580 File Offset: 0x000B2780
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string label = "ConfirmResolutionChange".Translate(Mathf.CeilToInt(this.TimeUntilRevert));
			Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
			if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), true, true, true))
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000B4688 File Offset: 0x000B2888
		private void Revert()
		{
			if (Prefs.LogVerbose)
			{
				Log.Message(string.Concat(new object[]
				{
					"Reverting screen settings to ",
					this.oldRes.x,
					"x",
					this.oldRes.z,
					", fs=",
					this.oldFullscreen.ToString()
				}), false);
			}
			ResolutionUtility.SetResolutionRaw(this.oldRes.x, this.oldRes.z, this.oldFullscreen);
			Prefs.FullScreen = this.oldFullscreen;
			Prefs.ScreenWidth = this.oldRes.x;
			Prefs.ScreenHeight = this.oldRes.z;
			Prefs.UIScale = this.oldUIScale;
			GenUI.ClearLabelWidthCache();
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000B4755 File Offset: 0x000B2955
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x040011C8 RID: 4552
		private float startTime;

		// Token: 0x040011C9 RID: 4553
		private IntVec2 oldRes;

		// Token: 0x040011CA RID: 4554
		private bool oldFullscreen;

		// Token: 0x040011CB RID: 4555
		private float oldUIScale;

		// Token: 0x040011CC RID: 4556
		private const float RevertTime = 10f;
	}
}
