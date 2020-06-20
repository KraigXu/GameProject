using System;

namespace Verse
{
	// Token: 0x0200011D RID: 285
	public abstract class Entity
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000807 RID: 2055
		public abstract string LabelCap { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000808 RID: 2056
		public abstract string Label { get; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000255AA File Offset: 0x000237AA
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x000255AA File Offset: 0x000237AA
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x000255B2 File Offset: 0x000237B2
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x0600080C RID: 2060
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x0600080D RID: 2061
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x0600080E RID: 2062 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000255AA File Offset: 0x000237AA
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
