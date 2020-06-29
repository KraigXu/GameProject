using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public abstract class CompScanner : ThingComp
	{
		
		
		public CompProperties_Scanner Props
		{
			get
			{
				return (CompProperties_Scanner)this.props;
			}
		}

		
		
		public bool CanUseNow
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && !RoofUtility.IsAnyCellUnderRoof(this.parent) && (this.forbiddable == null || !this.forbiddable.Forbidden) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.lastScanTick, "lastScanTick", 0f, false);
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.forbiddable = this.parent.GetComp<CompForbiddable>();
		}

		
		public void Used(Pawn worker)
		{
			if (!this.CanUseNow)
			{
				Log.Error("Used while CanUseNow is false.", false);
			}
			this.lastScanTick = (float)Find.TickManager.TicksGame;
			this.lastUserSpeed = 1f;
			if (this.Props.scanSpeedStat != null)
			{
				this.lastUserSpeed = worker.GetStatValue(this.Props.scanSpeedStat, true);
			}
			this.daysWorkingSinceLastFinding += this.lastUserSpeed / 60000f;
			if (this.TickDoesFind(this.lastUserSpeed))
			{
				this.DoFind(worker);
				this.daysWorkingSinceLastFinding = 0f;
			}
		}

		
		protected virtual bool TickDoesFind(float scanSpeed)
		{
			return Find.TickManager.TicksGame % 59 == 0 && (Rand.MTBEventOccurs(this.Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (this.Props.scanFindGuaranteedDays > 0f && this.daysWorkingSinceLastFinding >= this.Props.scanFindGuaranteedDays));
		}

		
		public override string CompInspectStringExtra()
		{
			string t = "";
			if (this.lastScanTick > (float)(Find.TickManager.TicksGame - 30))
			{
				t += "UserScanAbility".Translate() + ": " + this.lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((this.Props.scanFindMtbDays / this.lastUserSpeed).ToString("F1")) + "\n";
			}
			return t + "ScanningProgressToGuaranteedFind".Translate() + ": " + (this.daysWorkingSinceLastFinding / this.Props.scanFindGuaranteedDays).ToStringPercent();
		}

		
		protected abstract void DoFind(Pawn worker);

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Find now",
					action = delegate
					{
						this.DoFind(PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>());
					}
				};
			}
			yield break;
		}

		
		protected float daysWorkingSinceLastFinding;

		
		protected float lastUserSpeed = 1f;

		
		protected float lastScanTick = -1f;

		
		protected CompPowerTrader powerComp;

		
		protected CompForbiddable forbiddable;
	}
}
