using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Area_NoRoof : Area
	{
		
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x00145E97 File Offset: 0x00144097
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x00145EA8 File Offset: 0x001440A8
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x00145EBE File Offset: 0x001440BE
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		
		public Area_NoRoof()
		{
		}

		
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
