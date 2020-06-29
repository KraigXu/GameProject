using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class NeedDef : Def
	{
		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public Type needClass;

		
		public Intelligence minIntelligence;

		
		public bool colonistAndPrisonersOnly;

		
		public bool colonistsOnly;

		
		public bool onlyIfCausedByHediff;

		
		public bool neverOnPrisoner;

		
		public List<RoyalTitleDef> titleRequiredAny;

		
		public List<HediffDef> hediffRequiredAny;

		
		public bool showOnNeedList = true;

		
		public float baseLevel = 0.5f;

		
		public bool major;

		
		public int listPriority;

		
		[NoTranslate]
		public string tutorHighlightTag;

		
		public bool showForCaravanMembers;

		
		public bool scaleBar;

		
		public float fallPerDay = 0.5f;

		
		public float seekerRisePerHour;

		
		public float seekerFallPerHour;

		
		public bool freezeWhileSleeping;

		
		public bool freezeInMentalState;
	}
}
