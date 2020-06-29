using System;
using System.Linq;

namespace Verse
{
	
	public class RegionLink
	{
		
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00047ABF File Offset: 0x00045CBF
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x00047AC9 File Offset: 0x00045CC9
		public Region RegionA
		{
			get
			{
				return this.regions[0];
			}
			set
			{
				this.regions[0] = value;
			}
		}

		
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00047AD4 File Offset: 0x00045CD4
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x00047ADE File Offset: 0x00045CDE
		public Region RegionB
		{
			get
			{
				return this.regions[1];
			}
			set
			{
				this.regions[1] = value;
			}
		}

		
		public void Register(Region reg)
		{
			if (this.regions[0] == reg || this.regions[1] == reg)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to double-register region ",
					reg.ToString(),
					" in ",
					this
				}), false);
				return;
			}
			if (this.RegionA == null || !this.RegionA.valid)
			{
				this.RegionA = reg;
				return;
			}
			if (this.RegionB == null || !this.RegionB.valid)
			{
				this.RegionB = reg;
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Could not register region ",
				reg.ToString(),
				" in link ",
				this,
				": > 2 regions on link!\nRegionA: ",
				this.RegionA.DebugString,
				"\nRegionB: ",
				this.RegionB.DebugString
			}), false);
		}

		
		public void Deregister(Region reg)
		{
			if (this.RegionA == reg)
			{
				this.RegionA = null;
				if (this.RegionB == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
					return;
				}
			}
			else if (this.RegionB == reg)
			{
				this.RegionB = null;
				if (this.RegionA == null)
				{
					reg.Map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
		}

		
		public Region GetOtherRegion(Region reg)
		{
			if (reg != this.RegionA)
			{
				return this.RegionA;
			}
			return this.RegionB;
		}

		
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false);
			string text2 = string.Concat(new object[]
			{
				"span=",
				this.span.ToString(),
				" hash=",
				this.UniqueHashCode()
			});
			return string.Concat(new string[]
			{
				"(",
				text2,
				", regions=",
				text,
				")"
			});
		}

		
		public Region[] regions = new Region[2];

		
		public EdgeSpan span;
	}
}
