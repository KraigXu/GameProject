using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008DA RID: 2266
	public class InteractionWorker
	{
		// Token: 0x06003655 RID: 13909 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x00126A06 File Offset: 0x00124C06
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x04001EBB RID: 7867
		public InteractionDef interaction;
	}
}
