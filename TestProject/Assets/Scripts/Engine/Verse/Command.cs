using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x000A4A43 File Offset: 0x000A2C43
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		
		// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x000A4A4B File Offset: 0x000A2C4B
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x000A4A58 File Offset: 0x000A2C58
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x000A4A60 File Offset: 0x000A2C60
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x000A4A68 File Offset: 0x000A2C68
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x000A4A70 File Offset: 0x000A2C70
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x000A4A70 File Offset: 0x000A2C70
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x000A4A78 File Offset: 0x000A2C78
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Text.Font = GameFont.Tiny;
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			bool flag = false;
			if (Mouse.IsOver(rect))
			{
				flag = true;
				if (!this.disabled)
				{
					GUI.color = GenUI.MouseoverColor;
				}
			}
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Command);
			Material material = this.disabled ? TexUI.GrayscaleGUI : null;
			GenUI.DrawTextureWithMaterial(rect, this.BGTexture, material, default(Rect));
			this.DrawIcon(rect, material);
			bool flag2 = false;
			KeyCode keyCode = (this.hotKey == null) ? KeyCode.None : this.hotKey.MainKey;
			if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
			{
				Widgets.Label(new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, 18f), keyCode.ToStringReadable());
				GizmoGridDrawer.drawnHotKeys.Add(keyCode);
				if (this.hotKey.KeyDownEvent)
				{
					flag2 = true;
					Event.current.Use();
				}
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				flag2 = true;
			}
			string labelCap = this.LabelCap;
			if (!labelCap.NullOrEmpty())
			{
				float num = Text.CalcHeight(labelCap, rect.width);
				Rect rect2 = new Rect(rect.x, rect.yMax - num + 12f, rect.width, num);
				GUI.DrawTexture(rect2, TexUI.GrayTextBG);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(rect2, labelCap);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			GUI.color = Color.white;
			if (Mouse.IsOver(rect) && this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					tip.text += "\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason;
				}
				TooltipHandler.TipRegion(rect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(rect)))
			{
				UIHighlighter.HighlightOpportunity(rect, this.HighlightTag);
			}
			Text.Font = GameFont.Small;
			if (flag2)
			{
				if (this.disabled)
				{
					if (!this.disabledReason.NullOrEmpty())
					{
						Messages.Message(this.disabledReason, MessageTypeDefOf.RejectInput, false);
					}
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				GizmoResult result;
				if (Event.current.button == 1)
				{
					result = new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
				}
				else
				{
					if (!TutorSystem.AllowAction(this.TutorTagSelect))
					{
						return new GizmoResult(GizmoState.Mouseover, null);
					}
					result = new GizmoResult(GizmoState.Interacted, Event.current);
					TutorSystem.Notify_Event(this.TutorTagSelect);
				}
				return result;
			}
			else
			{
				if (flag)
				{
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				return new GizmoResult(GizmoState.Clear, null);
			}
		}

		
		protected virtual void DrawIcon(Rect rect, Material buttonMat = null)
		{
			Texture2D badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			rect.position += new Vector2(this.iconOffset.x * rect.size.x, this.iconOffset.y * rect.size.y);
			GUI.color = this.IconDrawColor;
			Widgets.DrawTextureFitted(rect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, buttonMat);
			GUI.color = Color.white;
		}

		
		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			return command != null && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon) || (this.groupKey != 0 && command.groupKey != 0 && this.groupKey == command.groupKey));
		}

		
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Command(label=",
				this.defaultLabel,
				", defaultDesc=",
				this.defaultDesc,
				")"
			});
		}

		
		public string defaultLabel;

		
		public string defaultDesc = "No description.";

		
		public Texture2D icon;

		
		public float iconAngle;

		
		public Vector2 iconProportions = Vector2.one;

		
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		
		public float iconDrawScale = 1f;

		
		public Vector2 iconOffset;

		
		public Color defaultIconColor = Color.white;

		
		public KeyBindingDef hotKey;

		
		public SoundDef activateSound;

		
		public int groupKey;

		
		public string tutorTag = "TutorTagNotSet";

		
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		
		protected const float InnerIconDrawScale = 0.85f;
	}
}
