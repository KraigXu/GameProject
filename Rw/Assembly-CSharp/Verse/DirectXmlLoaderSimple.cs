using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x020002B6 RID: 694
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x060013CA RID: 5066 RVA: 0x00071AC1 File Offset: 0x0006FCC1
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(VirtualFile file)
		{
			XDocument xdocument = file.LoadAsXDocument();
			foreach (XElement xelement in xdocument.Root.Elements())
			{
				string key = xelement.Name.ToString();
				string text = xelement.Value;
				text = text.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = text,
					lineNumber = ((IXmlLineInfo)xelement).LineNumber
				};
			}
			IEnumerator<XElement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x02001485 RID: 5253
		public struct XmlKeyValuePair
		{
			// Token: 0x04004DD2 RID: 19922
			public string key;

			// Token: 0x04004DD3 RID: 19923
			public string value;

			// Token: 0x04004DD4 RID: 19924
			public int lineNumber;
		}
	}
}
