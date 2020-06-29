using System;
using Verse;

namespace RimWorld
{
	
	public class Hediff_High : HediffWithComps
	{
		
		
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
