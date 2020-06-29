using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class StatDrawEntry
	{
		
		// (get) Token: 0x060059C0 RID: 22976 RVA: 0x001E5754 File Offset: 0x001E3954
		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		
		// (get) Token: 0x060059C1 RID: 22977 RVA: 0x001E5779 File Offset: 0x001E3979
		public string LabelCap
		{
			get
			{
				if (this.labelInt != null)
				{
					return this.labelInt.CapitalizeFirst();
				}
				return this.stat.LabelCap;
			}
		}

		
		// (get) Token: 0x060059C2 RID: 22978 RVA: 0x001E57A0 File Offset: 0x001E39A0
		public string ValueString
		{
			get
			{
				if (this.numberSense == ToStringNumberSense.Factor)
				{
					return this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				}
				if (this.valueStringInt == null)
				{
					return this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq, !this.forceUnfinalizedMode);
				}
				return this.valueStringInt;
			}
		}

		
		// (get) Token: 0x060059C3 RID: 22979 RVA: 0x001E5804 File Offset: 0x001E3A04
		public int DisplayPriorityWithinCategory
		{
			get
			{
				return this.displayOrderWithinCategory;
			}
		}

		
		public StatDrawEntry(StatCategoryDef category, StatDef stat, float value, StatRequest optionalReq, ToStringNumberSense numberSense = ToStringNumberSense.Undefined, int? overrideDisplayPriorityWithinCategory = null, bool forceUnfinalizedMode = false)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = value;
			this.valueStringInt = null;
			this.displayOrderWithinCategory = ((overrideDisplayPriorityWithinCategory != null) ? overrideDisplayPriorityWithinCategory.Value : stat.displayPriorityInCategory);
			this.optionalReq = optionalReq;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
			this.hasOptionalReq = true;
			if (numberSense == ToStringNumberSense.Undefined)
			{
				this.numberSense = stat.toStringNumberSense;
				return;
			}
			this.numberSense = numberSense;
		}

		
		public StatDrawEntry(StatCategoryDef category, string label, string valueString, string reportText, int displayPriorityWithinCategory, string overrideReportTitle = null, IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = null, bool forceUnfinalizedMode = false)
		{
			this.category = category;
			this.stat = null;
			this.labelInt = label;
			this.value = 0f;
			this.valueStringInt = valueString;
			this.displayOrderWithinCategory = displayPriorityWithinCategory;
			this.numberSense = ToStringNumberSense.Absolute;
			this.overrideReportText = reportText;
			this.overrideReportTitle = overrideReportTitle;
			this.hyperlinks = hyperlinks;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
		}

		
		public StatDrawEntry(StatCategoryDef category, StatDef stat)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = stat.displayPriorityInCategory;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		
		public IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks(StatRequest req)
		{
			if (this.hyperlinks != null)
			{
				return this.hyperlinks;
			}
			if (this.stat != null)
			{
				return this.stat.Worker.GetInfoCardHyperlinks(req);
			}
			return null;
		}

		
		public string GetExplanationText(StatRequest optionalReq)
		{
			if (this.explanationText == null)
			{
				this.WriteExplanationTextInt();
			}
			string result;
			if (optionalReq.Empty || this.stat == null)
			{
				result = this.explanationText;
			}
			else
			{
				result = string.Format("{0}\n\n{1}", this.explanationText, this.stat.Worker.GetExplanationFull(optionalReq, this.numberSense, this.value));
			}
			return result;
		}

		
		private void WriteExplanationTextInt()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.overrideReportTitle.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportTitle);
			}
			if (!this.overrideReportText.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportText);
			}
			else if (this.stat != null)
			{
				stringBuilder.AppendLine(this.stat.description);
			}
			stringBuilder.AppendLine();
			this.explanationText = stringBuilder.ToString().TrimEndNewlines();
		}

		
		public float Draw(float x, float y, float width, bool selected, Action clickedCallback, Action mousedOverCallback, Vector2 scrollPosition, Rect scrollOutRect)
		{
			float num = width * 0.45f;
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(this.ValueString, num));
			if (y - scrollPosition.y + rect.height >= 0f && y - scrollPosition.y <= scrollOutRect.height)
			{
				if (selected)
				{
					Widgets.DrawHighlightSelected(rect);
				}
				else if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				Rect rect2 = rect;
				rect2.width -= num;
				Widgets.Label(rect2, this.LabelCap);
				Rect rect3 = rect;
				rect3.x = rect2.xMax;
				rect3.width = num;
				Widgets.Label(rect3, this.ValueString);
				if (this.stat != null && Mouse.IsOver(rect))
				{
					StatDef localStat = this.stat;
					TooltipHandler.TipRegion(rect, new TipSignal(() => localStat.LabelCap + ": " + localStat.description, this.stat.GetHashCode()));
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					clickedCallback();
				}
				if (Mouse.IsOver(rect))
				{
					mousedOverCallback();
				}
			}
			return rect.height;
		}

		
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.LabelCap,
				": ",
				this.ValueString,
				")"
			});
		}

		
		public StatCategoryDef category;

		
		private int displayOrderWithinCategory;

		
		public StatDef stat;

		
		private float value;

		
		public StatRequest optionalReq;

		
		public bool hasOptionalReq;

		
		public bool forceUnfinalizedMode;

		
		private IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks;

		
		private string labelInt;

		
		private string valueStringInt;

		
		private string overrideReportText;

		
		private string overrideReportTitle;

		
		private string explanationText;

		
		private ToStringNumberSense numberSense;
	}
}
