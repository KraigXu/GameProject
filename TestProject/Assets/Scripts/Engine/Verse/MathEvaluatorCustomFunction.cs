using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		
		
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		
		
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		
		
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		
		
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		
		private XPathResultType[] argTypes;

		
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}
