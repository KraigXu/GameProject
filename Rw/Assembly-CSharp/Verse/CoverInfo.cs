using System;

namespace Verse
{
	// Token: 0x02000475 RID: 1141
	public struct CoverInfo
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x000CD3D6 File Offset: 0x000CB5D6
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x000CD3DE File Offset: 0x000CB5DE
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000CD3E6 File Offset: 0x000CB5E6
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000CD3F3 File Offset: 0x000CB5F3
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x040014A4 RID: 5284
		private Thing thingInt;

		// Token: 0x040014A5 RID: 5285
		private float blockChanceInt;
	}
}
