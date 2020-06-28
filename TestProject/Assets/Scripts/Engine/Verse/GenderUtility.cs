using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000455 RID: 1109
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x06002116 RID: 8470 RVA: 0x000CB090 File Offset: 0x000C9290
		public static string GetGenderLabel(this Pawn pawn)
		{
			return pawn.gender.GetLabel(pawn.RaceProps.Animal);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000CB0A8 File Offset: 0x000C92A8
		public static string GetLabel(this Gender gender, bool animal = false)
		{
			switch (gender)
			{
			case Gender.None:
				return "NoneLower".Translate();
			case Gender.Male:
				return animal ? "MaleAnimal".Translate() : "Male".Translate();
			case Gender.Female:
				return animal ? "FemaleAnimal".Translate() : "Female".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000CB11C File Offset: 0x000C931C
		public static string GetPronoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proit".Translate();
			case Gender.Male:
				return "Prohe".Translate();
			case Gender.Female:
				return "Proshe".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000CB174 File Offset: 0x000C9374
		public static string GetPossessive(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proits".Translate();
			case Gender.Male:
				return "Prohis".Translate();
			case Gender.Female:
				return "Proher".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000CB1CC File Offset: 0x000C93CC
		public static string GetObjective(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProitObj".Translate();
			case Gender.Male:
				return "ProhimObj".Translate();
			case Gender.Female:
				return "ProherObj".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000CB222 File Offset: 0x000C9422
		public static Texture2D GetIcon(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return GenderUtility.GenderlessIcon;
			case Gender.Male:
				return GenderUtility.MaleIcon;
			case Gender.Female:
				return GenderUtility.FemaleIcon;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000CB24F File Offset: 0x000C944F
		public static Gender Opposite(this Gender gender)
		{
			if (gender == Gender.Male)
			{
				return Gender.Female;
			}
			if (gender == Gender.Female)
			{
				return Gender.Male;
			}
			return Gender.None;
		}

		// Token: 0x0400142C RID: 5164
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x0400142D RID: 5165
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x0400142E RID: 5166
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);
	}
}
