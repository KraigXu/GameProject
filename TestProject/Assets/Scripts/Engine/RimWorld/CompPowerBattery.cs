using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompPowerBattery : CompPower
	{
		
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

		
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x0014FABF File Offset: 0x0014DCBF
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		
		// (get) Token: 0x06003F13 RID: 16147 RVA: 0x0014FAC7 File Offset: 0x0014DCC7
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x0014FADB File Offset: 0x0014DCDB
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		
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

		
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		
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

		
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				Log.Error("Drawing power we don't have from " + this.parent, false);
				this.storedEnergy = 0f;
			}
		}

		
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		
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

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{

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

		
		private float storedEnergy;

		
		private const float SelfDischargingWatts = 5f;
	}
}
