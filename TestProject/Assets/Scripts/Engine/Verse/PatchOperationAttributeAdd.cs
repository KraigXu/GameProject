﻿using System;
using System.Xml;

namespace Verse
{
	
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}

		
		protected string value;
	}
}
