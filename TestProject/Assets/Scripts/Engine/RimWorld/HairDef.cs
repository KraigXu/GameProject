using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CB RID: 2251
	public class HairDef : Def
	{
		// Token: 0x04001E53 RID: 7763
		[NoTranslate]
		public string texPath;

		// Token: 0x04001E54 RID: 7764
		public HairGender hairGender = HairGender.Any;

		// Token: 0x04001E55 RID: 7765
		[NoTranslate]
		public List<string> hairTags = new List<string>();
	}
}
