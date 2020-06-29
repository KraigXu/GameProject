using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public struct SlateRef<T> : ISlateRef, IEquatable<SlateRef<T>>
	{
		
		
		
		string ISlateRef.SlateRef
		{
			get
			{
				return this.slateRef;
			}
			set
			{
				this.slateRef = value;
			}
		}

		
		public SlateRef(string slateRef)
		{
			this.slateRef = slateRef;
		}

		
		public T GetValue(Slate slate)
		{
			T result;
			this.TryGetValue(slate, out result);
			return result;
		}

		
		public bool TryGetValue(Slate slate, out T value)
		{
			return this.TryGetConvertedValue<T>(slate, out value);
		}

		
		public bool TryGetConvertedValue<TAnything>(Slate slate, out TAnything value)
		{
			if (this.slateRef == null)
			{
				value = default(TAnything);
				return true;
			}
			SlateRef<T>.tmpCurSlate = slate;
			string text = SlateRef<T>.HighPriorityVarsRegex.Replace(this.slateRef, new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate));
			object obj;
			bool flag;
			if (!SlateRefUtility.CheckSingleVariableSyntax(text, slate, out obj, out flag))
			{
				obj = SlateRef<T>.MathExprRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorEvaluateMathExpressionCached);
				obj = SlateRef<T>.VarsRegex.Replace((string)obj, SlateRef<T>.RegexMatchEvaluatorConcatenateCached);
				flag = true;
			}
			SlateRef<T>.tmpCurSlate = null;
			if (!flag)
			{
				value = default(TAnything);
				return false;
			}
			if (obj == null)
			{
				value = default(TAnything);
				return true;
			}
			if (obj is TAnything)
			{
				value = (TAnything)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<TAnything>(obj))
			{
				value = ConvertHelper.Convert<TAnything>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert SlateRef \"",
				this.slateRef,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(TAnything).Name
			}), false);
			value = default(TAnything);
			return false;
		}

		
		private static string RegexMatchEvaluatorConcatenate(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				return "";
			}
			if (obj == null)
			{
				return "";
			}
			return obj.ToString();
		}

		
		private static string RegexMatchEvaluatorConcatenateZeroIfEmpty(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				Log.ErrorOnce("Tried to use variable \"" + value + "\" in a math expression but it doesn't exist.", value.GetHashCode() ^ 194857119, false);
				return "0";
			}
			if (obj == null)
			{
				return "0";
			}
			string text = obj.ToString();
			if (text == "")
			{
				return "0";
			}
			return text;
		}

		
		private static string RegexMatchEvaluatorResolveMathExpression(Match match)
		{
			string text = match.Groups[1].Value;
			text = SlateRef<T>.VarsRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmptyCached);
			return MathEvaluator.Evaluate(text).ToString();
		}

		
		public override bool Equals(object obj)
		{
			return obj is SlateRef<T> && this.Equals((SlateRef<T>)obj);
		}

		
		public bool Equals(SlateRef<T> other)
		{
			return this == other;
		}

		
		public static bool operator ==(SlateRef<T> a, SlateRef<T> b)
		{
			return a.slateRef == b.slateRef;
		}

		
		public static bool operator !=(SlateRef<T> a, SlateRef<T> b)
		{
			return !(a == b);
		}

		
		public static implicit operator SlateRef<T>(T t)
		{
			return new SlateRef<T>((t != null) ? t.ToString() : null);
		}

		
		public override int GetHashCode()
		{
			if (this.slateRef == null)
			{
				return 0;
			}
			return this.slateRef.GetHashCode();
		}

		
		public string ToString(Slate slate)
		{
			string result;
			this.TryGetConvertedValue<string>(slate, out result);
			return result;
		}

		
		public override string ToString()
		{
			if (!QuestGen.Working)
			{
				return this.slateRef;
			}
			return this.ToString(QuestGen.slate);
		}

		
		public const string SlateRefFieldName = "slateRef";

		
		[MustTranslate_SlateRef]
		private string slateRef;

		
		private static Slate tmpCurSlate;

		
		private static readonly Regex VarsRegex = new Regex("\\$([a-zA-Z0-1_/]*)");

		
		private static readonly Regex HighPriorityVarsRegex = new Regex("\\(\\(\\$([a-zA-Z0-1_/]*)\\)\\)");

		
		private static readonly Regex MathExprRegex = new Regex("\\$\\((.*)\\)");

		
		private static MatchEvaluator RegexMatchEvaluatorConcatenateCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate);

		
		private static MatchEvaluator RegexMatchEvaluatorConcatenateZeroIfEmptyCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmpty);

		
		private static MatchEvaluator RegexMatchEvaluatorEvaluateMathExpressionCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorResolveMathExpression);
	}
}
