using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Area_BuildRoof : Area
	{
		
		// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x00145E4D File Offset: 0x0014404D
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x00145E5E File Offset: 0x0014405E
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00145E74 File Offset: 0x00144074
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		
		public Area_BuildRoof()
		{
		}

		
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
