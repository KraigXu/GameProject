using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003F0 RID: 1008
	public class WindowStack
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x000B715C File Offset: 0x000B535C
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x170005A0 RID: 1440
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x000B7177 File Offset: 0x000B5377
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x000B7184 File Offset: 0x000B5384
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x000B718C File Offset: 0x000B538C
		public bool WindowsForcePause
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].forcePause)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x000B71C8 File Offset: 0x000B53C8
		public bool WindowsPreventCameraMotion
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventCameraMotion)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x000B7204 File Offset: 0x000B5404
		public bool WindowsPreventDrawTutor
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventDrawTutor)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x000B723D File Offset: 0x000B543D
		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				if (this.gameStartDialogOpen)
				{
					return 0f;
				}
				if (this.timeGameStartDialogClosed < 0f)
				{
					return 9999999f;
				}
				return Time.time - this.timeGameStartDialogClosed;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x000B726C File Offset: 0x000B546C
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x000B7284 File Offset: 0x000B5484
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x000B7294 File Offset: 0x000B5494
		public bool NonImmediateDialogWindowOpen
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (!(this.windows[i] is ImmediateWindow) && this.windows[i].layer == WindowLayer.Dialog)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000B72E4 File Offset: 0x000B54E4
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000B7320 File Offset: 0x000B5520
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null && this.CloseWindowsBecauseClicked(null))
			{
				Event.current.Use();
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				this.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				this.Notify_PressedAccept();
			}
			if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.KeyDown) && !this.GetsInput(null))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000B73B4 File Offset: 0x000B55B4
		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = this.windowStackOnGUITmpList.Count - 1; i >= 0; i--)
			{
				this.windowStackOnGUITmpList[i].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int j = 0; j < this.windowStackOnGUITmpList.Count; j++)
			{
				if (this.windowStackOnGUITmpList[j].drawShadow)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[j].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[j].windowRect);
					GUI.color = Color.white;
				}
				this.windowStackOnGUITmpList[j].WindowOnGUI();
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000B74C4 File Offset: 0x000B56C4
		public void Notify_ClickedInsideWindow(Window window)
		{
			if (this.GetsInput(window))
			{
				this.windows.Remove(window);
				this.InsertAtCorrectPositionInList(window);
				this.focusedWindow = window;
			}
			else
			{
				Event.current.Use();
			}
			this.CloseWindowsBecauseClicked(window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000B7510 File Offset: 0x000B5710
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000B7520 File Offset: 0x000B5720
		public void Notify_PressedCancel()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnCancel || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnCancelKeyPressed();
					return;
				}
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000B7594 File Offset: 0x000B5794
		public void Notify_PressedAccept()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnAccept || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnAcceptKeyPressed();
					return;
				}
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000B7605 File Offset: 0x000B5805
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000B760E File Offset: 0x000B580E
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000B7624 File Offset: 0x000B5824
		public bool IsOpen<WindowType>()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000B7660 File Offset: 0x000B5860
		public bool IsOpen(Type type)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x000B769F File Offset: 0x000B589F
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000B76B0 File Offset: 0x000B58B0
		public WindowType WindowOfType<WindowType>() where WindowType : class
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return this.windows[i] as WindowType;
				}
			}
			return default(WindowType);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000B7708 File Offset: 0x000B5908
		public bool GetsInput(Window window)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					return true;
				}
				if (this.windows[i].absorbInputAroundWindow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000B7754 File Offset: 0x000B5954
		public void Add(Window window)
		{
			this.RemoveWindowsOfType(window.GetType());
			window.ID = WindowStack.uniqueWindowID++;
			window.PreOpen();
			this.InsertAtCorrectPositionInList(window);
			this.FocusAfterInsertIfShould(window);
			this.updateInternalWindowsOrderLater = true;
			window.PostOpen();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000B77A4 File Offset: 0x000B59A4
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (ID == 0)
			{
				Log.Warning("Used 0 as immediate window ID.", false);
				return;
			}
			ID = -Math.Abs(ID);
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].ID == ID)
				{
					ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[i];
					immediateWindow.windowRect = rect;
					immediateWindow.doWindowFunc = doWindowFunc;
					immediateWindow.layer = layer;
					immediateWindow.doWindowBackground = doBackground;
					immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
					immediateWindow.shadowAlpha = shadowAlpha;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha);
			}
			this.immediateWindowsRequests.Add(ID);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000B7868 File Offset: 0x000B5A68
		public bool TryRemove(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == windowType)
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000B78BC File Offset: 0x000B5ABC
		public bool TryRemoveAssignableFromType(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (windowType.IsAssignableFrom(this.windows[i].GetType()))
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000B7910 File Offset: 0x000B5B10
		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] == window)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (doCloseSound && window.soundClose != null)
			{
				window.soundClose.PlayOneShotOnCamera(null);
			}
			window.PreClose();
			this.windows.Remove(window);
			window.PostClose();
			if (this.focusedWindow == window)
			{
				if (this.windows.Count > 0)
				{
					this.focusedWindow = this.windows[this.windows.Count - 1];
				}
				else
				{
					this.focusedWindow = null;
				}
				this.updateInternalWindowsOrderLater = true;
			}
			return true;
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000B79C4 File Offset: 0x000B5BC4
		public Window GetWindowAt(Vector2 pos)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i].windowRect.Contains(pos))
				{
					return this.windows[i];
				}
			}
			return null;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000B7A10 File Offset: 0x000B5C10
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.", false);
				return;
			}
			ImmediateWindow immediateWindow = new ImmediateWindow();
			immediateWindow.ID = ID;
			immediateWindow.layer = layer;
			immediateWindow.doWindowFunc = doWindowFunc;
			immediateWindow.doWindowBackground = doBackground;
			immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
			immediateWindow.shadowAlpha = shadowAlpha;
			immediateWindow.PreOpen();
			immediateWindow.windowRect = rect;
			this.InsertAtCorrectPositionInList(immediateWindow);
			this.FocusAfterInsertIfShould(immediateWindow);
			this.updateInternalWindowsOrderLater = true;
			immediateWindow.PostOpen();
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000B7A8C File Offset: 0x000B5C8C
		private void UpdateImmediateWindowsList()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			this.updateImmediateWindowsListTmpList.Clear();
			this.updateImmediateWindowsListTmpList.AddRange(this.windows);
			for (int i = 0; i < this.updateImmediateWindowsListTmpList.Count; i++)
			{
				if (this.IsImmediateWindow(this.updateImmediateWindowsListTmpList[i]))
				{
					bool flag = false;
					for (int j = 0; j < this.immediateWindowsRequests.Count; j++)
					{
						if (this.immediateWindowsRequests[j] == this.updateImmediateWindowsListTmpList[i].ID)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.TryRemove(this.updateImmediateWindowsListTmpList[i], true);
					}
				}
			}
			this.immediateWindowsRequests.Clear();
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000B7B4C File Offset: 0x000B5D4C
		private void InsertAtCorrectPositionInList(Window window)
		{
			int index = 0;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (window.layer >= this.windows[i].layer)
				{
					index = i + 1;
				}
			}
			this.windows.Insert(index, window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000B7BA4 File Offset: 0x000B5DA4
		private void FocusAfterInsertIfShould(Window window)
		{
			if (!window.focusWhenOpened)
			{
				return;
			}
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					this.focusedWindow = this.windows[i];
					this.updateInternalWindowsOrderLater = true;
					return;
				}
				if (this.windows[i] == this.focusedWindow)
				{
					break;
				}
			}
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000B7C10 File Offset: 0x000B5E10
		private void AdjustWindowsIfResolutionChanged()
		{
			IntVec2 a = new IntVec2(UI.screenWidth, UI.screenHeight);
			if (!UnityGUIBugsFixer.ResolutionsEqual(a, this.prevResolution))
			{
				this.prevResolution = a;
				for (int i = 0; i < this.windows.Count; i++)
				{
					this.windows[i].Notify_ResolutionChanged();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
			}
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000B7C7C File Offset: 0x000B5E7C
		private void RemoveWindowsOfType(Type type)
		{
			this.removeWindowsOfTypeTmpList.Clear();
			this.removeWindowsOfTypeTmpList.AddRange(this.windows);
			for (int i = 0; i < this.removeWindowsOfTypeTmpList.Count; i++)
			{
				if (this.removeWindowsOfTypeTmpList[i].onlyOneOfTypeAllowed && this.removeWindowsOfTypeTmpList[i].GetType() == type)
				{
					this.TryRemove(this.removeWindowsOfTypeTmpList[i], true);
				}
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000B7CFC File Offset: 0x000B5EFC
		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			int num = this.closeWindowsTmpList.Count - 1;
			while (num >= 0 && this.closeWindowsTmpList[num] != clickedWindow)
			{
				if (this.closeWindowsTmpList[num].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[num], true);
				}
				num--;
			}
			return result;
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000B7D78 File Offset: 0x000B5F78
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000B7D84 File Offset: 0x000B5F84
		private void UpdateInternalWindowsOrder()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				GUI.BringWindowToFront(this.windows[i].ID);
			}
			if (this.focusedWindow != null)
			{
				GUI.FocusWindow(this.focusedWindow.ID);
			}
		}

		// Token: 0x04001257 RID: 4695
		public Window currentlyDrawnWindow;

		// Token: 0x04001258 RID: 4696
		private List<Window> windows = new List<Window>();

		// Token: 0x04001259 RID: 4697
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x0400125A RID: 4698
		private bool updateInternalWindowsOrderLater;

		// Token: 0x0400125B RID: 4699
		private Window focusedWindow;

		// Token: 0x0400125C RID: 4700
		private static int uniqueWindowID;

		// Token: 0x0400125D RID: 4701
		private bool gameStartDialogOpen;

		// Token: 0x0400125E RID: 4702
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x0400125F RID: 4703
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04001260 RID: 4704
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04001261 RID: 4705
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04001262 RID: 4706
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04001263 RID: 4707
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
