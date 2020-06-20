using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEE RID: 2798
	public class Hediff_High : HediffWithComps
	{
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x00161469 File Offset: 0x0015F669
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity <= 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent("F0");
			}
		}
	}
}
