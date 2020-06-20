using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000038 RID: 56
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000107B4 File Offset: 0x0000E9B4
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000330 RID: 816 RVA: 0x000107BC File Offset: 0x0000E9BC
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000107C9 File Offset: 0x0000E9C9
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00010306 File Offset: 0x0000E506
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000107D6 File Offset: 0x0000E9D6
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000107EC File Offset: 0x0000E9EC
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		// Token: 0x040000B6 RID: 182
		private XPathResultType[] argTypes;

		// Token: 0x040000B7 RID: 183
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}
