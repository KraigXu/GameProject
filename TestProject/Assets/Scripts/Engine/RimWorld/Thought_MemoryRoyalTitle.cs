using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		
		
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"));
			}
		}

		
		
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
		}

		
		public RoyalTitleDef titleDef;
	}
}
