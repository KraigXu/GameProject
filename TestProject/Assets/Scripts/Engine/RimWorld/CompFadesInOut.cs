using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompFadesInOut : ThingComp
	{
		
		
		public CompProperties_FadesInOut Props
		{
			get
			{
				return (CompProperties_FadesInOut)this.props;
			}
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				this.ageTicks++;
			}
		}

		
		public float Opacity()
		{
			float num = this.ageTicks.TicksToSeconds();
			if (num <= this.Props.fadeInSecs)
			{
				if (this.Props.fadeInSecs > 0f)
				{
					return num / this.Props.fadeInSecs;
				}
				return 1f;
			}
			else
			{
				if (num <= this.Props.fadeInSecs + this.Props.solidTimeSecs)
				{
					return 1f;
				}
				if (this.Props.fadeOutSecs > 0f)
				{
					return 1f - Mathf.InverseLerp(this.Props.fadeInSecs + this.Props.solidTimeSecs, this.Props.fadeInSecs + this.Props.solidTimeSecs + this.Props.fadeOutSecs, num);
				}
				return 1f;
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
		}

		
		private int ageTicks;
	}
}
