using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D09 RID: 3337
	public class CompFadesInOut : ThingComp
	{
		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x001B3E34 File Offset: 0x001B2034
		public CompProperties_FadesInOut Props
		{
			get
			{
				return (CompProperties_FadesInOut)this.props;
			}
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001B3E41 File Offset: 0x001B2041
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				this.ageTicks++;
			}
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001B3E64 File Offset: 0x001B2064
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

		// Token: 0x06005133 RID: 20787 RVA: 0x001B3F2F File Offset: 0x001B212F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
		}

		// Token: 0x04002CFE RID: 11518
		private int ageTicks;
	}
}
