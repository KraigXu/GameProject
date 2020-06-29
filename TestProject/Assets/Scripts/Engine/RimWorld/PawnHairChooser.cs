using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class PawnHairChooser
	{
		
		public static HairDef RandomHairDefFor(Pawn pawn, FactionDef factionType)
		{
			IEnumerable<string> enumerable = PawnHairChooser.HairTagsFromBackstory(pawn);
			IEnumerable<string> enumerable2 = PawnHairChooser.HairTagsFromPawnKind(pawn);
			IEnumerable<string> chosen = (enumerable2.Count<string>() > 0) ? enumerable2 : enumerable;
			if (chosen.Count<string>() == 0)
			{
				chosen = factionType.hairTags;
			}
			return (from hair in DefDatabase<HairDef>.AllDefs
			where hair.hairTags.SharesElementWith(chosen)
			select hair).RandomElementByWeight((HairDef hair) => PawnHairChooser.HairChoiceLikelihoodFor(hair, pawn));
		}

		
		private static IEnumerable<string> HairTagsFromBackstory(Pawn pawn)
		{
			foreach (Backstory b in pawn.story.AllBackstories)
			{
				if (b.hairTags != null)
				{
					int num;
					for (int i = 0; i < b.hairTags.Count; i = num + 1)
					{
						yield return b.hairTags[i];
						num = i;
					}
				}
				
			}
			IEnumerator<Backstory> enumerator = null;
			yield break;
			yield break;
		}

		
		private static IEnumerable<string> HairTagsFromPawnKind(Pawn pawn)
		{
			if (pawn.kindDef.hairTags != null)
			{
				int num;
				for (int i = 0; i < pawn.kindDef.hairTags.Count; i = num + 1)
				{
					yield return pawn.kindDef.hairTags[i];
					num = i;
				}
			}
			yield break;
		}

		
		private static float HairChoiceLikelihoodFor(HairDef hair, Pawn pawn)
		{
			if (pawn.gender == Gender.None)
			{
				return 100f;
			}
			if (pawn.gender == Gender.Male)
			{
				switch (hair.hairGender)
				{
				case HairGender.Male:
					return 70f;
				case HairGender.MaleUsually:
					return 30f;
				case HairGender.Any:
					return 60f;
				case HairGender.FemaleUsually:
					return 5f;
				case HairGender.Female:
					return 1f;
				}
			}
			if (pawn.gender == Gender.Female)
			{
				switch (hair.hairGender)
				{
				case HairGender.Male:
					return 1f;
				case HairGender.MaleUsually:
					return 5f;
				case HairGender.Any:
					return 60f;
				case HairGender.FemaleUsually:
					return 30f;
				case HairGender.Female:
					return 70f;
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Unknown hair likelihood for ",
				hair,
				" with ",
				pawn
			}), false);
			return 0f;
		}
	}
}
