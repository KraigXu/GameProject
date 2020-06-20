using System;

namespace Verse
{
	// Token: 0x02000133 RID: 307
	public abstract class LanguageWorker
	{
		// Token: 0x060008A8 RID: 2216 RVA: 0x0002D7B0 File Offset: 0x0002B9B0
		public virtual string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("IndefiniteForm".CanTranslate())
			{
				return "IndefiniteForm".Translate(str);
			}
			return "IndefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0002D812 File Offset: 0x0002BA12
		public string WithIndefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithIndefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), plural, name);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002D829 File Offset: 0x0002BA29
		public string WithIndefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002D83C File Offset: 0x0002BA3C
		public string WithIndefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, plural, name));
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0002D850 File Offset: 0x0002BA50
		public virtual string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("DefiniteForm".CanTranslate())
			{
				return "DefiniteForm".Translate(str);
			}
			return "DefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0002D8B2 File Offset: 0x0002BAB2
		public string WithDefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithDefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), plural, name);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002D8C9 File Offset: 0x0002BAC9
		public string WithDefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0002D8DC File Offset: 0x0002BADC
		public string WithDefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, plural, name));
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002D8ED File Offset: 0x0002BAED
		public virtual string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString();
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002D8F6 File Offset: 0x0002BAF6
		public virtual string PostProcessed(string str)
		{
			str = str.MergeMultipleSpaces(true);
			return str;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0002D902 File Offset: 0x0002BB02
		public virtual string ToTitleCase(string str)
		{
			return str.CapitalizeFirst();
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0002D90A File Offset: 0x0002BB0A
		public virtual string Pluralize(string str, Gender gender, int count = -1)
		{
			return str;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0002D90D File Offset: 0x0002BB0D
		public string Pluralize(string str, int count = -1)
		{
			return this.Pluralize(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), count);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002D90A File Offset: 0x0002BB0A
		public virtual string PostProcessedKeyedTranslation(string translation)
		{
			return translation;
		}
	}
}
