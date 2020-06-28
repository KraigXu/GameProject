using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000119 RID: 281
	public class TickList
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0002492C File Offset: 0x00022B2C
		private int TickInterval
		{
			get
			{
				switch (this.tickType)
				{
				case TickerType.Normal:
					return 1;
				case TickerType.Rare:
					return 250;
				case TickerType.Long:
					return 2000;
				default:
					return -1;
				}
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00024968 File Offset: 0x00022B68
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000249C4 File Offset: 0x00022BC4
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00024A10 File Offset: 0x00022C10
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00024A60 File Offset: 0x00022C60
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00024A6E File Offset: 0x00022C6E
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00024A7C File Offset: 0x00022C7C
		public void Tick()
		{
			for (int i = 0; i < this.thingsToRegister.Count; i++)
			{
				this.BucketOf(this.thingsToRegister[i]).Add(this.thingsToRegister[i]);
			}
			this.thingsToRegister.Clear();
			for (int j = 0; j < this.thingsToDeregister.Count; j++)
			{
				this.BucketOf(this.thingsToDeregister[j]).Remove(this.thingsToDeregister[j]);
			}
			this.thingsToDeregister.Clear();
			if (DebugSettings.fastEcology)
			{
				Find.World.tileTemperatures.ClearCaches();
				for (int k = 0; k < this.thingLists.Count; k++)
				{
					List<Thing> list = this.thingLists[k];
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].def.category == ThingCategory.Plant)
						{
							list[l].TickLong();
						}
					}
				}
			}
			List<Thing> list2 = this.thingLists[Find.TickManager.TicksGame % this.TickInterval];
			for (int m = 0; m < list2.Count; m++)
			{
				if (!list2[m].Destroyed)
				{
					try
					{
						switch (this.tickType)
						{
						case TickerType.Normal:
							list2[m].Tick();
							break;
						case TickerType.Rare:
							list2[m].TickRare();
							break;
						case TickerType.Long:
							list2[m].TickLong();
							break;
						}
					}
					catch (Exception ex)
					{
						string text = list2[m].Spawned ? (" (at " + list2[m].Position + ")") : "";
						if (Prefs.DevMode)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToStringSafe<Thing>(),
								text,
								": ",
								ex
							}), false);
						}
						else
						{
							Log.ErrorOnce(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToStringSafe<Thing>(),
								text,
								". Suppressing further errors. Exception: ",
								ex
							}), list2[m].thingIDNumber ^ 576876901, false);
						}
					}
				}
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00024D00 File Offset: 0x00022F00
		private List<Thing> BucketOf(Thing t)
		{
			int num = t.GetHashCode();
			if (num < 0)
			{
				num *= -1;
			}
			int index = num % this.TickInterval;
			return this.thingLists[index];
		}

		// Token: 0x0400070E RID: 1806
		private TickerType tickType;

		// Token: 0x0400070F RID: 1807
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x04000710 RID: 1808
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x04000711 RID: 1809
		private List<Thing> thingsToDeregister = new List<Thing>();
	}
}
