using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001170 RID: 4464
	public class QuestNode_GetRandomPawnKindForFaction : QuestNode
	{
		// Token: 0x060067C8 RID: 26568 RVA: 0x00244347 File Offset: 0x00242547
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x00244350 File Offset: 0x00242550
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x00244360 File Offset: 0x00242560
		private bool SetVars(Slate slate)
		{
			Thing value = this.factionOf.GetValue(slate);
			if (value == null)
			{
				return false;
			}
			Faction faction = value.Faction;
			if (faction == null)
			{
				return false;
			}
			List<QuestNode_GetRandomPawnKindForFaction.Choice> value2 = this.choices.GetValue(slate);
			for (int i = 0; i < value2.Count; i++)
			{
				PawnKindDef var;
				if (((value2[i].factionDef != null && faction.def == value2[i].factionDef) || (!value2[i].categoryTag.NullOrEmpty() && value2[i].categoryTag == faction.def.categoryTag)) && value2[i].pawnKinds.TryRandomElement(out var))
				{
					slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
					return true;
				}
			}
			if (this.fallback.GetValue(slate) != null)
			{
				slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.fallback.GetValue(slate), false);
				return true;
			}
			return false;
		}

		// Token: 0x04003FF1 RID: 16369
		public SlateRef<Thing> factionOf;

		// Token: 0x04003FF2 RID: 16370
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003FF3 RID: 16371
		public SlateRef<List<QuestNode_GetRandomPawnKindForFaction.Choice>> choices;

		// Token: 0x04003FF4 RID: 16372
		public SlateRef<PawnKindDef> fallback;

		// Token: 0x02001F4D RID: 8013
		public class Choice
		{
			// Token: 0x0400754A RID: 30026
			public FactionDef factionDef;

			// Token: 0x0400754B RID: 30027
			public string categoryTag;

			// Token: 0x0400754C RID: 30028
			public List<PawnKindDef> pawnKinds;
		}
	}
}
