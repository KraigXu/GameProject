using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6A RID: 3434
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x0600539C RID: 21404 RVA: 0x001BF495 File Offset: 0x001BD695
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x001BF4A2 File Offset: 0x001BD6A2
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x001BF4B4 File Offset: 0x001BD6B4
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x001BF4CC File Offset: 0x001BD6CC
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x001BF4D9 File Offset: 0x001BD6D9
		public override void CompTick()
		{
			this.DoTicks(1);
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x001BF4E2 File Offset: 0x001BD6E2
		public override void CompTickRare()
		{
			this.DoTicks(250);
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x001BF4F0 File Offset: 0x001BD6F0
		private void DoTicks(int ticks)
		{
			if (!this.Ruined)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					this.ruinedPercent += (ambientTemperature - this.Props.maxSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				else if (ambientTemperature < this.Props.minSafeTemperature)
				{
					this.ruinedPercent -= (ambientTemperature - this.Props.minSafeTemperature) * this.Props.progressPerDegreePerTick * (float)ticks;
				}
				if (this.ruinedPercent >= 1f)
				{
					this.ruinedPercent = 1f;
					this.parent.BroadcastCompSignal("RuinedByTemperature");
					return;
				}
				if (this.ruinedPercent < 0f)
				{
					this.ruinedPercent = 0f;
				}
			}
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x001BF5C8 File Offset: 0x001BD7C8
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x001BF60C File Offset: 0x001BD80C
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x001BF633 File Offset: 0x001BD833
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompTemperatureRuinable>().ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x001BF64C File Offset: 0x001BD84C
		public override string CompInspectStringExtra()
		{
			if (this.Ruined)
			{
				return "RuinedByTemperature".Translate();
			}
			if (this.ruinedPercent > 0f)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				string str;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					str = "Overheating".Translate();
				}
				else
				{
					if (ambientTemperature >= this.Props.minSafeTemperature)
					{
						return null;
					}
					str = "Freezing".Translate();
				}
				return str + ": " + this.ruinedPercent.ToStringPercent();
			}
			return null;
		}

		// Token: 0x04002E31 RID: 11825
		protected float ruinedPercent;

		// Token: 0x04002E32 RID: 11826
		public const string RuinedSignal = "RuinedByTemperature";
	}
}
