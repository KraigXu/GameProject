using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Pawn_AbilityTracker : IExposable
	{
		
		public Pawn_AbilityTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void AbilitiesTick()
		{
			for (int i = 0; i < this.abilities.Count; i++)
			{
				this.abilities[i].AbilityTick();
			}
		}

		
		public void GainAbility(AbilityDef def)
		{
			if (!this.abilities.Any((Ability a) => a.def == def))
			{
				this.abilities.Add(Activator.CreateInstance(def.abilityClass, new object[]
				{
					this.pawn,
					def
				}) as Ability);
			}
		}

		
		public void RemoveAbility(AbilityDef def)
		{
			Ability ability = this.abilities.FirstOrDefault((Ability x) => x.def == def);
			if (ability != null)
			{
				this.abilities.Remove(ability);
			}
		}

		
		public Ability GetAbility(AbilityDef def)
		{
			for (int i = 0; i < this.abilities.Count; i++)
			{
				if (this.abilities[i].def == def)
				{
					return this.abilities[i];
				}
			}
			return null;
		}

		
		public IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Ability ability in from a in this.abilities
			orderby a.def.level, a.def.EntropyGain
			select a)
			{
				if (this.pawn.Drafted || ability.def.displayGizmoWhileUndrafted)
				{
					foreach (Command command in ability.GetGizmos())
					{
						yield return command;
					}
					IEnumerator<Command> enumerator2 = null;
				}
			}
			IEnumerator<Ability> enumerator = null;
			yield break;
			yield break;
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.abilities.RemoveAll((Ability a) => a.def == null);
			}
		}

		
		public Pawn pawn;

		
		public List<Ability> abilities = new List<Ability>();
	}
}
