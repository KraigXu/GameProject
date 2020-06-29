using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		
		
		
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

		
		
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		
		
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		
		
		
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

		
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget)
		{
			this.shownItem = shownItemForIcon;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		
		public FloatMenuOption(string label, Action action, Texture2D itemIcon, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget)
		{
			this.itemIcon = itemIcon;
			this.iconColor = iconColor;
		}

		
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

		
		private string labelInt;

		
		public Action action;

		
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		
		public bool autoTakeable;

		
		public float autoTakeablePriority;

		
		public Action mouseoverGuiAction;

		
		public Thing revalidateClickTarget;

		
		public WorldObject revalidateWorldClickTarget;

		
		public float extraPartWidth;

		
		public Func<Rect, bool> extraPartOnGUI;

		
		public string tutorTag;

		
		private FloatMenuSizeMode sizeMode;

		
		private float cachedRequiredHeight;

		
		private float cachedRequiredWidth;

		
		private bool drawPlaceHolderIcon;

		
		private ThingDef shownItem;

		
		private Texture2D itemIcon;

		
		private Color iconColor = Color.white;

		
		public const float MaxWidth = 300f;

		
		private const float NormalVerticalMargin = 4f;

		
		private const float TinyVerticalMargin = 1f;

		
		private const float NormalHorizontalMargin = 6f;

		
		private const float TinyHorizontalMargin = 3f;

		
		private const float MouseOverLabelShift = 4f;

		
		private static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		
		private static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		
		private static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		
		private static readonly Color ColorTextActive = Color.white;

		
		private static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		
		public const float ExtraPartHeight = 30f;

		
		private const float ItemIconSize = 27f;

		
		private const float ItemIconMargin = 4f;
	}
}
