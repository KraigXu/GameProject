﻿using System;
using System.IO;
using System.Xml;

namespace Verse
{
	
	public class ScribeLoader
	{
		
		public void InitLoading(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoading() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			if (this.curParent != null)
			{
				Log.Error("Current parent is not null in InitLoading", false);
				this.curParent = null;
			}
			if (this.curPathRelToParent != null)
			{
				Log.Error("Current path relative to parent is not null in InitLoading", false);
				this.curPathRelToParent = null;
			}
			try
			{
				StreamReader streamReader = new StreamReader(filePath);
				{
					XmlTextReader xmlTextReader = new XmlTextReader(streamReader);
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						this.curXmlParent = xmlDocument.DocumentElement;
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading file: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		
		public void InitLoadingMetaHeaderOnly(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoadingMetaHeaderOnly() but current mode is " + Scribe.mode, false);
				Scribe.ForceStop();
			}
			try
			{
				StreamReader streamReader = new StreamReader(filePath);
				{
					XmlTextReader xmlTextReader = new XmlTextReader(streamReader);
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							XmlReader xmlReader = xmlTextReader.ReadSubtree();
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.Load(xmlReader);
								XmlElement xmlElement = xmlDocument.CreateElement("root");
								xmlElement.AppendChild(xmlDocument.DocumentElement);
								this.curXmlParent = xmlElement;
								goto IL_82;
							}
							goto IL_80;
							IL_82:
							goto IL_8E;
						}
						IL_80:
						return;
					}
					IL_8E:;
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading meta header: ",
					filePath,
					"\n",
					ex
				}), false);
				this.ForceStop();
				throw;
			}
		}

		
		public void FinalizeLoading()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called FinalizeLoading() but current mode is " + Scribe.mode, false);
				return;
			}
			try
			{
				Scribe.ExitNode();
				this.curXmlParent = null;
				this.curParent = null;
				this.curPathRelToParent = null;
				Scribe.mode = LoadSaveMode.Inactive;
				this.crossRefs.ResolveAllCrossReferences();
				this.initer.DoAllPostLoadInits();
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg, false);
				this.ForceStop();
				throw;
			}
		}

		
		public bool EnterNode(string nodeName)
		{
			if (this.curXmlParent != null)
			{
				XmlNode xmlNode = this.curXmlParent[nodeName];
				if (xmlNode == null && char.IsDigit(nodeName[0]))
				{
					xmlNode = this.curXmlParent.ChildNodes[int.Parse(nodeName)];
				}
				if (xmlNode == null)
				{
					return false;
				}
				this.curXmlParent = xmlNode;
			}
			this.curPathRelToParent = this.curPathRelToParent + "/" + nodeName;
			return true;
		}

		
		public void ExitNode()
		{
			if (this.curXmlParent != null)
			{
				this.curXmlParent = this.curXmlParent.ParentNode;
			}
			if (this.curPathRelToParent != null)
			{
				int num = this.curPathRelToParent.LastIndexOf('/');
				this.curPathRelToParent = ((num > 0) ? this.curPathRelToParent.Substring(0, num) : null);
			}
		}

		
		public void ForceStop()
		{
			this.curXmlParent = null;
			this.curParent = null;
			this.curPathRelToParent = null;
			this.crossRefs.Clear(false);
			this.initer.Clear();
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		
		public CrossRefHandler crossRefs = new CrossRefHandler();

		
		public PostLoadIniter initer = new PostLoadIniter();

		
		public IExposable curParent;

		
		public XmlNode curXmlParent;

		
		public string curPathRelToParent;
	}
}
