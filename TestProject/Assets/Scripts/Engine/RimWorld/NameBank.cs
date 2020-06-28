using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B12 RID: 2834
	public class NameBank
	{
		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x00166361 File Offset: 0x00164561
		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				int num;
				for (int i = 0; i < NameBank.numGenders; i = num + 1)
				{
					for (int j = 0; j < NameBank.numSlots; j = num + 1)
					{
						yield return this.names[i, j];
						num = j;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00166374 File Offset: 0x00164574
		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x001663D8 File Offset: 0x001645D8
		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				foreach (string str in (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>())
				{
					Log.Error("Duplicated name: " + str, false);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]", false);
					}
				}
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x00166560 File Offset: 0x00164760
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x00166570 File Offset: 0x00164770
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x001665C0 File Offset: 0x001647C0
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x001665DC File Offset: 0x001647DC
		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Name list for gender=",
					gender,
					" slot=",
					slot,
					" is empty."
				}), false);
				return "Errorname";
			}
			string text;
			for (;;)
			{
				text = list.RandomElement<string>();
				if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
				{
					break;
				}
				num++;
				if (num > 50)
				{
					return text;
				}
			}
			return text;
		}

		// Token: 0x04002659 RID: 9817
		public PawnNameCategory nameType;

		// Token: 0x0400265A RID: 9818
		private List<string>[,] names;

		// Token: 0x0400265B RID: 9819
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		// Token: 0x0400265C RID: 9820
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;
	}
}
