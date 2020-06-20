using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E63 RID: 3683
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x06005940 RID: 22848 RVA: 0x001DCB7D File Offset: 0x001DAD7D
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x06005941 RID: 22849 RVA: 0x001DCBA0 File Offset: 0x001DADA0
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				float num = inRect.height - 79f;
				FactionUIUtility.DrawRelatedFactionInfo(inRect, this.faction, ref num);
			}
		}

		// Token: 0x0400304F RID: 12367
		private Faction faction;

		// Token: 0x04003050 RID: 12368
		private const float RelatedFactionInfoSize = 79f;
	}
}
