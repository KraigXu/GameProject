using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091D RID: 2333
	public class ConceptDef : Def
	{
		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x00129B0E File Offset: 0x00127D0E
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x00129B20 File Offset: 0x00127D20
		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x00129B2F File Offset: 0x00127D2F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
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

		// Token: 0x06003769 RID: 14185 RVA: 0x00129B3F File Offset: 0x00127D3F
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x00129B48 File Offset: 0x00127D48
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

		// Token: 0x040020B5 RID: 8373
		public float priority = float.MaxValue;

		// Token: 0x040020B6 RID: 8374
		public bool noteTeaches;

		// Token: 0x040020B7 RID: 8375
		public bool needsOpportunity;

		// Token: 0x040020B8 RID: 8376
		public bool opportunityDecays = true;

		// Token: 0x040020B9 RID: 8377
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x040020BA RID: 8378
		[MustTranslate]
		private string helpText;

		// Token: 0x040020BB RID: 8379
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x040020BC RID: 8380
		private static List<string> tmpParseErrors = new List<string>();
	}
}
