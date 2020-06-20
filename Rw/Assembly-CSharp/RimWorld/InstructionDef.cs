using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091E RID: 2334
	public class InstructionDef : Def
	{
		// Token: 0x0600376E RID: 14190 RVA: 0x00129BB1 File Offset: 0x00127DB1
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.instructionClass == null)
			{
				yield return "no instruction class";
			}
			if (this.text.NullOrEmpty())
			{
				yield return "no text";
			}
			if (this.eventTagInitiate.NullOrEmpty())
			{
				yield return "no eventTagInitiate";
			}
			InstructionDef.tmpParseErrors.Clear();
			this.text.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
			int num;
			for (int i = 0; i < InstructionDef.tmpParseErrors.Count; i = num + 1)
			{
				yield return "text error: " + InstructionDef.tmpParseErrors[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x040020BD RID: 8381
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x040020BE RID: 8382
		[MustTranslate]
		public string text;

		// Token: 0x040020BF RID: 8383
		public bool startCentered;

		// Token: 0x040020C0 RID: 8384
		public bool tutorialModeOnly = true;

		// Token: 0x040020C1 RID: 8385
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x040020C2 RID: 8386
		public InstructionDef eventTagInitiateSource;

		// Token: 0x040020C3 RID: 8387
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x040020C4 RID: 8388
		[NoTranslate]
		public List<string> actionTagsAllowed;

		// Token: 0x040020C5 RID: 8389
		[MustTranslate]
		public string rejectInputMessage;

		// Token: 0x040020C6 RID: 8390
		public ConceptDef concept;

		// Token: 0x040020C7 RID: 8391
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x040020C8 RID: 8392
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x040020C9 RID: 8393
		public int targetCount;

		// Token: 0x040020CA RID: 8394
		public ThingDef thingDef;

		// Token: 0x040020CB RID: 8395
		public RecipeDef recipeDef;

		// Token: 0x040020CC RID: 8396
		public int recipeTargetCount = 1;

		// Token: 0x040020CD RID: 8397
		public ThingDef giveOnActivateDef;

		// Token: 0x040020CE RID: 8398
		public int giveOnActivateCount;

		// Token: 0x040020CF RID: 8399
		public bool endTutorial;

		// Token: 0x040020D0 RID: 8400
		public bool resetBuildDesignatorStuffs;

		// Token: 0x040020D1 RID: 8401
		private static List<string> tmpParseErrors = new List<string>();
	}
}
