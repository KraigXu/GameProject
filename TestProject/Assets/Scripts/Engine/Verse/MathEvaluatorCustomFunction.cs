using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000107B4 File Offset: 0x0000E9B4
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		
		// (get) Token: 0x06000330 RID: 816 RVA: 0x000107BC File Offset: 0x0000E9BC
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000107C9 File Offset: 0x0000E9C9
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00010306 File Offset: 0x0000E506
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
