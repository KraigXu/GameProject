using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003EE RID: 1006
	public abstract class Window
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x000B6823 File Offset: 0x000B4A23
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001DBB RID: 7611 RVA: 0x000B6A2F File Offset: 0x000B4C2F
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001DBD RID: 7613 RVA: 0x000B6A36 File Offset: 0x000B4C36
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000B6A44 File Offset: 0x000B4C44
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000B6B0D File Offset: 0x000B4D0D
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x06001DC0 RID: 7616
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x000B6B24 File Offset: 0x000B4D24
		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			if (this.layer == WindowLayer.Dialog)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.DesignatorManager.Dragger.EndDrag();
					Find.DesignatorManager.Deselect();
					Find.Selector.Notify_DialogOpened();
				}
				if (Find.World != null)
				{
					Find.WorldSelector.Notify_DialogOpened();
				}
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000B6B7C File Offset: 0x000B4D7C
		public virtual void PostOpen()
		{
			if (this.soundAppear != null)
			{
				this.soundAppear.PlayOneShotOnCamera(null);
			}
			if (this.soundAmbient != null)
			{
				this.sustainerAmbient = this.soundAmbient.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
			}
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreClose()
		{
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostClose()
		{
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000B6BB4 File Offset: 0x000B4DB4
		public virtual void WindowOnGUI()
		{
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			this.windowRect = GUI.Window(this.ID, this.windowRect, this.innerWindowOnGUICached, "", Widgets.EmptyStyle);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x000B6C30 File Offset: 0x000B4E30
		private void InnerWindowOnGUI(int x)
		{
			Rect rect = this.windowRect.AtZero();
			UnityGUIBugsFixer.OnGUI();
			Find.WindowStack.currentlyDrawnWindow = this;
			if (this.doWindowBackground)
			{
				Widgets.DrawWindowBackground(rect);
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedAccept();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Find.WindowStack.Notify_ClickedInsideWindow(this);
			}
			if (Event.current.type == EventType.KeyDown && !Find.WindowStack.GetsInput(this))
			{
				Event.current.Use();
			}
			if (!this.optionalTitle.NullOrEmpty())
			{
				GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
			}
			if (this.doCloseX && Widgets.CloseButtonFor(rect))
			{
				this.Close(true);
			}
			if (this.resizeable && Event.current.type != EventType.Repaint)
			{
				Rect lhs = this.resizer.DoResizeControl(this.windowRect);
				if (lhs != this.windowRect)
				{
					this.resizeLater = true;
					this.resizeLaterRect = lhs;
				}
			}
			Rect rect2 = rect.ContractedBy(this.Margin);
			if (!this.optionalTitle.NullOrEmpty())
			{
				rect2.yMin += this.Margin + 25f;
			}
			GUI.BeginGroup(rect2);
			try
			{
				this.DoWindowContents(rect2.AtZero());
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception filling window for ",
					base.GetType(),
					": ",
					ex
				}), false);
			}
			GUI.EndGroup();
			if (this.resizeable && Event.current.type == EventType.Repaint)
			{
				this.resizer.DoResizeControl(this.windowRect);
			}
			if (this.doCloseButton)
			{
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(new Rect(rect.width / 2f - this.CloseButSize.x / 2f, rect.height - 55f, this.CloseButSize.x, this.CloseButSize.y), "CloseButton".Translate(), true, true, true))
				{
					this.Close(true);
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsOpen)
			{
				this.OnCancelKeyPressed();
			}
			if (this.draggable)
			{
				GUI.DragWindow();
			}
			else if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
			}
			ScreenFader.OverlayOnGUI(rect.size);
			Find.WindowStack.currentlyDrawnWindow = null;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000B6EE4 File Offset: 0x000B50E4
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000B6EF4 File Offset: 0x000B50F4
		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000B6F63 File Offset: 0x000B5163
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000B6F7E File Offset: 0x000B517E
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000B6F99 File Offset: 0x000B5199
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x04001231 RID: 4657
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x04001232 RID: 4658
		public string optionalTitle;

		// Token: 0x04001233 RID: 4659
		public bool doCloseX;

		// Token: 0x04001234 RID: 4660
		public bool doCloseButton;

		// Token: 0x04001235 RID: 4661
		public bool closeOnAccept = true;

		// Token: 0x04001236 RID: 4662
		public bool closeOnCancel = true;

		// Token: 0x04001237 RID: 4663
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04001238 RID: 4664
		public bool closeOnClickedOutside;

		// Token: 0x04001239 RID: 4665
		public bool forcePause;

		// Token: 0x0400123A RID: 4666
		public bool preventCameraMotion = true;

		// Token: 0x0400123B RID: 4667
		public bool preventDrawTutor;

		// Token: 0x0400123C RID: 4668
		public bool doWindowBackground = true;

		// Token: 0x0400123D RID: 4669
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x0400123E RID: 4670
		public bool absorbInputAroundWindow;

		// Token: 0x0400123F RID: 4671
		public bool resizeable;

		// Token: 0x04001240 RID: 4672
		public bool draggable;

		// Token: 0x04001241 RID: 4673
		public bool drawShadow = true;

		// Token: 0x04001242 RID: 4674
		public bool focusWhenOpened = true;

		// Token: 0x04001243 RID: 4675
		public float shadowAlpha = 1f;

		// Token: 0x04001244 RID: 4676
		public SoundDef soundAppear;

		// Token: 0x04001245 RID: 4677
		public SoundDef soundClose;

		// Token: 0x04001246 RID: 4678
		public SoundDef soundAmbient;

		// Token: 0x04001247 RID: 4679
		public bool silenceAmbientSound;

		// Token: 0x04001248 RID: 4680
		public const float StandardMargin = 18f;

		// Token: 0x04001249 RID: 4681
		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x0400124A RID: 4682
		public int ID;

		// Token: 0x0400124B RID: 4683
		public Rect windowRect;

		// Token: 0x0400124C RID: 4684
		private Sustainer sustainerAmbient;

		// Token: 0x0400124D RID: 4685
		private WindowResizer resizer;

		// Token: 0x0400124E RID: 4686
		private bool resizeLater;

		// Token: 0x0400124F RID: 4687
		private Rect resizeLaterRect;

		// Token: 0x04001250 RID: 4688
		private string onGUIProfilerLabelCached;

		// Token: 0x04001251 RID: 4689
		public string extraOnGUIProfilerLabelCached;

		// Token: 0x04001252 RID: 4690
		private GUI.WindowFunction innerWindowOnGUICached;
	}
}
