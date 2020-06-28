using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000039 RID: 57
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000337 RID: 823 RVA: 0x000107FF File Offset: 0x0000E9FF
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00010802 File Offset: 0x0000EA02
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00010818 File Offset: 0x0000EA18
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		// Token: 0x040000B8 RID: 184
		private string prefix;

		// Token: 0x040000B9 RID: 185
		private string name;
	}
}
