using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA5 RID: 2981
	public class Pawn_AbilityTracker : IExposable
	{
		// Token: 0x060045CF RID: 17871 RVA: 0x00179072 File Offset: 0x00177272
		public Pawn_AbilityTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x0017908C File Offset: 0x0017728C
		public void AbilitiesTick()
		{
			for (int i = 0; i < this.abilities.Count; i++)
			{
				this.abilities[i].AbilityTick();
			}
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x001790C0 File Offset: 0x001772C0
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

		// Token: 0x060045D2 RID: 17874 RVA: 0x0017912C File Offset: 0x0017732C
		public void RemoveAbility(AbilityDef def)
		{
			Ability ability = this.abilities.FirstOrDefault((Ability x) => x.def == def);
			if (ability != null)
			{
				this.abilities.Remove(ability);
			}
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x00179170 File Offset: 0x00177370
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

		// Token: 0x060045D4 RID: 17876 RVA: 0x001791B5 File Offset: 0x001773B5
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

		// Token: 0x060045D5 RID: 17877 RVA: 0x001791C8 File Offset: 0x001773C8
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

		// Token: 0x0400282A RID: 10282
		public Pawn pawn;

		// Token: 0x0400282B RID: 10283
		public List<Ability> abilities = new List<Ability>();
	}
}
