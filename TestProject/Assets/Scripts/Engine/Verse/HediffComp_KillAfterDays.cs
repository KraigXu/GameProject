﻿using System;

namespace Verse
{
	
	public class HediffComp_KillAfterDays : HediffComp
	{
		
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0005EB40 File Offset: 0x0005CD40
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.addedTick = Find.TickManager.TicksGame;
		}

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame - this.addedTick >= 60000 * this.Props.days)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.addedTick, "addedTick", 0, false);
		}

		
		private int addedTick;
	}
}
