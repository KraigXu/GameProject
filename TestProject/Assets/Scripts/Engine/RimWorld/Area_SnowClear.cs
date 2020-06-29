using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Area_SnowClear : Area
	{
		
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x00145EE1 File Offset: 0x001440E1
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x00145EF2 File Offset: 0x001440F2
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x0013A1CF File Offset: 0x001383CF
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		
		public Area_SnowClear()
		{
		}

		
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
