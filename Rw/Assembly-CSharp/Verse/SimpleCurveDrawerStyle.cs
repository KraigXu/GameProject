using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000413 RID: 1043
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x000C06DD File Offset: 0x000BE8DD
		// (set) Token: 0x06001F0E RID: 7950 RVA: 0x000C06E5 File Offset: 0x000BE8E5
		public bool DrawBackground { get; set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x000C06EE File Offset: 0x000BE8EE
		// (set) Token: 0x06001F10 RID: 7952 RVA: 0x000C06F6 File Offset: 0x000BE8F6
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x000C06FF File Offset: 0x000BE8FF
		// (set) Token: 0x06001F12 RID: 7954 RVA: 0x000C0707 File Offset: 0x000BE907
		public bool DrawMeasures { get; set; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x000C0710 File Offset: 0x000BE910
		// (set) Token: 0x06001F14 RID: 7956 RVA: 0x000C0718 File Offset: 0x000BE918
		public bool DrawPoints { get; set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x000C0721 File Offset: 0x000BE921
		// (set) Token: 0x06001F16 RID: 7958 RVA: 0x000C0729 File Offset: 0x000BE929
		public bool DrawLegend { get; set; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x000C0732 File Offset: 0x000BE932
		// (set) Token: 0x06001F18 RID: 7960 RVA: 0x000C073A File Offset: 0x000BE93A
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x000C0743 File Offset: 0x000BE943
		// (set) Token: 0x06001F1A RID: 7962 RVA: 0x000C074B File Offset: 0x000BE94B
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000C0754 File Offset: 0x000BE954
		// (set) Token: 0x06001F1C RID: 7964 RVA: 0x000C075C File Offset: 0x000BE95C
		public bool UseFixedSection { get; set; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x000C0765 File Offset: 0x000BE965
		// (set) Token: 0x06001F1E RID: 7966 RVA: 0x000C076D File Offset: 0x000BE96D
		public bool UseFixedScale { get; set; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x000C0776 File Offset: 0x000BE976
		// (set) Token: 0x06001F20 RID: 7968 RVA: 0x000C077E File Offset: 0x000BE97E
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001F21 RID: 7969 RVA: 0x000C0787 File Offset: 0x000BE987
		// (set) Token: 0x06001F22 RID: 7970 RVA: 0x000C078F File Offset: 0x000BE98F
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x000C0798 File Offset: 0x000BE998
		// (set) Token: 0x06001F24 RID: 7972 RVA: 0x000C07A0 File Offset: 0x000BE9A0
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001F25 RID: 7973 RVA: 0x000C07A9 File Offset: 0x000BE9A9
		// (set) Token: 0x06001F26 RID: 7974 RVA: 0x000C07B1 File Offset: 0x000BE9B1
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x000C07BA File Offset: 0x000BE9BA
		// (set) Token: 0x06001F28 RID: 7976 RVA: 0x000C07C2 File Offset: 0x000BE9C2
		public bool XIntegersOnly { get; set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001F29 RID: 7977 RVA: 0x000C07CB File Offset: 0x000BE9CB
		// (set) Token: 0x06001F2A RID: 7978 RVA: 0x000C07D3 File Offset: 0x000BE9D3
		public bool YIntegersOnly { get; set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000C07DC File Offset: 0x000BE9DC
		// (set) Token: 0x06001F2C RID: 7980 RVA: 0x000C07E4 File Offset: 0x000BE9E4
		public string LabelX { get; set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x000C07ED File Offset: 0x000BE9ED
		// (set) Token: 0x06001F2E RID: 7982 RVA: 0x000C07F5 File Offset: 0x000BE9F5
		public FloatRange FixedSection { get; set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000C07FE File Offset: 0x000BE9FE
		// (set) Token: 0x06001F30 RID: 7984 RVA: 0x000C0806 File Offset: 0x000BEA06
		public Vector2 FixedScale { get; set; }

		// Token: 0x06001F31 RID: 7985 RVA: 0x000C0810 File Offset: 0x000BEA10
		public SimpleCurveDrawerStyle()
		{
			this.DrawBackground = false;
			this.DrawBackgroundLines = true;
			this.DrawMeasures = false;
			this.DrawPoints = true;
			this.DrawLegend = false;
			this.DrawCurveMousePoint = false;
			this.OnlyPositiveValues = false;
			this.UseFixedSection = false;
			this.UseFixedScale = false;
			this.UseAntiAliasedLines = false;
			this.PointsRemoveOptimization = false;
			this.MeasureLabelsXCount = 5;
			this.MeasureLabelsYCount = 5;
			this.XIntegersOnly = false;
			this.YIntegersOnly = false;
			this.LabelX = "x";
		}
	}
}
