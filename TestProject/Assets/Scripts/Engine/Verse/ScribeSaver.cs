using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020002D1 RID: 721
	public class ScribeSaver
	{
		// Token: 0x0600144A RID: 5194 RVA: 0x00076AA0 File Offset: 0x00074CA0
		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			if (this.curPath != null)
			{
				Log.Error("Current path is not null in InitSaving", false);
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
			}
			try
			{
				Scribe.mode = LoadSaveMode.Saving;
				this.saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				this.writer = XmlWriter.Create(this.saveStream, xmlWriterSettings);
				this.writer.WriteStartDocument();
				this.EnterNode(documentElementName);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init saving file: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00076B98 File Offset: 0x00074D98
		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode, false);
				return;
			}
			if (this.anyInternalException)
			{
				this.ForceStop();
				throw new Exception("Can't finalize saving due to internal exception. The whole file would be most likely corrupted anyway.");
			}
			try
			{
				if (this.writer != null)
				{
					this.ExitNode();
					this.writer.WriteEndDocument();
					this.writer.Flush();
					this.writer.Close();
					this.writer = null;
				}
				if (this.saveStream != null)
				{
					this.saveStream.Flush();
					this.saveStream.Close();
					this.saveStream = null;
				}
				Scribe.mode = LoadSaveMode.Inactive;
				this.savingForDebug = false;
				this.loadIDsErrorsChecker.CheckForErrorsAndClear();
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
				this.anyInternalException = false;
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg, false);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00076CA4 File Offset: 0x00074EA4
		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.", false);
				return;
			}
			try
			{
				this.writer.WriteElementString(elementName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00076CF0 File Offset: 0x00074EF0
		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.", false);
				return;
			}
			try
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00076D3C File Offset: 0x00074F3C
		public string DebugOutputFor(IExposable saveable)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("DebugOutput needs current mode to be Inactive", false);
				return "";
			}
			string result;
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
					xmlWriterSettings.Indent = true;
					xmlWriterSettings.IndentChars = "  ";
					xmlWriterSettings.OmitXmlDeclaration = true;
					try
					{
						using (this.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
						{
							Scribe.mode = LoadSaveMode.Saving;
							this.savingForDebug = true;
							Scribe_Deep.Look<IExposable>(ref saveable, "saveable", Array.Empty<object>());
						}
						result = stringWriter.ToString();
					}
					finally
					{
						this.ForceStop();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception while getting debug output: " + arg, false);
				this.ForceStop();
				result = "";
			}
			return result;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00076E38 File Offset: 0x00075038
		public bool EnterNode(string nodeName)
		{
			if (this.writer == null)
			{
				return false;
			}
			try
			{
				this.writer.WriteStartElement(nodeName);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
			return true;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00076E7C File Offset: 0x0007507C
		public void ExitNode()
		{
			if (this.writer == null)
			{
				return;
			}
			try
			{
				this.writer.WriteEndElement();
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x00076EBC File Offset: 0x000750BC
		public void ForceStop()
		{
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer = null;
			}
			if (this.saveStream != null)
			{
				this.saveStream.Close();
				this.saveStream = null;
			}
			this.savingForDebug = false;
			this.loadIDsErrorsChecker.Clear();
			this.curPath = null;
			this.savedNodes.Clear();
			this.nextListElementTemporaryId = 0;
			this.anyInternalException = false;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04000DA4 RID: 3492
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x04000DA5 RID: 3493
		public bool savingForDebug;

		// Token: 0x04000DA6 RID: 3494
		private Stream saveStream;

		// Token: 0x04000DA7 RID: 3495
		private XmlWriter writer;

		// Token: 0x04000DA8 RID: 3496
		private string curPath;

		// Token: 0x04000DA9 RID: 3497
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x04000DAA RID: 3498
		private int nextListElementTemporaryId;

		// Token: 0x04000DAB RID: 3499
		private bool anyInternalException;
	}
}
