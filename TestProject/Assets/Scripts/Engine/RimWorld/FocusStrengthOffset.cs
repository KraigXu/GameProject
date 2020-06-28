using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D22 RID: 3362
	public abstract class FocusStrengthOffset
	{
		// Token: 0x060051C5 RID: 20933 RVA: 0x0004EE9A File Offset: 0x0004D09A
		public virtual string GetExplanation(Thing parent)
		{
			return "";
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x0004EE9A File Offset: 0x0004D09A
		public virtual string GetExplanationAbstract(ThingDef def = null)
		{
			return "";
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x0004EE9A File Offset: 0x0004D09A
		public virtual string InspectStringExtra(Thing parent, Pawn user = null)
		{
			return "";
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float GetOffset(Thing parent, Pawn user = null)
		{
			return 0f;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanApply(Thing parent, Pawn user = null)
		{
			return true;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001B5F2B File Offset: 0x001B412B
		public virtual float MaxOffset(bool forAbstract = false)
		{
			return this.offset;
		}

		// Token: 0x04002D28 RID: 11560
		public float offset;
	}
}
