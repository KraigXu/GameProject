using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ExpectationDef : Def
	{
		
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x00124DBC File Offset: 0x00122FBC
		public bool WealthTriggered
		{
			get
			{
				return this.maxMapWealth >= 0f;
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.order < 0)
			{
				yield return "order not defined";
			}
			yield break;
		}

		
		public int order = -1;

		
		public int thoughtStage = -1;

		
		public float maxMapWealth = -1f;

		
		public float joyToleranceDropPerDay;

		
		public int joyKindsNeeded;
	}
}
