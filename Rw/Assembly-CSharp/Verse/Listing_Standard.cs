using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003A9 RID: 937
	public class Listing_Standard : Listing
	{
		// Token: 0x06001B81 RID: 7041 RVA: 0x000A874D File Offset: 0x000A694D
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000A875C File Offset: 0x000A695C
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000A876B File Offset: 0x000A696B
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000A877F File Offset: 0x000A697F
		public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
		{
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			rect.height = 100000f;
			rect.width -= 20f;
			this.Begin(rect.AtZero());
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000A87BC File Offset: 0x000A69BC
		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int i = this.labelScrollbarPositions.Count - 1; i >= 0; i--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[i].First))
					{
						this.labelScrollbarPositions.RemoveAt(i);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000A8827 File Offset: 0x000A6A27
		public void EndScrollView(ref Rect viewRect)
		{
			viewRect = new Rect(0f, 0f, this.listingRect.width, this.curY);
			Widgets.EndScrollView();
			this.End();
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000A885A File Offset: 0x000A6A5A
		public Rect Label(TaggedString label, float maxHeight = -1f, string tooltip = null)
		{
			return this.Label(label.Resolve(), maxHeight, tooltip);
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000A886C File Offset: 0x000A6A6C
		public Rect Label(string label, float maxHeight = -1f, string tooltip = null)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0f && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num);
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false, true, false);
				this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000A88FC File Offset: 0x000A6AFC
		public void LabelDouble(string leftLabel, string rightLabel, string tip = null)
		{
			float num = base.ColumnWidth / 2f;
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height);
			if (!tip.NullOrEmpty())
			{
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, tip);
			}
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000A8980 File Offset: 0x000A6B80
		[Obsolete]
		public bool RadioButton(string label, bool active, float tabIn = 0f, string tooltip = null)
		{
			return this.RadioButton_NewTemp(label, active, tabIn, tooltip, null);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		public bool RadioButton_NewTemp(string label, bool active, float tabIn = 0f, string tooltip = null, float? tooltipDelay = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			rect.xMin += tabIn;
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TipSignal tip = (tooltipDelay != null) ? new TipSignal(tooltip, tooltipDelay.Value) : new TipSignal(tooltip);
				TooltipHandler.TipRegion(rect, tip);
			}
			bool result = Widgets.RadioButtonLabeled(rect, label, active);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000A8A24 File Offset: 0x000A6C24
		public void CheckboxLabeled(string label, ref bool checkOn, string tooltip = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000A8A7C File Offset: 0x000A6C7C
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			bool result = Widgets.CheckboxLabeledSelectable(base.GetRect(lineHeight), label, ref selected, ref checkOn);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000A8AAC File Offset: 0x000A6CAC
		public bool ButtonText(string label, string highlightTag = null)
		{
			Rect rect = base.GetRect(30f);
			bool result = Widgets.ButtonText(rect, label, true, true, true);
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000A8AE5 File Offset: 0x000A6CE5
		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, true, true);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000A8B18 File Offset: 0x000A6D18
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(this.curX, this.curY, width, height), tex, true);
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000A8B49 File Offset: 0x000A6D49
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000A8B84 File Offset: 0x000A6D84
		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result;
			if (lineCount == 1)
			{
				result = Widgets.TextField(rect, text);
			}
			else
			{
				result = Widgets.TextArea(rect, text, false);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000A8BC4 File Offset: 0x000A6DC4
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			string result = Widgets.TextEntryLabeled(base.GetRect(Text.LineHeight * (float)lineCount), label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000A8BE7 File Offset: 0x000A6DE7
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Widgets.TextFieldNumeric<T>(base.GetRect(Text.LineHeight), ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000A8C0A File Offset: 0x000A6E0A
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Widgets.TextFieldNumericLabeled<T>(base.GetRect(Text.LineHeight), label, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000A8C2F File Offset: 0x000A6E2F
		public void IntRange(ref IntRange range, int min, int max)
		{
			Widgets.IntRange(base.GetRect(28f), (int)base.CurHeight, ref range, min, max, null, 0);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000A8C5C File Offset: 0x000A6E5C
		public float Slider(float val, float min, float max)
		{
			float num = Widgets.HorizontalSlider(base.GetRect(22f), val, min, max, false, null, null, null, -1f);
			if (num != val)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			base.Gap(this.verticalSpacing);
			return num;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000A8CA0 File Offset: 0x000A6EA0
		public void IntAdjuster(ref int val, int countChange, int min = 0)
		{
			Rect rect = base.GetRect(24f);
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange, true, true, true))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange, true, true, true))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000A8D60 File Offset: 0x000A6F60
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			if (Widgets.ButtonText(base.GetRect(24f), label, true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000A8D92 File Offset: 0x000A6F92
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Widgets.IntEntry(base.GetRect(24f), ref val, ref editBuffer, multiplier);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000A8DB4 File Offset: 0x000A6FB4
		public Listing_Standard BeginSection(float height)
		{
			Rect rect = base.GetRect(height + 8f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect.ContractedBy(4f));
			return listing_Standard;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000A8DEB File Offset: 0x000A6FEB
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000A8DF4 File Offset: 0x000A6FF4
		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			if (this.labelScrollbarPositions == null)
			{
				return Vector2.zero;
			}
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					return this.labelScrollbarPositions[i].Second;
				}
			}
			return Vector2.zero;
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000A8E68 File Offset: 0x000A7068
		private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
		{
			if (this.labelScrollbarPositions == null)
			{
				this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
				this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
			}
			this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
					return;
				}
			}
			this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000A8F14 File Offset: 0x000A7114
		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = this.listingRect.width - 21f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(this.curX, this.curY, width, 21f);
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 1);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, name);
			Text.WordWrap = true;
			if (deleteCallback != null && Widgets.ButtonImage(new Rect(rect.xMax, rect.y, 21f, 21f), TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor, true))
			{
				deleteCallback();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.curY += 21f;
			return Widgets.ButtonInvisible(rect, true);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000A8FE0 File Offset: 0x000A71E0
		public void LabelCheckboxDebug(string label, ref bool checkOn)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Widgets.CheckboxLabeled(new Rect(this.curX, this.curY, base.ColumnWidth, 22f), label, ref checkOn, false, null, null, false);
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000A9038 File Offset: 0x000A7238
		public bool ButtonDebug(string label)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			bool wordWrap = Text.WordWrap;
			Text.WordWrap = false;
			bool result = Widgets.ButtonText(new Rect(this.curX, this.curY, base.ColumnWidth, 22f), label, true, true, true);
			Text.WordWrap = wordWrap;
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x0400104A RID: 4170
		private GameFont font;

		// Token: 0x0400104B RID: 4171
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x0400104C RID: 4172
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x0400104D RID: 4173
		private const float DefSelectionLineHeight = 21f;
	}
}
