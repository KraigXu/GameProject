using System;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000105 RID: 261
	public class BattleLogEntry_AbilityUsed : BattleLogEntry_Event
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x00020A3F File Offset: 0x0001EC3F
		public BattleLogEntry_AbilityUsed()
		{
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00020A47 File Offset: 0x0001EC47
		public BattleLogEntry_AbilityUsed(Pawn caster, Thing target, AbilityDef ability, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.abilityUsed = ability;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00020A5A File Offset: 0x0001EC5A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.abilityUsed, "abilityUsed");
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00020A74 File Offset: 0x0001EC74
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ABILITY", this.abilityUsed));
			if (this.subjectPawn == null && this.subjectThing == null)
			{
				result.Rules.Add(new Rule_String("SUBJECT_definite", "AreaLower".Translate()));
			}
			return result;
		}

		// Token: 0x04000696 RID: 1686
		public AbilityDef abilityUsed;
	}
}
