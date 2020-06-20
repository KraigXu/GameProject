using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000036 RID: 54
	public class MathEvaluatorCustomContext : XsltContext
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00010290 File Offset: 0x0000E490
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00010298 File Offset: 0x0000E498
		public MathEvaluatorCustomContext()
		{
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000102A0 File Offset: 0x0000E4A0
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000102B0 File Offset: 0x0000E4B0
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
		{
			MathEvaluatorCustomFunctions.FunctionType[] functionTypes = MathEvaluatorCustomFunctions.FunctionTypes;
			for (int i = 0; i < functionTypes.Length; i++)
			{
				if (functionTypes[i].name == name)
				{
					return new MathEvaluatorCustomFunction(functionTypes[i], argTypes);
				}
			}
			return null;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000102EC File Offset: 0x0000E4EC
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00010306 File Offset: 0x0000E506
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		// Token: 0x040000B4 RID: 180
		private XsltArgumentList argList;
	}
}
