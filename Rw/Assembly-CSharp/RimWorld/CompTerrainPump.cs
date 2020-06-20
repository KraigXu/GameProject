using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6B RID: 3435
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x001BF6E4 File Offset: 0x001BD8E4
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x060053A9 RID: 21417 RVA: 0x001BF6F1 File Offset: 0x001BD8F1
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x001BF700 File Offset: 0x001BD900
		private float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001BF730 File Offset: 0x001BD930
		private bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x001BF748 File Offset: 0x001BD948
		private int TicksUntilRadiusInteger
		{
			get
			{
				float num = Mathf.Ceil(this.CurrentRadius) - this.CurrentRadius;
				if (num < 1E-05f)
				{
					num = 1f;
				}
				float num2 = this.Props.radius / this.Props.daysToRadius;
				return (int)(num / num2 * 60000f);
			}
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x001BF798 File Offset: 0x001BD998
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x001BF7AB File Offset: 0x001BD9AB
		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x001BF7B4 File Offset: 0x001BD9B4
		public override void CompTickRare()
		{
			if (this.Working)
			{
				this.progressTicks += 250;
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius);
				for (int i = 0; i < num; i++)
				{
					this.AffectCell(this.parent.Position + GenRadial.RadialPattern[i]);
				}
			}
		}

		// Token: 0x060053B0 RID: 21424
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x060053B1 RID: 21425 RVA: 0x001BF814 File Offset: 0x001BDA14
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x001BF828 File Offset: 0x001BDA28
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x001BF85C File Offset: 0x001BDA5C
		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true, true) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true, true);
			}
			return text;
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x001BF944 File Offset: 0x001BDB44
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Progress 1 day",
					action = delegate
					{
						this.progressTicks += 60000;
					}
				};
			}
			yield break;
		}

		// Token: 0x04002E33 RID: 11827
		private CompPowerTrader powerComp;

		// Token: 0x04002E34 RID: 11828
		private int progressTicks;
	}
}
