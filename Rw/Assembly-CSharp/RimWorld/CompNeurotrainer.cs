using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D32 RID: 3378
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x0600520A RID: 21002 RVA: 0x001B697F File Offset: 0x001B4B7F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
			Scribe_Defs.Look<AbilityDef>(ref this.ability, "ability");
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x001B69A8 File Offset: 0x001B4BA8
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			CompProperties_Neurotrainer compProperties_Neurotrainer = (CompProperties_Neurotrainer)props;
			this.ability = compProperties_Neurotrainer.ability;
			this.skill = compProperties_Neurotrainer.skill;
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x001B69DB File Offset: 0x001B4BDB
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			return string.Format(base.Props.useLabel, (this.skill != null) ? this.skill.skillLabel : this.ability.label);
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x001B6A10 File Offset: 0x001B4C10
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompNeurotrainer compNeurotrainer = other.TryGetComp<CompNeurotrainer>();
			return compNeurotrainer != null && compNeurotrainer.skill == this.skill && compNeurotrainer.ability == this.ability;
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x001B6A54 File Offset: 0x001B4C54
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompNeurotrainer compNeurotrainer = piece.TryGetComp<CompNeurotrainer>();
			if (compNeurotrainer != null)
			{
				compNeurotrainer.skill = this.skill;
				compNeurotrainer.ability = this.ability;
			}
		}

		// Token: 0x04002D39 RID: 11577
		public SkillDef skill;

		// Token: 0x04002D3A RID: 11578
		public AbilityDef ability;
	}
}
