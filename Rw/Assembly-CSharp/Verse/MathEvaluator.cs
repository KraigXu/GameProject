using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000035 RID: 53
	public static class MathEvaluator
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0001011B File Offset: 0x0000E31B
		private static XPathNavigator Navigator
		{
			get
			{
				if (MathEvaluator.doc == null)
				{
					MathEvaluator.doc = new XPathDocument(new StringReader("<root />"));
				}
				if (MathEvaluator.navigator == null)
				{
					MathEvaluator.navigator = MathEvaluator.doc.CreateNavigator();
				}
				return MathEvaluator.navigator;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00010154 File Offset: 0x0000E354
		public static double Evaluate(string expr)
		{
			if (expr.NullOrEmpty())
			{
				return 0.0;
			}
			expr = MathEvaluator.AddSpacesRegex.Replace(expr, " ${1} ");
			expr = expr.Replace("/", " div ");
			expr = expr.Replace("%", " mod ");
			double result;
			try
			{
				XPathExpression xpathExpression = XPathExpression.Compile("number(" + expr + ")");
				xpathExpression.SetContext(MathEvaluator.Context);
				double num = (double)MathEvaluator.Navigator.Evaluate(xpathExpression);
				if (double.IsNaN(num))
				{
					Log.ErrorOnce("Expression \"" + expr + "\" evaluated to NaN.", expr.GetHashCode() ^ 48337162, false);
					num = 0.0;
				}
				result = num;
			}
			catch (XPathException ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Could not evaluate expression \"",
					expr,
					"\". Error: ",
					ex
				}), expr.GetHashCode() ^ 980986121, false);
				result = 0.0;
			}
			return result;
		}

		// Token: 0x040000B0 RID: 176
		private static XPathDocument doc;

		// Token: 0x040000B1 RID: 177
		private static XPathNavigator navigator;

		// Token: 0x040000B2 RID: 178
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		// Token: 0x040000B3 RID: 179
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}
