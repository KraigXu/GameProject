using System;
using Verse;

namespace RimWorld
{
	
	public class Hediff_High : HediffWithComps
	{
		
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
