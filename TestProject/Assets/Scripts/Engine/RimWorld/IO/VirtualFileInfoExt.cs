using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace RimWorld.IO
{
	// Token: 0x020012A8 RID: 4776
	public static class VirtualFileInfoExt
	{
		// Token: 0x060070DD RID: 28893 RVA: 0x0027505C File Offset: 0x0027325C
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
