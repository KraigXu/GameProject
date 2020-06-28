using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200038B RID: 907
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x000A4A43 File Offset: 0x000A2C43
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x000A4A4B File Offset: 0x000A2C4B
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x000A4A58 File Offset: 0x000A2C58
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x000A4A60 File Offset: 0x000A2C60
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x000A4A68 File Offset: 0x000A2C68
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x000A4A70 File Offset: 0x000A2C70
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x000A4A70 File Offset: 0x000A2C70
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x000A4A78 File Offset: 0x000A2C78
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000A4A7F File Offset: 0x000A2C7F
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000A4A88 File Offset: 0x000A2C88
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

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000A4D84 File Offset: 0x000A2F84
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

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000A4E2C File Offset: 0x000A302C
		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			return command != null && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon) || (this.groupKey != 0 && command.groupKey != 0 && this.groupKey == command.groupKey));
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000A4E9E File Offset: 0x000A309E
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000A4EB4 File Offset: 0x000A30B4
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

		// Token: 0x04000FD3 RID: 4051
		public string defaultLabel;

		// Token: 0x04000FD4 RID: 4052
		public string defaultDesc = "No description.";

		// Token: 0x04000FD5 RID: 4053
		public Texture2D icon;

		// Token: 0x04000FD6 RID: 4054
		public float iconAngle;

		// Token: 0x04000FD7 RID: 4055
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x04000FD8 RID: 4056
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04000FD9 RID: 4057
		public float iconDrawScale = 1f;

		// Token: 0x04000FDA RID: 4058
		public Vector2 iconOffset;

		// Token: 0x04000FDB RID: 4059
		public Color defaultIconColor = Color.white;

		// Token: 0x04000FDC RID: 4060
		public KeyBindingDef hotKey;

		// Token: 0x04000FDD RID: 4061
		public SoundDef activateSound;

		// Token: 0x04000FDE RID: 4062
		public int groupKey;

		// Token: 0x04000FDF RID: 4063
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x04000FE0 RID: 4064
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x04000FE1 RID: 4065
		protected const float InnerIconDrawScale = 0.85f;
	}
}
