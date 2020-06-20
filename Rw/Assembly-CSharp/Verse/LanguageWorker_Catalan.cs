using System;

namespace Verse
{
	// Token: 0x02000134 RID: 308
	public class LanguageWorker_Catalan : LanguageWorker
	{
		// Token: 0x060008B7 RID: 2231 RVA: 0x0002D923 File Offset: 0x0002BB23
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unes " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002D963 File Offset: 0x0002BB63
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "les " : "els ") + str;
			}
			return this.WithElLaArticle(str, gender, false);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002D998 File Offset: 0x0002BB98
		private string WithElLaArticle(string str, Gender gender, bool name)
		{
			if (str.Length == 0 || (!this.IsVowel(str[0]) && str[0] != 'h' && str[0] != 'H'))
			{
				return ((gender == Gender.Female) ? "la " : "el ") + str;
			}
			if (name)
			{
				return ((gender == Gender.Female) ? "l'" : "n'") + str;
			}
			return "l'" + str;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002DA10 File Offset: 0x0002BC10
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (gender == Gender.Female)
			{
				return number + "a";
			}
			if (number == 1 || number == 3)
			{
				return number + "r";
			}
			if (number == 2)
			{
				return number + "n";
			}
			if (number == 4)
			{
				return number + "t";
			}
			return number + "è";
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002DA85 File Offset: 0x0002BC85
		public bool IsVowel(char ch)
		{
			return "ieɛaoɔuəuàêèéòóüúIEƐAOƆUƏUÀÊÈÉÒÓÜÚ".IndexOf(ch) >= 0;
		}
	}
}
