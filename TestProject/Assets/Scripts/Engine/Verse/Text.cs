using System;
using UnityEngine;

namespace Verse
{
	
	public static class Text
	{
		
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x000AAAE7 File Offset: 0x000A8CE7
		// (set) Token: 0x06001C0C RID: 7180 RVA: 0x000AAAEE File Offset: 0x000A8CEE
		public static GameFont Font
		{
			get
			{
				return Text.fontInt;
			}
			set
			{
				if (value == GameFont.Tiny && !LongEventHandler.AnyEventNowOrWaiting && !LanguageDatabase.activeLanguage.info.canBeTiny)
				{
					Text.fontInt = GameFont.Small;
					return;
				}
				Text.fontInt = value;
			}
		}

		
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000AAB18 File Offset: 0x000A8D18
		// (set) Token: 0x06001C0E RID: 7182 RVA: 0x000AAB1F File Offset: 0x000A8D1F
		public static TextAnchor Anchor
		{
			get
			{
				return Text.anchorInt;
			}
			set
			{
				Text.anchorInt = value;
			}
		}

		
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x000AAB27 File Offset: 0x000A8D27
		// (set) Token: 0x06001C10 RID: 7184 RVA: 0x000AAB2E File Offset: 0x000A8D2E
		public static bool WordWrap
		{
			get
			{
				return Text.wordWrapInt;
			}
			set
			{
				Text.wordWrapInt = value;
			}
		}

		
		// (get) Token: 0x06001C11 RID: 7185 RVA: 0x000AAB36 File Offset: 0x000A8D36
		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x000AAB43 File Offset: 0x000A8D43
		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

		
		// (get) Token: 0x06001C13 RID: 7187 RVA: 0x000AAB50 File Offset: 0x000A8D50
		public static GUIStyle CurFontStyle
		{
			get
			{
				GUIStyle guistyle;
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					guistyle = Text.fontStyles[0];
					break;
				case GameFont.Small:
					guistyle = Text.fontStyles[1];
					break;
				case GameFont.Medium:
					guistyle = Text.fontStyles[2];
					break;
				default:
					throw new NotImplementedException();
				}
				guistyle.alignment = Text.anchorInt;
				guistyle.wordWrap = Text.wordWrapInt;
				return guistyle;
			}
		}

		
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000AABB4 File Offset: 0x000A8DB4
		public static GUIStyle CurTextFieldStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textFieldStyles[0];
				case GameFont.Small:
					return Text.textFieldStyles[1];
				case GameFont.Medium:
					return Text.textFieldStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x000AABF8 File Offset: 0x000A8DF8
		public static GUIStyle CurTextAreaStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaStyles[0];
				case GameFont.Small:
					return Text.textAreaStyles[1];
				case GameFont.Medium:
					return Text.textAreaStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000AAC3C File Offset: 0x000A8E3C
		public static GUIStyle CurTextAreaReadOnlyStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaReadOnlyStyles[0];
				case GameFont.Small:
					return Text.textAreaReadOnlyStyles[1];
				case GameFont.Medium:
					return Text.textAreaReadOnlyStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		
		static Text()
		{
			Text.fonts[0] = (Font)Resources.Load("Fonts/Calibri_tiny");
			Text.fonts[1] = (Font)Resources.Load("Fonts/Arial_small");
			Text.fonts[2] = (Font)Resources.Load("Fonts/Arial_medium");
			Text.fontStyles[0] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[0].font = Text.fonts[0];
			Text.fontStyles[1] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[1].font = Text.fonts[1];
			Text.fontStyles[1].contentOffset = new Vector2(0f, -1f);
			Text.fontStyles[2] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[2].font = Text.fonts[2];
			for (int i = 0; i < Text.textFieldStyles.Length; i++)
			{
				Text.textFieldStyles[i] = new GUIStyle(GUI.skin.textField);
				Text.textFieldStyles[i].alignment = TextAnchor.MiddleLeft;
			}
			Text.textFieldStyles[0].font = Text.fonts[0];
			Text.textFieldStyles[1].font = Text.fonts[1];
			Text.textFieldStyles[2].font = Text.fonts[2];
			for (int j = 0; j < Text.textAreaStyles.Length; j++)
			{
				Text.textAreaStyles[j] = new GUIStyle(Text.textFieldStyles[j]);
				Text.textAreaStyles[j].alignment = TextAnchor.UpperLeft;
				Text.textAreaStyles[j].wordWrap = true;
			}
			for (int k = 0; k < Text.textAreaReadOnlyStyles.Length; k++)
			{
				Text.textAreaReadOnlyStyles[k] = new GUIStyle(Text.textAreaStyles[k]);
				GUIStyle guistyle = Text.textAreaReadOnlyStyles[k];
				guistyle.normal.background = null;
				guistyle.active.background = null;
				guistyle.onHover.background = null;
				guistyle.hover.background = null;
				guistyle.onFocused.background = null;
				guistyle.focused.background = null;
			}
			GUI.skin.settings.doubleClickSelectsWord = true;
			int num = 0;
			foreach (object obj in Enum.GetValues(typeof(GameFont)))
			{
				Text.Font = (GameFont)obj;
				Text.lineHeights[num] = Text.CalcHeight("W", 999f);
				Text.spaceBetweenLines[num] = Text.CalcHeight("W\nW", 999f) - Text.CalcHeight("W", 999f) * 2f;
				num++;
			}
			Text.Font = GameFont.Small;
		}

		
		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		
		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

		
		public static void StartOfOnGUI()
		{
			if (!Text.WordWrap)
			{
				Log.ErrorOnce("Word wrap was false at end of frame.", 764362, false);
				Text.WordWrap = true;
			}
			if (Text.Anchor != TextAnchor.UpperLeft)
			{
				Log.ErrorOnce("Alignment was " + Text.Anchor + " at end of frame.", 15558, false);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Text.Font = GameFont.Small;
		}

		
		private static GameFont fontInt = GameFont.Small;

		
		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		
		private static bool wordWrapInt = true;

		
		private static Font[] fonts = new Font[3];

		
		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		
		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		
		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		
		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		
		private static readonly float[] lineHeights = new float[3];

		
		private static readonly float[] spaceBetweenLines = new float[3];

		
		private static GUIContent tmpTextGUIContent = new GUIContent();

		
		private const int NumFonts = 3;

		
		public const float SmallFontHeight = 22f;
	}
}
