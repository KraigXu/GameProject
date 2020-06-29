using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public abstract class CompStatOffsetBase : ThingComp
	{
		
		
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		
		
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
