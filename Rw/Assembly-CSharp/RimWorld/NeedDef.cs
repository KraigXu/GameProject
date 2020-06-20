using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008E5 RID: 2277
	public class NeedDef : Def
	{
		// Token: 0x0600367E RID: 13950 RVA: 0x001275BA File Offset: 0x001257BA
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.description.NullOrEmpty() && this.showOnNeedList)
			{
				yield return "no description";
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
			}
			if (this.needClass == typeof(Need_Seeker) && (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f))
			{
				yield return "seeker rise/fall rates not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001EF9 RID: 7929
		public Type needClass;

		// Token: 0x04001EFA RID: 7930
		public Intelligence minIntelligence;

		// Token: 0x04001EFB RID: 7931
		public bool colonistAndPrisonersOnly;

		// Token: 0x04001EFC RID: 7932
		public bool colonistsOnly;

		// Token: 0x04001EFD RID: 7933
		public bool onlyIfCausedByHediff;

		// Token: 0x04001EFE RID: 7934
		public bool neverOnPrisoner;

		// Token: 0x04001EFF RID: 7935
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04001F00 RID: 7936
		public List<HediffDef> hediffRequiredAny;

		// Token: 0x04001F01 RID: 7937
		public bool showOnNeedList = true;

		// Token: 0x04001F02 RID: 7938
		public float baseLevel = 0.5f;

		// Token: 0x04001F03 RID: 7939
		public bool major;

		// Token: 0x04001F04 RID: 7940
		public int listPriority;

		// Token: 0x04001F05 RID: 7941
		[NoTranslate]
		public string tutorHighlightTag;

		// Token: 0x04001F06 RID: 7942
		public bool showForCaravanMembers;

		// Token: 0x04001F07 RID: 7943
		public bool scaleBar;

		// Token: 0x04001F08 RID: 7944
		public float fallPerDay = 0.5f;

		// Token: 0x04001F09 RID: 7945
		public float seekerRisePerHour;

		// Token: 0x04001F0A RID: 7946
		public float seekerFallPerHour;

		// Token: 0x04001F0B RID: 7947
		public bool freezeWhileSleeping;

		// Token: 0x04001F0C RID: 7948
		public bool freezeInMentalState;
	}
}
