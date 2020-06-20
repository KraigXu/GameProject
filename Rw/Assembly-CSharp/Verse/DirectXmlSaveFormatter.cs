using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x020002B7 RID: 695
	public static class DirectXmlSaveFormatter
	{
		// Token: 0x060013CB RID: 5067 RVA: 0x00071AD4 File Offset: 0x0006FCD4
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (!root.Elements().Any<XElement>())
			{
				return;
			}
			foreach (XNode xnode in root.Elements().ToList<XElement>())
			{
				XText content = new XText("\n");
				xnode.AddAfterSelf(content);
			}
			root.Elements().First<XElement>().AddBeforeSelf(new XText("\n"));
			root.Elements().Last<XElement>().AddAfterSelf(new XText("\n"));
			foreach (XElement element in root.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element, 1);
			}
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00071BBC File Offset: 0x0006FDBC
		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			foreach (XElement element2 in element.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element2, depth + 1);
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00071C44 File Offset: 0x0006FE44
		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < depth; i++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}
