using System;
using Verse;

namespace RimWorld
{
	
	public class FiringIncident : IExposable
	{
		
		public FiringIncident()
		{
		}

		
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", Array.Empty<object>());
		}

		
		public override string ToString()
		{
			string text = this.def.defName;
			if (this.parms != null)
			{
				text = text + ", parms=(" + this.parms.ToString() + ")";
			}
			if (this.source != null)
			{
				text = text + ", source=" + this.source;
			}
			if (this.sourceQuestPart != null)
			{
				text = text + ", sourceQuestPart=" + this.sourceQuestPart;
			}
			return text;
		}

		
		public IncidentDef def;

		
		public IncidentParms parms = new IncidentParms();

		
		public StorytellerComp source;

		
		public QuestPart sourceQuestPart;
	}
}
