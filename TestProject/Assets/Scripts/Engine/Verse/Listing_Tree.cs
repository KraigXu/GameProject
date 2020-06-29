using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public class Listing_Tree : Listing_Lines
	{
		
		
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		
		
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		
		protected void LabelLeft(string label, string tipText, int indentLevel, float widthOffset = 0f)
		{
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight)
			{
				xMin = this.XAtIndentLevel(indentLevel) + 18f
			};
			Widgets.DrawHighlightIfMouseover(rect);
			if (!tipText.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, tipText);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.width = this.LabelWidth - rect.xMin + widthOffset;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label.Truncate(rect.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			if (!node.Openable)
			{
				return false;
			}
			float x = this.XAtIndentLevel(indentLevel);
			float y = this.curY + this.lineHeight / 2f - 9f;
			Rect butRect = new Rect(x, y, 18f, 18f);
			Texture2D tex = node.IsOpen(openMask) ? TexButton.Collapse : TexButton.Reveal;
			if (Widgets.ButtonImage(butRect, tex, true))
			{
				bool flag = node.IsOpen(openMask);
				if (flag)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
				node.SetOpen(openMask, !flag);
				return true;
			}
			return false;
		}

		
		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, 50f);
			rect.xMin = this.LabelWidth;
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			this.curY += rect.height;
			Text.WordWrap = false;
		}

		
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool result = Widgets.ButtonText(new Rect(0f, this.curY, base.ColumnWidth, num), label, true, true, true);
			this.curY += num + 0f;
			Text.WordWrap = false;
			return result;
		}

		
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		
		public float nestIndentWidth = 11f;

		
		protected const float OpenCloseWidgetSize = 18f;
	}
}
