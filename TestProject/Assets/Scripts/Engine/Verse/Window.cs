﻿using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public abstract class Window
	{
		
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x000B6823 File Offset: 0x000B4A23
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		
		// (get) Token: 0x06001DBB RID: 7611 RVA: 0x000B6A2F File Offset: 0x000B4C2F
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06001DBD RID: 7613 RVA: 0x000B6A36 File Offset: 0x000B4C36
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
		}

		
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		
		public abstract void DoWindowContents(Rect inRect);

		
		public virtual void ExtraOnGUI()
		{
		}

		
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

		
		public virtual void PreClose()
		{
		}

		
		public virtual void PostClose()
		{
		}

		
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

		
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		
		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		
		public WindowLayer layer = WindowLayer.Dialog;

		
		public string optionalTitle;

		
		public bool doCloseX;

		
		public bool doCloseButton;

		
		public bool closeOnAccept = true;

		
		public bool closeOnCancel = true;

		
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		
		public bool closeOnClickedOutside;

		
		public bool forcePause;

		
		public bool preventCameraMotion = true;

		
		public bool preventDrawTutor;

		
		public bool doWindowBackground = true;

		
		public bool onlyOneOfTypeAllowed = true;

		
		public bool absorbInputAroundWindow;

		
		public bool resizeable;

		
		public bool draggable;

		
		public bool drawShadow = true;

		
		public bool focusWhenOpened = true;

		
		public float shadowAlpha = 1f;

		
		public SoundDef soundAppear;

		
		public SoundDef soundClose;

		
		public SoundDef soundAmbient;

		
		public bool silenceAmbientSound;

		
		public const float StandardMargin = 18f;

		
		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		
		public int ID;

		
		public Rect windowRect;

		
		private Sustainer sustainerAmbient;

		
		private WindowResizer resizer;

		
		private bool resizeLater;

		
		private Rect resizeLaterRect;

		
		private string onGUIProfilerLabelCached;

		
		public string extraOnGUIProfilerLabelCached;

		
		private GUI.WindowFunction innerWindowOnGUICached;
	}
}
