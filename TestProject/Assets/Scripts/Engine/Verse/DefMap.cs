using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200040B RID: 1035
	public class DefMap<D, V> : IExposable, IEnumerable<KeyValuePair<D, V>>, IEnumerable where D : Def, new() where V : new()
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x000BE800 File Offset: 0x000BCA00
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x170005C2 RID: 1474
		public V this[D def]
		{
			get
			{
				return this.values[(int)def.index];
			}
			set
			{
				this.values[(int)def.index] = value;
			}
		}

		// Token: 0x170005C3 RID: 1475
		public V this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000BE85C File Offset: 0x000BCA5C
		public DefMap()
		{
			int defCount = DefDatabase<D>.DefCount;
			if (defCount == 0)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Constructed DefMap<",
					typeof(D),
					", ",
					typeof(V),
					"> without defs being initialized. Try constructing it in ResolveReferences instead of the constructor."
				}));
			}
			this.values = new List<V>(defCount);
			for (int i = 0; i < defCount; i++)
			{
				this.values.Add(Activator.CreateInstance<V>());
			}
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000BE8E4 File Offset: 0x000BCAE4
		public void ExposeData()
		{
			Scribe_Collections.Look<V>(ref this.values, "vals", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int defCount = DefDatabase<D>.DefCount;
				for (int i = this.values.Count; i < defCount; i++)
				{
					this.values.Add(Activator.CreateInstance<V>());
				}
				while (this.values.Count > defCount)
				{
					this.values.RemoveLast<V>();
				}
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000BE958 File Offset: 0x000BCB58
		public void SetAll(V val)
		{
			for (int i = 0; i < this.values.Count; i++)
			{
				this.values[i] = val;
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000BE988 File Offset: 0x000BCB88
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000BE990 File Offset: 0x000BCB90
		public IEnumerator<KeyValuePair<D, V>> GetEnumerator()
		{
			return (from d in DefDatabase<D>.AllDefsListForReading
			select new KeyValuePair<D, V>(d, this[d])).GetEnumerator();
		}

		// Token: 0x040012E6 RID: 4838
		private List<V> values;
	}
}
