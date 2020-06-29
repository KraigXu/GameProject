using System;
using RimWorld;

namespace Verse
{
	
	public class CompGlower : ThingComp
	{
		
		
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		
		
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

		
		
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		
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

		
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}

		
		private bool glowOnInt;
	}
}
