using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020011B4 RID: 4532
	public struct SlateRef<T> : ISlateRef, IEquatable<SlateRef<T>>
	{
		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x060068B9 RID: 26809 RVA: 0x00249479 File Offset: 0x00247679
		// (set) Token: 0x060068BA RID: 26810 RVA: 0x00249481 File Offset: 0x00247681
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

		// Token: 0x060068BB RID: 26811 RVA: 0x00249481 File Offset: 0x00247681
		public SlateRef(string slateRef)
		{
			this.slateRef = slateRef;
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x0024948C File Offset: 0x0024768C
		public T GetValue(Slate slate)
		{
			T result;
			this.TryGetValue(slate, out result);
			return result;
		}

		// Token: 0x060068BD RID: 26813 RVA: 0x002494A4 File Offset: 0x002476A4
		public bool TryGetValue(Slate slate, out T value)
		{
			return this.TryGetConvertedValue<T>(slate, out value);
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x002494B0 File Offset: 0x002476B0
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

		// Token: 0x060068BF RID: 26815 RVA: 0x002495CC File Offset: 0x002477CC
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

		// Token: 0x060068C0 RID: 26816 RVA: 0x00249610 File Offset: 0x00247810
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

		// Token: 0x060068C1 RID: 26817 RVA: 0x0024968C File Offset: 0x0024788C
		private static string RegexMatchEvaluatorResolveMathExpression(Match match)
		{
			string text = match.Groups[1].Value;
			text = SlateRef<T>.VarsRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmptyCached);
			return MathEvaluator.Evaluate(text).ToString();
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x002496CA File Offset: 0x002478CA
		public override bool Equals(object obj)
		{
			return obj is SlateRef<T> && this.Equals((SlateRef<T>)obj);
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x002496E2 File Offset: 0x002478E2
		public bool Equals(SlateRef<T> other)
		{
			return this == other;
		}

		// Token: 0x060068C4 RID: 26820 RVA: 0x002496F0 File Offset: 0x002478F0
		public static bool operator ==(SlateRef<T> a, SlateRef<T> b)
		{
			return a.slateRef == b.slateRef;
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x00249703 File Offset: 0x00247903
		public static bool operator !=(SlateRef<T> a, SlateRef<T> b)
		{
			return !(a == b);
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x0024970F File Offset: 0x0024790F
		public static implicit operator SlateRef<T>(T t)
		{
			return new SlateRef<T>((t != null) ? t.ToString() : null);
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x0024972E File Offset: 0x0024792E
		public override int GetHashCode()
		{
			if (this.slateRef == null)
			{
				return 0;
			}
			return this.slateRef.GetHashCode();
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x00249748 File Offset: 0x00247948
		public string ToString(Slate slate)
		{
			string result;
			this.TryGetConvertedValue<string>(slate, out result);
			return result;
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x00249760 File Offset: 0x00247960
		public override string ToString()
		{
			if (!QuestGen.Working)
			{
				return this.slateRef;
			}
			return this.ToString(QuestGen.slate);
		}

		// Token: 0x04004137 RID: 16695
		public const string SlateRefFieldName = "slateRef";

		// Token: 0x04004138 RID: 16696
		[MustTranslate_SlateRef]
		private string slateRef;

		// Token: 0x04004139 RID: 16697
		private static Slate tmpCurSlate;

		// Token: 0x0400413A RID: 16698
		private static readonly Regex VarsRegex = new Regex("\\$([a-zA-Z0-1_/]*)");

		// Token: 0x0400413B RID: 16699
		private static readonly Regex HighPriorityVarsRegex = new Regex("\\(\\(\\$([a-zA-Z0-1_/]*)\\)\\)");

		// Token: 0x0400413C RID: 16700
		private static readonly Regex MathExprRegex = new Regex("\\$\\((.*)\\)");

		// Token: 0x0400413D RID: 16701
		private static MatchEvaluator RegexMatchEvaluatorConcatenateCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate);

		// Token: 0x0400413E RID: 16702
		private static MatchEvaluator RegexMatchEvaluatorConcatenateZeroIfEmptyCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmpty);

		// Token: 0x0400413F RID: 16703
		private static MatchEvaluator RegexMatchEvaluatorEvaluateMathExpressionCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorResolveMathExpression);
	}
}
