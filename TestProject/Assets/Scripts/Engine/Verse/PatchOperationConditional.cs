using System;
using System.Xml;

namespace Verse
{
	
	public class PatchOperationConditional : PatchOperationPathed
	{
		
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return this.match != null || this.nomatch != null;
		}

		
		private PatchOperation match;

		
		private PatchOperation nomatch;
	}
}
