using System;
using System.IO;
using System.Xml;

namespace Verse
{
	
	public class LoadableXmlAsset
	{
		
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x000734C0 File Offset: 0x000716C0
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar.ToString() + this.name;
			}
		}

		
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

		
		public override string ToString()
		{
			return this.name;
		}

		
		private static XmlReader reader;

		
		public string name;

		
		public string fullFolderPath;

		
		public XmlDocument xmlDoc;

		
		public ModContentPack mod;
	}
}
