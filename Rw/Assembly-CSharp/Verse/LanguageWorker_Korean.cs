using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x0200013D RID: 317
	public class LanguageWorker_Korean : LanguageWorker
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x0002EDD8 File Offset: 0x0002CFD8
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			str = this.ReplaceJosa(str);
			return str;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002EDED File Offset: 0x0002CFED
		public override string PostProcessedKeyedTranslation(string translation)
		{
			translation = base.PostProcessedKeyedTranslation(translation);
			translation = this.ReplaceJosa(translation);
			return translation;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0002EE04 File Offset: 0x0002D004
		public string ReplaceJosa(string src)
		{
			LanguageWorker_Korean.tmpStringBuilder.Length = 0;
			MatchCollection matchCollection = LanguageWorker_Korean.JosaPattern.Matches(src);
			int num = 0;
			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];
				LanguageWorker_Korean.JosaPair josaPair = LanguageWorker_Korean.JosaPatternPaired[match.Value];
				LanguageWorker_Korean.tmpStringBuilder.Append(src, num, match.Index - num);
				if (match.Index > 0)
				{
					char inChar = src[match.Index - 1];
					if ((match.Value != "(으)로" && this.HasJong(inChar)) || (match.Value == "(으)로" && this.HasJongExceptRieul(inChar)))
					{
						LanguageWorker_Korean.tmpStringBuilder.Append(josaPair.josa1);
					}
					else
					{
						LanguageWorker_Korean.tmpStringBuilder.Append(josaPair.josa2);
					}
				}
				else
				{
					LanguageWorker_Korean.tmpStringBuilder.Append(josaPair.josa1);
				}
				num = match.Index + match.Length;
			}
			LanguageWorker_Korean.tmpStringBuilder.Append(src, num, src.Length - num);
			return LanguageWorker_Korean.tmpStringBuilder.ToString();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002EF28 File Offset: 0x0002D128
		private bool HasJong(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return LanguageWorker_Korean.AlphabetEndPattern.Contains(inChar);
			}
			return (inChar - '가') % '\u001c' > '\0';
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002EF4C File Offset: 0x0002D14C
		private bool HasJongExceptRieul(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return false;
			}
			int num = (int)((inChar - '가') % '\u001c');
			return num != 8 && num != 0;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002EF79 File Offset: 0x0002D179
		private bool IsKorean(char inChar)
		{
			return inChar >= '가' && inChar <= '힣';
		}

		// Token: 0x04000777 RID: 1911
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x04000778 RID: 1912
		private static readonly Regex JosaPattern = new Regex("\\(이\\)가|\\(와\\)과|\\(을\\)를|\\(은\\)는|\\(아\\)야|\\(이\\)여|\\(으\\)로|\\(이\\)라");

		// Token: 0x04000779 RID: 1913
		private static readonly Dictionary<string, LanguageWorker_Korean.JosaPair> JosaPatternPaired = new Dictionary<string, LanguageWorker_Korean.JosaPair>
		{
			{
				"(이)가",
				new LanguageWorker_Korean.JosaPair("이", "가")
			},
			{
				"(와)과",
				new LanguageWorker_Korean.JosaPair("과", "와")
			},
			{
				"(을)를",
				new LanguageWorker_Korean.JosaPair("을", "를")
			},
			{
				"(은)는",
				new LanguageWorker_Korean.JosaPair("은", "는")
			},
			{
				"(아)야",
				new LanguageWorker_Korean.JosaPair("아", "야")
			},
			{
				"(이)여",
				new LanguageWorker_Korean.JosaPair("이여", "여")
			},
			{
				"(으)로",
				new LanguageWorker_Korean.JosaPair("으로", "로")
			},
			{
				"(이)라",
				new LanguageWorker_Korean.JosaPair("이라", "라")
			}
		};

		// Token: 0x0400077A RID: 1914
		private static readonly List<char> AlphabetEndPattern = new List<char>
		{
			'b',
			'c',
			'k',
			'l',
			'm',
			'n',
			'p',
			'q',
			't'
		};

		// Token: 0x02001398 RID: 5016
		private struct JosaPair
		{
			// Token: 0x060076C8 RID: 30408 RVA: 0x0028FA85 File Offset: 0x0028DC85
			public JosaPair(string josa1, string josa2)
			{
				this.josa1 = josa1;
				this.josa2 = josa2;
			}

			// Token: 0x04004A7F RID: 19071
			public readonly string josa1;

			// Token: 0x04004A80 RID: 19072
			public readonly string josa2;
		}
	}
}
