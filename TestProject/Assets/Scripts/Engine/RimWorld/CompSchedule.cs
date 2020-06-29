﻿using System;
using Verse;

namespace RimWorld
{
	
	public class CompSchedule : ThingComp
	{
		
		
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		
		
		
		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed == value)
				{
					return;
				}
				this.intAllowed = value;
				this.parent.BroadcastCompSignal(this.intAllowed ? "ScheduledOn" : "ScheduledOff");
			}
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
				return;
			}
			this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.Allowed)
			{
				return this.Props.offMessage;
			}
			return null;
		}

		
		public const string ScheduledOnSignal = "ScheduledOn";

		
		public const string ScheduledOffSignal = "ScheduledOff";

		
		private bool intAllowed;
	}
}
