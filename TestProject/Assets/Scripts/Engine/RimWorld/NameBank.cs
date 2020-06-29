using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class NameBank
	{
		
		
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

		
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		
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

		
		public PawnNameCategory nameType;

		
		private List<string>[,] names;

		
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;
	}
}
