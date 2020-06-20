using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000380 RID: 896
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000A3537 File Offset: 0x000A1737
		// (set) Token: 0x06001A89 RID: 6793 RVA: 0x000A353F File Offset: 0x000A173F
		public string Label
		{
			get
			{
				return this.labelInt;
			}
			set
			{
				if (value.NullOrEmpty())
				{
					value = "(missing label)";
				}
				this.labelInt = value.TrimEnd(Array.Empty<char>());
				this.SetSizeMode(this.sizeMode);
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x000A356D File Offset: 0x000A176D
		private float VerticalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 1f;
				}
				return 4f;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000A3583 File Offset: 0x000A1783
		private float HorizontalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 3f;
				}
				return 6f;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x000A3599 File Offset: 0x000A1799
		private float IconOffset
		{
			get
			{
				if (this.shownItem == null && !this.drawPlaceHolderIcon && !(this.itemIcon != null))
				{
					return 0f;
				}
				return 27f;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000A35C4 File Offset: 0x000A17C4
		private GameFont CurrentFont
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return GameFont.Tiny;
				}
				return GameFont.Small;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x000A35D2 File Offset: 0x000A17D2
		// (set) Token: 0x06001A8F RID: 6799 RVA: 0x000A35DD File Offset: 0x000A17DD
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
			set
			{
				if (value)
				{
					this.action = null;
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x000A35E9 File Offset: 0x000A17E9
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x000A35F1 File Offset: 0x000A17F1
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x000A35F9 File Offset: 0x000A17F9
		// (set) Token: 0x06001A93 RID: 6803 RVA: 0x000A360B File Offset: 0x000A180B
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return this.priorityInt;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label, false);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x000A3634 File Offset: 0x000A1834
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000A3698 File Offset: 0x000A1898
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget)
		{
			this.shownItem = shownItemForIcon;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000A36CC File Offset: 0x000A18CC
		public FloatMenuOption(string label, Action action, Texture2D itemIcon, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget)
		{
			this.itemIcon = itemIcon;
			this.iconColor = iconColor;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000A36FC File Offset: 0x000A18FC
		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			GameFont font = Text.Font;
			Text.Font = this.CurrentFont;
			float width = 300f - (2f * this.HorizontalMargin + 4f + this.extraPartWidth + this.IconOffset);
			this.cachedRequiredHeight = 2f * this.VerticalMargin + Text.CalcHeight(this.Label, width);
			this.cachedRequiredWidth = this.HorizontalMargin + 4f + Text.CalcSize(this.Label).x + this.extraPartWidth + this.HorizontalMargin + this.IconOffset + 4f;
			Text.Font = font;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000A37AC File Offset: 0x000A19AC
		public void Chosen(bool colonistOrdering, FloatMenu floatMenu)
		{
			if (floatMenu != null)
			{
				floatMenu.PreOptionChosen(this);
			}
			if (!this.Disabled)
			{
				if (this.action != null)
				{
					if (colonistOrdering)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
					return;
				}
			}
			else
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000A37F8 File Offset: 0x000A19F8
		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
		{
			Rect rect2 = rect;
			float height = rect2.height;
			rect2.height = height - 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			Rect rect3 = rect;
			rect3.xMin += 4f;
			rect3.xMax = rect.x + 27f;
			rect3.yMin += 4f;
			rect3.yMax = rect.y + 27f;
			if (flag)
			{
				rect3.x += 4f;
			}
			Rect rect4 = rect;
			rect4.xMin += this.HorizontalMargin;
			rect4.xMax -= this.HorizontalMargin;
			rect4.xMax -= 4f;
			rect4.xMax -= this.extraPartWidth + this.IconOffset;
			rect4.x += this.IconOffset;
			if (flag)
			{
				rect4.x += 4f;
			}
			Rect rect5 = default(Rect);
			if (this.extraPartWidth != 0f)
			{
				float num = Mathf.Min(Text.CalcSize(this.Label).x, rect4.width - 4f);
				rect5 = new Rect(rect4.xMin + num, rect4.yMin, this.extraPartWidth, 30f);
				flag2 = Mouse.IsOver(rect5);
			}
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect2);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag && !flag2)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			if (this.sizeMode == FloatMenuSizeMode.Tiny)
			{
				rect4.y += 1f;
			}
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, this.Label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = this.iconColor;
			if (this.shownItem != null || this.drawPlaceHolderIcon)
			{
				Widgets.DefIcon(rect3, this.shownItem, null, 1f, this.drawPlaceHolderIcon);
			}
			else if (this.itemIcon)
			{
				GUI.DrawTexture(rect3, this.itemIcon);
			}
			GUI.color = color;
			if (this.extraPartOnGUI != null)
			{
				bool flag3 = this.extraPartOnGUI(rect5);
				GUI.color = color;
				if (flag3)
				{
					return true;
				}
			}
			if (flag && this.mouseoverGuiAction != null)
			{
				this.mouseoverGuiAction();
			}
			if (this.tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
			}
			if (!Widgets.ButtonInvisible(rect2, true))
			{
				return false;
			}
			if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
			{
				return false;
			}
			this.Chosen(colonistOrdering, floatMenu);
			if (this.tutorTag != null)
			{
				TutorSystem.Notify_Event(this.tutorTag);
			}
			return true;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000A3B38 File Offset: 0x000A1D38
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"FloatMenuOption(",
				this.Label,
				", ",
				this.Disabled ? "disabled" : "enabled",
				")"
			});
		}

		// Token: 0x04000F9B RID: 3995
		private string labelInt;

		// Token: 0x04000F9C RID: 3996
		public Action action;

		// Token: 0x04000F9D RID: 3997
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x04000F9E RID: 3998
		public bool autoTakeable;

		// Token: 0x04000F9F RID: 3999
		public float autoTakeablePriority;

		// Token: 0x04000FA0 RID: 4000
		public Action mouseoverGuiAction;

		// Token: 0x04000FA1 RID: 4001
		public Thing revalidateClickTarget;

		// Token: 0x04000FA2 RID: 4002
		public WorldObject revalidateWorldClickTarget;

		// Token: 0x04000FA3 RID: 4003
		public float extraPartWidth;

		// Token: 0x04000FA4 RID: 4004
		public Func<Rect, bool> extraPartOnGUI;

		// Token: 0x04000FA5 RID: 4005
		public string tutorTag;

		// Token: 0x04000FA6 RID: 4006
		private FloatMenuSizeMode sizeMode;

		// Token: 0x04000FA7 RID: 4007
		private float cachedRequiredHeight;

		// Token: 0x04000FA8 RID: 4008
		private float cachedRequiredWidth;

		// Token: 0x04000FA9 RID: 4009
		private bool drawPlaceHolderIcon;

		// Token: 0x04000FAA RID: 4010
		private ThingDef shownItem;

		// Token: 0x04000FAB RID: 4011
		private Texture2D itemIcon;

		// Token: 0x04000FAC RID: 4012
		private Color iconColor = Color.white;

		// Token: 0x04000FAD RID: 4013
		public const float MaxWidth = 300f;

		// Token: 0x04000FAE RID: 4014
		private const float NormalVerticalMargin = 4f;

		// Token: 0x04000FAF RID: 4015
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04000FB0 RID: 4016
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04000FB1 RID: 4017
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x04000FB2 RID: 4018
		private const float MouseOverLabelShift = 4f;

		// Token: 0x04000FB3 RID: 4019
		private static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04000FB4 RID: 4020
		private static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		// Token: 0x04000FB5 RID: 4021
		private static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		// Token: 0x04000FB6 RID: 4022
		private static readonly Color ColorTextActive = Color.white;

		// Token: 0x04000FB7 RID: 4023
		private static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04000FB8 RID: 4024
		public const float ExtraPartHeight = 30f;

		// Token: 0x04000FB9 RID: 4025
		private const float ItemIconSize = 27f;

		// Token: 0x04000FBA RID: 4026
		private const float ItemIconMargin = 4f;
	}
}
