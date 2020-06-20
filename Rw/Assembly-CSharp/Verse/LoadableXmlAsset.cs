using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020002BD RID: 701
	public class LoadableXmlAsset
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x000734C0 File Offset: 0x000716C0
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar.ToString() + this.name;
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000734EC File Offset: 0x000716EC
		public LoadableXmlAsset(string name, string fullFolderPath, string contents)
		{
			this.name = name;
			this.fullFolderPath = fullFolderPath;
			try
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.IgnoreComments = true;
				xmlReaderSettings.IgnoreWhitespace = true;
				xmlReaderSettings.CheckCharacters = false;
				using (StringReader stringReader = new StringReader(contents))
				{
					using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
					{
						this.xmlDoc = new XmlDocument();
						this.xmlDoc.Load(xmlReader);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception reading ",
					name,
					" as XML: ",
					ex
				}), false);
				this.xmlDoc = null;
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x000735C4 File Offset: 0x000717C4
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000D63 RID: 3427
		private static XmlReader reader;

		// Token: 0x04000D64 RID: 3428
		public string name;

		// Token: 0x04000D65 RID: 3429
		public string fullFolderPath;

		// Token: 0x04000D66 RID: 3430
		public XmlDocument xmlDoc;

		// Token: 0x04000D67 RID: 3431
		public ModContentPack mod;
	}
}
