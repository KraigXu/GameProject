using System;
using Verse;

namespace RimWorld
{
	
	public class CompNeurotrainer : CompUsable
	{
		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
			Scribe_Defs.Look<AbilityDef>(ref this.ability, "ability");
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			CompProperties_Neurotrainer compProperties_Neurotrainer = (CompProperties_Neurotrainer)props;
			this.ability = compProperties_Neurotrainer.ability;
			this.skill = compProperties_Neurotrainer.skill;
		}

		
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			return string.Format(base.Props.useLabel, (this.skill != null) ? this.skill.skillLabel : this.ability.label);
		}

		
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompNeurotrainer compNeurotrainer = other.TryGetComp<CompNeurotrainer>();
			return compNeurotrainer != null && compNeurotrainer.skill == this.skill && compNeurotrainer.ability == this.ability;
		}

		
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

		
		public SkillDef skill;

		
		public AbilityDef ability;
	}
}
