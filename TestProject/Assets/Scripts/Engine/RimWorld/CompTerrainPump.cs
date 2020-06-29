using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class CompTerrainPump : ThingComp
	{
		
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x001BF6E4 File Offset: 0x001BD8E4
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		
		// (get) Token: 0x060053A9 RID: 21417 RVA: 0x001BF6F1 File Offset: 0x001BD8F1
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x001BF700 File Offset: 0x001BD900
		private float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001BF730 File Offset: 0x001BD930
		private bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		
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

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		
		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		
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

		
		protected abstract void AffectCell(IntVec3 c);

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		
		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true, true) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true, true);
			}
			return text;
		}

		
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

		
		private CompPowerTrader powerComp;

		
		private int progressTicks;
	}
}
