using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace RimWorld.IO
{
	
	public static class VirtualFileInfoExt
	{
		
		public static XDocument LoadAsXDocument(this VirtualFile file)
		{
			XDocument result;
			using (Stream stream = file.CreateReadStream())
			{
				result = XDocument.Load(XmlReader.Create(stream), LoadOptions.SetLineInfo);
			}
			return result;
		}
	}
}
