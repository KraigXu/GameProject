using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D65 RID: 3429
	public abstract class CompStatOffsetBase : ThingComp
	{
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06005384 RID: 21380 RVA: 0x001BF0BE File Offset: 0x001BD2BE
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06005385 RID: 21381 RVA: 0x001BF0CB File Offset: 0x001BD2CB
		public Pawn LastUser
		{
			get
			{
				return this.lastUser;
			}
		}

		// Token: 0x06005386 RID: 21382
		public abstract float GetStatOffset(Pawn pawn = null);

		// Token: 0x06005387 RID: 21383
		public abstract IEnumerable<string> GetExplanation();

		// Token: 0x06005388 RID: 21384 RVA: 0x001BF0D3 File Offset: 0x001BD2D3
		public void Used(Pawn pawn)
		{
			this.lastUser = pawn;
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x001BF0DC File Offset: 0x001BD2DC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.lastUser, "lastUser", false);
		}

		// Token: 0x04002E29 RID: 11817
		protected Pawn lastUser;
	}
}
