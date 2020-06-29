using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	
	public class MathEvaluatorCustomContext : XsltContext
	{
		
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00010290 File Offset: 0x0000E490
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		
		public MathEvaluatorCustomContext()
		{
		}

		
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		
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

		
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		
		private XsltArgumentList argList;
	}
}
