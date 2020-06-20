using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D81 RID: 3457
	public abstract class CompScanner : ThingComp
	{
		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005444 RID: 21572 RVA: 0x001C22B3 File Offset: 0x001C04B3
		public CompProperties_Scanner Props
		{
			get
			{
				return (CompProperties_Scanner)this.props;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005445 RID: 21573 RVA: 0x001C22C0 File Offset: 0x001C04C0
		public bool CanUseNow
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && !RoofUtility.IsAnyCellUnderRoof(this.parent) && (this.forbiddable == null || !this.forbiddable.Forbidden) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x001C232C File Offset: 0x001C052C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.daysWorkingSinceLastFinding, "daysWorkingSinceLastFinding", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.lastScanTick, "lastScanTick", 0f, false);
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x001C2381 File Offset: 0x001C0581
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.forbiddable = this.parent.GetComp<CompForbiddable>();
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x001C23AC File Offset: 0x001C05AC
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

		// Token: 0x06005449 RID: 21577 RVA: 0x001C2448 File Offset: 0x001C0648
		protected virtual bool TickDoesFind(float scanSpeed)
		{
			return Find.TickManager.TicksGame % 59 == 0 && (Rand.MTBEventOccurs(this.Props.scanFindMtbDays / scanSpeed, 60000f, 59f) || (this.Props.scanFindGuaranteedDays > 0f && this.daysWorkingSinceLastFinding >= this.Props.scanFindGuaranteedDays));
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x001C24AC File Offset: 0x001C06AC
		public override string CompInspectStringExtra()
		{
			string t = "";
			if (this.lastScanTick > (float)(Find.TickManager.TicksGame - 30))
			{
				t += "UserScanAbility".Translate() + ": " + this.lastUserSpeed.ToStringPercent() + "\n" + "ScanAverageInterval".Translate() + ": " + "PeriodDays".Translate((this.Props.scanFindMtbDays / this.lastUserSpeed).ToString("F1")) + "\n";
			}
			return t + "ScanningProgressToGuaranteedFind".Translate() + ": " + (this.daysWorkingSinceLastFinding / this.Props.scanFindGuaranteedDays).ToStringPercent();
		}

		// Token: 0x0600544B RID: 21579
		protected abstract void DoFind(Pawn worker);

		// Token: 0x0600544C RID: 21580 RVA: 0x001C25A3 File Offset: 0x001C07A3
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

		// Token: 0x04002E6E RID: 11886
		protected float daysWorkingSinceLastFinding;

		// Token: 0x04002E6F RID: 11887
		protected float lastUserSpeed = 1f;

		// Token: 0x04002E70 RID: 11888
		protected float lastScanTick = -1f;

		// Token: 0x04002E71 RID: 11889
		protected CompPowerTrader powerComp;

		// Token: 0x04002E72 RID: 11890
		protected CompForbiddable forbiddable;
	}
}
