using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000BD3 RID: 3027
	public class Thought_MemorySocialCumulative : Thought_MemorySocial
	{
		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060047D6 RID: 18390 RVA: 0x00185822 File Offset: 0x00183A22
		public override bool ShouldDiscard
		{
			get
			{
				return this.opinionOffset == 0f;
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x00185831 File Offset: 0x00183A31
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return Mathf.Min(this.opinionOffset, this.def.maxCumulatedOpinionOffset);
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00185870 File Offset: 0x00183A70
		public override void ThoughtInterval()
		{
			base.ThoughtInterval();
			if (this.age >= 60000)
			{
				if (this.opinionOffset < 0f)
				{
					this.opinionOffset += 1f;
					if (this.opinionOffset > 0f)
					{
						this.opinionOffset = 0f;
					}
				}
				else if (this.opinionOffset > 0f)
				{
					this.opinionOffset -= 1f;
					if (this.opinionOffset < 0f)
					{
						this.opinionOffset = 0f;
					}
				}
				this.age = 0;
			}
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00185908 File Offset: 0x00183B08
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			List<Thought_Memory> memories = this.pawn.needs.mood.thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				if (memories[i].def == this.def)
				{
					Thought_MemorySocialCumulative thought_MemorySocialCumulative = (Thought_MemorySocialCumulative)memories[i];
					if (thought_MemorySocialCumulative.OtherPawn() == this.otherPawn)
					{
						thought_MemorySocialCumulative.opinionOffset += this.opinionOffset;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400293A RID: 10554
		private const float OpinionOffsetChangePerDay = 1f;
	}
}
