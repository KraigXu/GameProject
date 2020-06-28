using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000162 RID: 354
	public class CompGlower : ThingComp
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00035E89 File Offset: 0x00034089
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00035E98 File Offset: 0x00034098
		private bool ShouldBeLitNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					return false;
				}
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				if (compPowerTrader != null && !compPowerTrader.PowerOn)
				{
					return false;
				}
				CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && !compRefuelable.HasFuel)
				{
					return false;
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = this.parent.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft <= 0)
				{
					return false;
				}
				CompSendSignalOnPawnProximity compSendSignalOnPawnProximity = this.parent.TryGetComp<CompSendSignalOnPawnProximity>();
				return compSendSignalOnPawnProximity == null || !compSendSignalOnPawnProximity.Sent;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00035F29 File Offset: 0x00034129
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00035F34 File Offset: 0x00034134
		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt == shouldBeLitNow)
			{
				return;
			}
			this.glowOnInt = shouldBeLitNow;
			if (!this.glowOnInt)
			{
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
				map.glowGrid.DeRegisterGlower(this);
				return;
			}
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00035FA8 File Offset: 0x000341A8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.ShouldBeLitNow)
			{
				this.UpdateLit(this.parent.Map);
				this.parent.Map.glowGrid.RegisterGlower(this);
				return;
			}
			this.UpdateLit(this.parent.Map);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00035FF8 File Offset: 0x000341F8
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0003608B File Offset: 0x0003428B
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0003609F File Offset: 0x0003429F
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}

		// Token: 0x04000809 RID: 2057
		private bool glowOnInt;
	}
}
