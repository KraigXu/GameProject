using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DescriptionPart : QuestPartActivable
	{
		
		
		public override string DescriptionPart
		{
			get
			{
				return this.resolvedDescriptionPart;
			}
		}

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedDescriptionPart = receivedArgs.GetFormattedText(this.descriptionPart);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.descriptionPart, "descriptionPart", null, false);
			Scribe_Values.Look<string>(ref this.resolvedDescriptionPart, "resolvedDescriptionPart", null, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.descriptionPart = "Debug description part.";
		}

		
		public string descriptionPart;

		
		private string resolvedDescriptionPart;
	}
}
