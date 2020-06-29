using System;
using Verse;

namespace RimWorld
{
	
	public class Alert_Custom : Alert
	{
		
		public override string GetLabel()
		{
			return this.label;
		}

		
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		
		public override AlertReport GetReport()
		{
			return this.report;
		}

		
		public string label;

		
		public string explanation;

		
		public AlertReport report;
	}
}
