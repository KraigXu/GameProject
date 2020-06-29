using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public abstract class CompStatOffsetBase : ThingComp
	{
		
		// (get) Token: 0x06005384 RID: 21380 RVA: 0x001BF0BE File Offset: 0x001BD2BE
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		
		// (get) Token: 0x06005385 RID: 21381 RVA: 0x001BF0CB File Offset: 0x001BD2CB
		public Pawn LastUser
		{
			get
			{
				return this.lastUser;
			}
		}

		
		public abstract float GetStatOffset(Pawn pawn = null);

		
		public abstract IEnumerable<string> GetExplanation();

		
		public void Used(Pawn pawn)
		{
			this.lastUser = pawn;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.lastUser, "lastUser", false);
		}

		
		protected Pawn lastUser;
	}
}
