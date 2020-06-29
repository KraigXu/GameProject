using System;

namespace Verse
{
	
	public abstract class Entity
	{
		
		// (get) Token: 0x06000807 RID: 2055
		public abstract string LabelCap { get; }

		
		// (get) Token: 0x06000808 RID: 2056
		public abstract string Label { get; }

		
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000255AA File Offset: 0x000237AA
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x000255AA File Offset: 0x000237AA
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x000255B2 File Offset: 0x000237B2
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
