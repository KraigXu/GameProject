using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		
		
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		
		
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		
		
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		
		private string prefix;

		
		private string name;
	}
}
