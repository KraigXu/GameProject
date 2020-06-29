using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Area_Home : Area
	{
		
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x00145DCC File Offset: 0x00143FCC
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x00145DDD File Offset: 0x00143FDD
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x00145DF3 File Offset: 0x00143FF3
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		
		public Area_Home()
		{
		}

		
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		
		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] == val)
			{
				return;
			}
			base.Set(c, val);
			base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
		}
	}
}
