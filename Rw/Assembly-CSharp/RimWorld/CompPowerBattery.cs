using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A75 RID: 2677
	public class CompPowerBattery : CompPower
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06003F11 RID: 16145 RVA: 0x0014FA84 File Offset: 0x0014DC84
		public float AmountCanAccept
		{
			get
			{
				if (this.parent.IsBrokenDown())
				{
					return 0f;
				}
				CompProperties_Battery props = this.Props;
				return (props.storedEnergyMax - this.storedEnergy) / props.efficiency;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x0014FABF File Offset: 0x0014DCBF
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003F13 RID: 16147 RVA: 0x0014FAC7 File Offset: 0x0014DCC7
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x0014FADB File Offset: 0x0014DCDB
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0014FAE8 File Offset: 0x0014DCE8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.storedEnergy, "storedPower", 0f, false);
			CompProperties_Battery props = this.Props;
			if (this.storedEnergy > props.storedEnergyMax)
			{
				this.storedEnergy = props.storedEnergyMax;
			}
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x0014FB32 File Offset: 0x0014DD32
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x0014FB58 File Offset: 0x0014DD58
		public void AddEnergy(float amount)
		{
			if (amount < 0f)
			{
				Log.Error("Cannot add negative energy " + amount, false);
				return;
			}
			if (amount > this.AmountCanAccept)
			{
				amount = this.AmountCanAccept;
			}
			amount *= this.Props.efficiency;
			this.storedEnergy += amount;
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0014FBB2 File Offset: 0x0014DDB2
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				Log.Error("Drawing power we don't have from " + this.parent, false);
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0014FBF0 File Offset: 0x0014DDF0
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0014FC0D File Offset: 0x0014DE0D
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0014FC28 File Offset: 0x0014DE28
		public override string CompInspectStringExtra()
		{
			CompProperties_Battery props = this.Props;
			string text = "PowerBatteryStored".Translate() + ": " + this.storedEnergy.ToString("F0") + " / " + props.storedEnergyMax.ToString("F0") + " Wd";
			text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (props.efficiency * 100f).ToString("F0") + "%";
			if (this.storedEnergy > 0f)
			{
				text += "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0014FD51 File Offset: 0x0014DF51
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Fill",
					action = delegate
					{
						this.SetStoredEnergyPct(1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Empty",
					action = delegate
					{
						this.SetStoredEnergyPct(0f);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x040024B9 RID: 9401
		private float storedEnergy;

		// Token: 0x040024BA RID: 9402
		private const float SelfDischargingWatts = 5f;
	}
}
