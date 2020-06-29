using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	
	public class LanguageWorker_French : LanguageWorker
	{
		
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "des " + str;
			}
			return ((gender == Gender.Female) ? "une " : "un ") + str;
		}

		
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "les " + str;
			}
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "l'" + str;
			}
			return ((gender == Gender.Female) ? "la " : "le ") + str;
		}

		
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (number != 1)
			{
				return number + "e";
			}
			return number + "er";
		}

		
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string item = str.ToLower();
			if (LanguageWorker_French.Exceptions1.Contains(item))
			{
				return str.Substring(0, str.Length - 3) + "aux";
			}
			if (LanguageWorker_French.Exceptions2.Contains(item))
			{
				return str + "s";
			}
			if (LanguageWorker_French.Exceptions3.Contains(item))
			{
				return str + "x";
			}
			if (str[str.Length - 1] == 's' || str[str.Length - 1] == 'x' || str[str.Length - 1] == 'z')
			{
				return str;
			}
			if (str.Length >= 2 && str[str.Length - 2] == 'a' && str[str.Length - 1] == 'l')
			{
				return str.Substring(0, str.Length - 2) + "aux";
			}
			if (str.Length >= 2 && str[str.Length - 2] == 'a' && str[str.Length - 1] == 'u')
			{
				return str.Substring(0, str.Length - 2) + "x";
			}
			if (str.Length >= 2 && str[str.Length - 2] == 'e' && str[str.Length - 1] == 'u')
			{
				return str.Substring(0, str.Length - 2) + "x";
			}
			return str + "s";
		}

		
		public override string PostProcessed(string str)
		{
			return this.PostProcessedInt(base.PostProcessed(str));
		}

		
		public override string PostProcessedKeyedTranslation(string translation)
		{
			return this.PostProcessedInt(base.PostProcessedKeyedTranslation(translation));
		}

		
		public bool IsVowel(char ch)
		{
			return "hiueøoɛœəɔaãɛ̃œ̃ɔ̃IHUEØOƐŒƏƆAÃƐ̃Œ̃Ɔ̃".IndexOf(ch) >= 0;
		}

		
		private string PostProcessedInt(string str)
		{
			str = str.Replace(" de le ", " du ").Replace("De le ", "Du ").Replace(" de les ", " des ").Replace("De les ", "Des ").Replace(" de des ", " des ").Replace("De des ", "Des ").Replace(" à le ", " au ").Replace("À le ", "Au ").Replace(" à les ", " aux ").Replace("À les ", "Aux ").Replace(" si il ", " s'il ").Replace("Si il ", "S'il ").Replace(" si ils ", " s'ils ").Replace("Si ils ", "S'ils ").Replace(" que il ", " qu'il ").Replace("Que il ", "Qu'il ").Replace(" que ils ", " qu'ils ").Replace("Que ils ", "Qu'ils ").Replace(" lorsque il ", " lorsqu'il ").Replace("Lorsque il ", "Lorsqu'il ").Replace(" lorsque ils ", " lorsqu'ils ").Replace("Lorsque ils ", "Lorsqu'ils ").Replace(" que elle ", " qu'elle ").Replace("Que elle ", "Qu'elle ").Replace(" que elles ", " qu'elles ").Replace("Que elles ", "Qu'elles ").Replace(" lorsque elle ", " lorsqu'elle ").Replace("Lorsque elle ", "Lorsqu'elle ").Replace(" lorsque elles ", " lorsqu'elles ").Replace("Lorsque elles ", "Lorsqu'elles ");
			LanguageWorker_French.tmpStr.Clear();
			LanguageWorker_French.tmpStr.Append(str);
			for (int i = 0; i < LanguageWorker_French.tmpStr.Length; i++)
			{
				if (i + 3 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == 'D' && LanguageWorker_French.tmpStr[i + 1] == 'e' && LanguageWorker_French.tmpStr[i + 2] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 3]))
				{
					LanguageWorker_French.tmpStr[i] = '\0';
					LanguageWorker_French.tmpStr[i + 1] = 'D';
					LanguageWorker_French.tmpStr[i + 2] = '\'';
				}
				else if (i + 3 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == 'L' && LanguageWorker_French.tmpStr[i + 1] == 'e' && LanguageWorker_French.tmpStr[i + 2] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 3]))
				{
					LanguageWorker_French.tmpStr[i] = '\0';
					LanguageWorker_French.tmpStr[i + 1] = 'L';
					LanguageWorker_French.tmpStr[i + 2] = '\'';
				}
				else if (i + 3 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == 'L' && LanguageWorker_French.tmpStr[i + 1] == 'a' && LanguageWorker_French.tmpStr[i + 2] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 3]))
				{
					LanguageWorker_French.tmpStr[i] = '\0';
					LanguageWorker_French.tmpStr[i + 1] = 'L';
					LanguageWorker_French.tmpStr[i + 2] = '\'';
				}
				else if (i + 4 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == ' ' && LanguageWorker_French.tmpStr[i + 1] == 'd' && LanguageWorker_French.tmpStr[i + 2] == 'e' && LanguageWorker_French.tmpStr[i + 3] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 4]))
				{
					LanguageWorker_French.tmpStr[i + 1] = '\0';
					LanguageWorker_French.tmpStr[i + 2] = 'd';
					LanguageWorker_French.tmpStr[i + 3] = '\'';
				}
				else if (i + 4 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == ' ' && LanguageWorker_French.tmpStr[i + 1] == 'l' && LanguageWorker_French.tmpStr[i + 2] == 'e' && LanguageWorker_French.tmpStr[i + 3] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 4]))
				{
					LanguageWorker_French.tmpStr[i + 1] = '\0';
					LanguageWorker_French.tmpStr[i + 2] = 'l';
					LanguageWorker_French.tmpStr[i + 3] = '\'';
				}
				else if (i + 4 < LanguageWorker_French.tmpStr.Length && LanguageWorker_French.tmpStr[i] == ' ' && LanguageWorker_French.tmpStr[i + 1] == 'l' && LanguageWorker_French.tmpStr[i + 2] == 'a' && LanguageWorker_French.tmpStr[i + 3] == ' ' && this.IsVowel(LanguageWorker_French.tmpStr[i + 4]))
				{
					LanguageWorker_French.tmpStr[i + 1] = '\0';
					LanguageWorker_French.tmpStr[i + 2] = 'l';
					LanguageWorker_French.tmpStr[i + 3] = '\'';
				}
			}
			str = LanguageWorker_French.tmpStr.ToString();
			str = str.Replace("\0", "");
			return str;
		}

		
		private static readonly List<string> Exceptions1 = new List<string>
		{
			"bail",
			"corail",
			"émail",
			"gemmail",
			"soupirail",
			"travail",
			"vantail",
			"vitrail"
		};

		
		private static readonly List<string> Exceptions2 = new List<string>
		{
			"bleu",
			"émeu",
			"landau",
			"lieu",
			"pneu",
			"sarrau",
			"bal",
			"banal",
			"fatal",
			"final",
			"festival"
		};

		
		private static readonly List<string> Exceptions3 = new List<string>
		{
			"bijou",
			"caillou",
			"chou",
			"genou",
			"hibou",
			"joujou",
			"pou"
		};

		
		private static StringBuilder tmpStr = new StringBuilder();
	}
}
