using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x0018566D File Offset: 0x0018386D
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"));
			}
		}

		
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x001856A0 File Offset: 0x001838A0
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
