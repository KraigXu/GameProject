using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B15 RID: 2837
	[StaticConstructorOnStartup]
	public static class PawnNameDatabaseShuffled
	{
		// Token: 0x060042C8 RID: 17096 RVA: 0x00166690 File Offset: 0x00164890
		static PawnNameDatabaseShuffled()
		{
			foreach (object obj in Enum.GetValues(typeof(PawnNameCategory)))
			{
				PawnNameCategory pawnNameCategory = (PawnNameCategory)obj;
				if (pawnNameCategory != PawnNameCategory.NoName)
				{
					PawnNameDatabaseShuffled.banks.Add(pawnNameCategory, new NameBank(pawnNameCategory));
				}
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Male, "First_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Female, "First_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Male, "Nick_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Female, "Nick_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.None, "Nick_Unisex");
			nameBank.AddNamesFromFile(PawnNameSlot.Last, Gender.None, "Last");
			foreach (NameBank nameBank2 in PawnNameDatabaseShuffled.banks.Values)
			{
				nameBank2.ErrorCheck();
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x001667A0 File Offset: 0x001649A0
		public static NameBank BankOf(PawnNameCategory category)
		{
			return PawnNameDatabaseShuffled.banks[category];
		}

		// Token: 0x04002665 RID: 9829
		private static Dictionary<PawnNameCategory, NameBank> banks = new Dictionary<PawnNameCategory, NameBank>();
	}
}
