using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06000337 RID: 823 RVA: 0x000107FF File Offset: 0x0000E9FF
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
