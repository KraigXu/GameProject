using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A83 RID: 2691
	public class PowerNet
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003F9A RID: 16282 RVA: 0x00151FE4 File Offset: 0x001501E4
		public Map Map
		{
			get
			{
				return this.powerNetManager.map;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06003F9B RID: 16283 RVA: 0x00151FF4 File Offset: 0x001501F4
		public bool HasActivePowerSource
		{
			get
			{
				if (!this.hasPowerSource)
				{
					return false;
				}
				for (int i = 0; i < this.transmitters.Count; i++)
				{
					if (this.IsActivePowerSource(this.transmitters[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x00152038 File Offset: 0x00150238
		public PowerNet(IEnumerable<CompPower> newTransmitters)
		{
			foreach (CompPower compPower in newTransmitters)
			{
				this.transmitters.Add(compPower);
				compPower.transNet = this;
				this.RegisterAllComponentsOf(compPower.parent);
				if (compPower.connectChildren != null)
				{
					List<CompPower> connectChildren = compPower.connectChildren;
					for (int i = 0; i < connectChildren.Count; i++)
					{
						this.RegisterConnector(connectChildren[i]);
					}
				}
			}
			this.hasPowerSource = false;
			for (int j = 0; j < this.transmitters.Count; j++)
			{
				if (this.IsPowerSource(this.transmitters[j]))
				{
					this.hasPowerSource = true;
					return;
				}
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x00152140 File Offset: 0x00150340
		private bool IsPowerSource(CompPower cp)
		{
			return cp is CompPowerBattery || (cp is CompPowerTrader && cp.Props.basePowerConsumption < 0f);
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0015216C File Offset: 0x0015036C
		private bool IsActivePowerSource(CompPower cp)
		{
			CompPowerBattery compPowerBattery = cp as CompPowerBattery;
			if (compPowerBattery != null && compPowerBattery.StoredEnergy > 0f)
			{
				return true;
			}
			CompPowerTrader compPowerTrader = cp as CompPowerTrader;
			return compPowerTrader != null && compPowerTrader.PowerOutput > 0f;
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x001521AC File Offset: 0x001503AC
		public void RegisterConnector(CompPower b)
		{
			if (this.connectors.Contains(b))
			{
				Log.Error("PowerNet registered connector it already had: " + b, false);
				return;
			}
			this.connectors.Add(b);
			this.RegisterAllComponentsOf(b.parent);
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x001521E6 File Offset: 0x001503E6
		public void DeregisterConnector(CompPower b)
		{
			this.connectors.Remove(b);
			this.DeregisterAllComponentsOf(b.parent);
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x00152204 File Offset: 0x00150404
		private void RegisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				if (this.powerComps.Contains(comp))
				{
					Log.Error("PowerNet adding powerComp " + comp + " which it already has.", false);
				}
				else
				{
					this.powerComps.Add(comp);
				}
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				if (this.batteryComps.Contains(comp2))
				{
					Log.Error("PowerNet adding batteryComp " + comp2 + " which it already has.", false);
					return;
				}
				this.batteryComps.Add(comp2);
			}
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x00152288 File Offset: 0x00150488
		private void DeregisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				this.powerComps.Remove(comp);
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				this.batteryComps.Remove(comp2);
			}
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x001522C4 File Offset: 0x001504C4
		public float CurrentEnergyGainRate()
		{
			if (DebugSettings.unlimitedPower)
			{
				return 100000f;
			}
			float num = 0f;
			for (int i = 0; i < this.powerComps.Count; i++)
			{
				if (this.powerComps[i].PowerOn)
				{
					num += this.powerComps[i].EnergyOutputPerTick;
				}
			}
			return num;
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00152324 File Offset: 0x00150524
		public float CurrentStoredEnergy()
		{
			float num = 0f;
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				num += this.batteryComps[i].StoredEnergy;
			}
			return num;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x00152364 File Offset: 0x00150564
		public void PowerNetTick()
		{
			float num = this.CurrentEnergyGainRate();
			float num2 = this.CurrentStoredEnergy();
			if (num2 + num >= -1E-07f && !this.Map.gameConditionManager.ElectricityDisabled)
			{
				float num3;
				if (this.batteryComps.Count > 0 && num2 >= 0.1f)
				{
					num3 = num2 - 5f;
				}
				else
				{
					num3 = num2;
				}
				if (num3 + num >= 0f)
				{
					PowerNet.partsWantingPowerOn.Clear();
					for (int i = 0; i < this.powerComps.Count; i++)
					{
						if (!this.powerComps[i].PowerOn && FlickUtility.WantsToBeOn(this.powerComps[i].parent) && !this.powerComps[i].parent.IsBrokenDown())
						{
							PowerNet.partsWantingPowerOn.Add(this.powerComps[i]);
						}
					}
					if (PowerNet.partsWantingPowerOn.Count > 0)
					{
						int num4 = 200 / PowerNet.partsWantingPowerOn.Count;
						if (num4 < 30)
						{
							num4 = 30;
						}
						if (Find.TickManager.TicksGame % num4 == 0)
						{
							int num5 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.partsWantingPowerOn.Count * 0.05f));
							for (int j = 0; j < num5; j++)
							{
								CompPowerTrader compPowerTrader = PowerNet.partsWantingPowerOn.RandomElement<CompPowerTrader>();
								if (!compPowerTrader.PowerOn && num + num2 >= -(compPowerTrader.EnergyOutputPerTick + 1E-07f))
								{
									compPowerTrader.PowerOn = true;
									num += compPowerTrader.EnergyOutputPerTick;
								}
							}
						}
					}
				}
				this.ChangeStoredEnergy(num);
				return;
			}
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				PowerNet.potentialShutdownParts.Clear();
				for (int k = 0; k < this.powerComps.Count; k++)
				{
					if (this.powerComps[k].PowerOn && this.powerComps[k].EnergyOutputPerTick < 0f)
					{
						PowerNet.potentialShutdownParts.Add(this.powerComps[k]);
					}
				}
				if (PowerNet.potentialShutdownParts.Count > 0)
				{
					int num6 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.potentialShutdownParts.Count * 0.05f));
					for (int l = 0; l < num6; l++)
					{
						PowerNet.potentialShutdownParts.RandomElement<CompPowerTrader>().PowerOn = false;
					}
				}
			}
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x001525BC File Offset: 0x001507BC
		private void ChangeStoredEnergy(float extra)
		{
			if (extra > 0f)
			{
				this.DistributeEnergyAmongBatteries(extra);
				return;
			}
			float num = -extra;
			this.givingBats.Clear();
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				if (this.batteryComps[i].StoredEnergy > 1E-07f)
				{
					this.givingBats.Add(this.batteryComps[i]);
				}
			}
			float a = num / (float)this.givingBats.Count;
			int num2 = 0;
			while (num > 1E-07f)
			{
				for (int j = 0; j < this.givingBats.Count; j++)
				{
					float num3 = Mathf.Min(a, this.givingBats[j].StoredEnergy);
					this.givingBats[j].DrawPower(num3);
					num -= num3;
					if (num < 1E-07f)
					{
						return;
					}
				}
				num2++;
				if (num2 > 10)
				{
					break;
				}
			}
			if (num > 1E-07f)
			{
				Log.Warning("Drew energy from a PowerNet that didn't have it.", false);
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x001526BC File Offset: 0x001508BC
		private void DistributeEnergyAmongBatteries(float energy)
		{
			if (energy <= 0f || !this.batteryComps.Any<CompPowerBattery>())
			{
				return;
			}
			PowerNet.batteriesShuffled.Clear();
			PowerNet.batteriesShuffled.AddRange(this.batteryComps);
			PowerNet.batteriesShuffled.Shuffle<CompPowerBattery>();
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 10000)
				{
					break;
				}
				float num2 = float.MaxValue;
				for (int i = 0; i < PowerNet.batteriesShuffled.Count; i++)
				{
					num2 = Mathf.Min(num2, PowerNet.batteriesShuffled[i].AmountCanAccept);
				}
				if (energy < num2 * (float)PowerNet.batteriesShuffled.Count)
				{
					goto IL_101;
				}
				for (int j = PowerNet.batteriesShuffled.Count - 1; j >= 0; j--)
				{
					float amountCanAccept = PowerNet.batteriesShuffled[j].AmountCanAccept;
					bool flag = amountCanAccept <= 0f || amountCanAccept == num2;
					if (num2 > 0f)
					{
						PowerNet.batteriesShuffled[j].AddEnergy(num2);
						energy -= num2;
					}
					if (flag)
					{
						PowerNet.batteriesShuffled.RemoveAt(j);
					}
				}
				if (energy < 0.0005f || !PowerNet.batteriesShuffled.Any<CompPowerBattery>())
				{
					goto IL_15C;
				}
			}
			Log.Error("Too many iterations.", false);
			goto IL_15C;
			IL_101:
			float amount = energy / (float)PowerNet.batteriesShuffled.Count;
			for (int k = 0; k < PowerNet.batteriesShuffled.Count; k++)
			{
				PowerNet.batteriesShuffled[k].AddEnergy(amount);
			}
			energy = 0f;
			IL_15C:
			PowerNet.batteriesShuffled.Clear();
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00152830 File Offset: 0x00150A30
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("POWERNET:");
			stringBuilder.AppendLine("  Created energy: " + this.debugLastCreatedEnergy);
			stringBuilder.AppendLine("  Raw stored energy: " + this.debugLastRawStoredEnergy);
			stringBuilder.AppendLine("  Apparent stored energy: " + this.debugLastApparentStoredEnergy);
			stringBuilder.AppendLine("  hasPowerSource: " + this.hasPowerSource.ToString());
			stringBuilder.AppendLine("  Connectors: ");
			foreach (CompPower compPower in this.connectors)
			{
				stringBuilder.AppendLine("      " + compPower.parent);
			}
			stringBuilder.AppendLine("  Transmitters: ");
			foreach (CompPower compPower2 in this.transmitters)
			{
				stringBuilder.AppendLine("      " + compPower2.parent);
			}
			stringBuilder.AppendLine("  powerComps: ");
			foreach (CompPowerTrader compPowerTrader in this.powerComps)
			{
				stringBuilder.AppendLine("      " + compPowerTrader.parent);
			}
			stringBuilder.AppendLine("  batteryComps: ");
			foreach (CompPowerBattery compPowerBattery in this.batteryComps)
			{
				stringBuilder.AppendLine("      " + compPowerBattery.parent);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040024FD RID: 9469
		public PowerNetManager powerNetManager;

		// Token: 0x040024FE RID: 9470
		public bool hasPowerSource;

		// Token: 0x040024FF RID: 9471
		public List<CompPower> connectors = new List<CompPower>();

		// Token: 0x04002500 RID: 9472
		public List<CompPower> transmitters = new List<CompPower>();

		// Token: 0x04002501 RID: 9473
		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		// Token: 0x04002502 RID: 9474
		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		// Token: 0x04002503 RID: 9475
		private float debugLastCreatedEnergy;

		// Token: 0x04002504 RID: 9476
		private float debugLastRawStoredEnergy;

		// Token: 0x04002505 RID: 9477
		private float debugLastApparentStoredEnergy;

		// Token: 0x04002506 RID: 9478
		private const int MaxRestartTryInterval = 200;

		// Token: 0x04002507 RID: 9479
		private const int MinRestartTryInterval = 30;

		// Token: 0x04002508 RID: 9480
		private const float RestartMinFraction = 0.05f;

		// Token: 0x04002509 RID: 9481
		private const int ShutdownInterval = 20;

		// Token: 0x0400250A RID: 9482
		private const float ShutdownMinFraction = 0.05f;

		// Token: 0x0400250B RID: 9483
		private const float MinStoredEnergyToTurnOn = 5f;

		// Token: 0x0400250C RID: 9484
		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		// Token: 0x0400250D RID: 9485
		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		// Token: 0x0400250E RID: 9486
		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		// Token: 0x0400250F RID: 9487
		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();
	}
}
