using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200012F RID: 303
	public static class LanguageDataWriter
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x0002C2FC File Offset: 0x0002A4FC
		public static void WriteBackstoryFile()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.DevOutputFolderPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			if (new FileInfo(GenFilePaths.BackstoryOutputFilePath).Exists)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("Cannot write: File already exists at " + GenFilePaths.BackstoryOutputFilePath, null, null, null, null, null, false, null, null));
				return;
			}
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.IndentChars = "\t";
			using (XmlWriter xmlWriter = XmlWriter.Create(GenFilePaths.BackstoryOutputFilePath, xmlWriterSettings))
			{
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("BackstoryTranslations");
				foreach (KeyValuePair<string, Backstory> keyValuePair in BackstoryDatabase.allBackstories)
				{
					Backstory value = keyValuePair.Value;
					xmlWriter.WriteStartElement(value.identifier);
					xmlWriter.WriteElementString("title", value.title);
					if (!value.titleFemale.NullOrEmpty())
					{
						xmlWriter.WriteElementString("titleFemale", value.titleFemale);
					}
					xmlWriter.WriteElementString("titleShort", value.titleShort);
					if (!value.titleShortFemale.NullOrEmpty())
					{
						xmlWriter.WriteElementString("titleShortFemale", value.titleShortFemale);
					}
					xmlWriter.WriteElementString("desc", value.baseDesc);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
			}
			Messages.Message("Fresh backstory translation file saved to " + GenFilePaths.BackstoryOutputFilePath, MessageTypeDefOf.NeutralEvent, false);
		}
	}
}
