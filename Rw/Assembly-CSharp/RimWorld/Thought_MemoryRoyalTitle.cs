using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD1 RID: 3025
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x0018566D File Offset: 0x0018386D
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"));
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x001856A0 File Offset: 0x001838A0
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x001856ED File Offset: 0x001838ED
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
		}

		// Token: 0x04002938 RID: 10552
		public RoyalTitleDef titleDef;
	}
}
