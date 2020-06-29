using System;

namespace Verse
{
	
	public struct CoverInfo
	{
		
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x000CD3D6 File Offset: 0x000CB5D6
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x000CD3DE File Offset: 0x000CB5DE
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000CD3E6 File Offset: 0x000CB5E6
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		
		private Thing thingInt;

		
		private float blockChanceInt;
	}
}
