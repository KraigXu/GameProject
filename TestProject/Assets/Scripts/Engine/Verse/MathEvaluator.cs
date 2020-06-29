using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	
	public static class MathEvaluator
	{
		
		
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

		
		private static XPathDocument doc;

		
		private static XPathNavigator navigator;

		
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}
