using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ConceptDef : Def
	{
		
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x00129B0E File Offset: 0x00127D0E
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x00129B20 File Offset: 0x00127D20
		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.priority > 9999999f)
			{
				yield return "priority isn't set";
			}
			if (this.helpText.NullOrEmpty())
			{
				yield return "no help text";
			}
			if (this.TriggeredDirect && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			ConceptDef.tmpParseErrors.Clear();
			this.helpText.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
			int num;
			for (int i = 0; i < ConceptDef.tmpParseErrors.Count; i = num + 1)
			{
				yield return "helpText error: " + ConceptDef.tmpParseErrors[i];
				num = i;
			}
			yield break;
			yield break;
		}

		
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		
		public void HighlightAllTags()
		{
			if (this.highlightTags != null)
			{
				for (int i = 0; i < this.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.highlightTags[i]);
				}
			}
		}

		
		public float priority = float.MaxValue;

		
		public bool noteTeaches;

		
		public bool needsOpportunity;

		
		public bool opportunityDecays = true;

		
		public ProgramState gameMode = ProgramState.Playing;

		
		[MustTranslate]
		private string helpText;

		
		[NoTranslate]
		public List<string> highlightTags;

		
		private static List<string> tmpParseErrors = new List<string>();
	}
}
