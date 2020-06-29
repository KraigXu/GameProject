﻿using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	
	public class HediffComp_ReactOnDamage : HediffComp
	{
		
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0005EDE8 File Offset: 0x0005CFE8
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		
		private void React()
		{
			if (this.Props.createHediff != null)
			{
				BodyPartRecord part = this.parent.Part;
				if (this.Props.createHediffOn != null)
				{
					part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == this.Props.createHediffOn, null);
				}
				this.parent.pawn.health.AddHediff(this.Props.createHediff, part, null, null);
			}
			if (this.Props.vomit)
			{
				this.parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
			}
		}
	}
}
