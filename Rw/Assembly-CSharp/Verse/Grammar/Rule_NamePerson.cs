using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x020004C9 RID: 1225
	public class Rule_NamePerson : Rule
	{
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000D84C2 File Offset: 0x000D66C2
		public override Rule DeepCopy()
		{
			Rule_NamePerson rule_NamePerson = (Rule_NamePerson)base.DeepCopy();
			rule_NamePerson.gender = this.gender;
			return rule_NamePerson;
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000D84DC File Offset: 0x000D66DC
		public override string Generate()
		{
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			Gender gender = this.gender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			return nameBank.GetName(PawnNameSlot.First, gender, false);
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000D8512 File Offset: 0x000D6712
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x040015D1 RID: 5585
		public Gender gender;
	}
}
