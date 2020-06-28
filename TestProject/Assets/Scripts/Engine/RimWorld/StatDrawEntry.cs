using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E76 RID: 3702
	public class StatDrawEntry
	{
		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x060059C0 RID: 22976 RVA: 0x001E5754 File Offset: 0x001E3954
		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		// Token: 0x17001018 RID: 4120
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

		// Token: 0x17001019 RID: 4121
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

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x060059C3 RID: 22979 RVA: 0x001E5804 File Offset: 0x001E3A04
		public int DisplayPriorityWithinCategory
		{
			get
			{
				return this.displayOrderWithinCategory;
			}
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x001E580C File Offset: 0x001E3A0C
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

		// Token: 0x060059C5 RID: 22981 RVA: 0x001E5890 File Offset: 0x001E3A90
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

		// Token: 0x060059C6 RID: 22982 RVA: 0x001E58FC File Offset: 0x001E3AFC
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

		// Token: 0x060059C7 RID: 22983 RVA: 0x001E594D File Offset: 0x001E3B4D
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

		// Token: 0x060059C8 RID: 22984 RVA: 0x001E597C File Offset: 0x001E3B7C
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

		// Token: 0x060059C9 RID: 22985 RVA: 0x001E59E4 File Offset: 0x001E3BE4
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

		// Token: 0x060059CA RID: 22986 RVA: 0x001E5A60 File Offset: 0x001E3C60
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

		// Token: 0x060059CB RID: 22987 RVA: 0x001E5B89 File Offset: 0x001E3D89
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

		// Token: 0x040030C4 RID: 12484
		public StatCategoryDef category;

		// Token: 0x040030C5 RID: 12485
		private int displayOrderWithinCategory;

		// Token: 0x040030C6 RID: 12486
		public StatDef stat;

		// Token: 0x040030C7 RID: 12487
		private float value;

		// Token: 0x040030C8 RID: 12488
		public StatRequest optionalReq;

		// Token: 0x040030C9 RID: 12489
		public bool hasOptionalReq;

		// Token: 0x040030CA RID: 12490
		public bool forceUnfinalizedMode;

		// Token: 0x040030CB RID: 12491
		private IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks;

		// Token: 0x040030CC RID: 12492
		private string labelInt;

		// Token: 0x040030CD RID: 12493
		private string valueStringInt;

		// Token: 0x040030CE RID: 12494
		private string overrideReportText;

		// Token: 0x040030CF RID: 12495
		private string overrideReportTitle;

		// Token: 0x040030D0 RID: 12496
		private string explanationText;

		// Token: 0x040030D1 RID: 12497
		private ToStringNumberSense numberSense;
	}
}
