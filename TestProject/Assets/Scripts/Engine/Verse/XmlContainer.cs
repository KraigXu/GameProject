using System;
using System.Xml;

namespace Verse
{
	
	public class XmlContainer
	{
		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		
		public XmlNode node;
	}
}
