using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	
	public static class DirectXmlLoaderSimple
	{
		
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

		
		public struct XmlKeyValuePair
		{
			
			public string key;

			
			public string value;

			
			public int lineNumber;
		}
	}
}
