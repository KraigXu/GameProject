using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A7F RID: 2687
	public class CompPowerTrader : CompPower
	{
		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06003F79 RID: 16249 RVA: 0x00151759 File Offset: 0x0014F959
		// (set) Token: 0x06003F7A RID: 16250 RVA: 0x00151761 File Offset: 0x0014F961
		public float PowerOutput
		{
			get
			{
				return this.powerOutputInt;
			}
			set
			{
				this.powerOutputInt = value;
				if (this.powerOutputInt > 0f)
				{
					this.powerLastOutputted = true;
				}
				if (this.powerOutputInt < 0f)
				{
					this.powerLastOutputted = false;
				}
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x00151792 File Offset: 0x0014F992
		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003F7C RID: 16252 RVA: 0x001517A0 File Offset: 0x0014F9A0
		// (set) Token: 0x06003F7D RID: 16253 RVA: 0x001517A8 File Offset: 0x0014F9A8
		public bool PowerOn
		{
			get
			{
				return this.powerOnInt;
			}
			set
			{
				if (this.powerOnInt == value)
				{
					return;
				}
				this.powerOnInt = value;
				if (!this.powerOnInt)
				{
					if (this.powerStoppedAction != null)
					{
						this.powerStoppedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOff");
					SoundDef soundDef = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
					if (soundDef.NullOrUndefined())
					{
						soundDef = SoundDefOf.Power_OffSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.EndSustainerPoweredIfActive();
					return;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					Log.Warning("Tried to power on " + this.parent + " which did not desire it.", false);
					return;
				}
				if (this.parent.IsBrokenDown())
				{
					Log.Warning("Tried to power on " + this.parent + " which is broken down.", false);
					return;
				}
				if (this.powerStartedAction != null)
				{
					this.powerStartedAction();
				}
				this.parent.BroadcastCompSignal("PowerTurnedOn");
				SoundDef soundDef2 = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
				if (soundDef2.NullOrUndefined())
				{
					soundDef2 = SoundDefOf.Power_OnSmall;
				}
				soundDef2.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				this.StartSustainerPoweredIfInactive();
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003F7E RID: 16254 RVA: 0x00151924 File Offset: 0x0014FB24
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.parent.LabelCap + " CompPower:");
				stringBuilder.AppendLine("   PowerOn: " + this.PowerOn.ToString());
				stringBuilder.AppendLine("   energyProduction: " + this.PowerOutput);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00151994 File Offset: 0x0014FB94
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "ScheduledOff" || signal == "Breakdown")
			{
				this.PowerOn = false;
			}
			if (signal == "RanOutOfFuel" && this.powerLastOutputted)
			{
				this.PowerOn = false;
			}
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x001519EB File Offset: 0x0014FBEB
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.flickableComp = this.parent.GetComp<CompFlickable>();
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00151A05 File Offset: 0x0014FC05
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.EndSustainerPoweredIfActive();
			this.powerOutputInt = 0f;
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00151A1F File Offset: 0x0014FC1F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x00151A3C File Offset: 0x0014FC3C
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.PowerOff);
					return;
				}
				if (FlickUtility.WantsToBeOn(this.parent) && !this.PowerOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.NeedsPower);
				}
			}
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x00151AC0 File Offset: 0x0014FCC0
		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			this.PowerOutput = -1f * props.basePowerConsumption;
			this.powerLastOutputted = (props.basePowerConsumption <= 0f);
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x00151B02 File Offset: 0x0014FD02
		public override void ResetPowerVars()
		{
			base.ResetPowerVars();
			this.powerOnInt = false;
			this.powerOutputInt = 0f;
			this.powerLastOutputted = false;
			this.sustainerPowered = null;
			if (this.flickableComp != null)
			{
				this.flickableComp.ResetToOn();
			}
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00151B3D File Offset: 0x0014FD3D
		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x00151B4C File Offset: 0x0014FD4C
		public override string CompInspectStringExtra()
		{
			string str;
			if (this.powerLastOutputted)
			{
				str = "PowerOutput".Translate() + ": " + this.PowerOutput.ToString("#####0") + " W";
			}
			else
			{
				str = "PowerNeeded".Translate() + ": " + (-this.PowerOutput).ToString("#####0") + " W";
			}
			return str + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x00151BF0 File Offset: 0x0014FDF0
		private void StartSustainerPoweredIfInactive()
		{
			CompProperties_Power props = base.Props;
			if (!props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				this.sustainerPowered = props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x00151C3D File Offset: 0x0014FE3D
		private void EndSustainerPoweredIfActive()
		{
			if (this.sustainerPowered != null)
			{
				this.sustainerPowered.End();
				this.sustainerPowered = null;
			}
		}

		// Token: 0x040024F0 RID: 9456
		public Action powerStartedAction;

		// Token: 0x040024F1 RID: 9457
		public Action powerStoppedAction;

		// Token: 0x040024F2 RID: 9458
		private bool powerOnInt;

		// Token: 0x040024F3 RID: 9459
		public float powerOutputInt;

		// Token: 0x040024F4 RID: 9460
		private bool powerLastOutputted;

		// Token: 0x040024F5 RID: 9461
		private Sustainer sustainerPowered;

		// Token: 0x040024F6 RID: 9462
		protected CompFlickable flickableComp;

		// Token: 0x040024F7 RID: 9463
		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		// Token: 0x040024F8 RID: 9464
		public const string PowerTurnedOffSignal = "PowerTurnedOff";
	}
}
