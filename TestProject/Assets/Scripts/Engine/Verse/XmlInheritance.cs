using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x020002BF RID: 703
	public static class XmlInheritance
	{
		// Token: 0x060013EA RID: 5098 RVA: 0x000735CC File Offset: 0x000717CC
		static XmlInheritance()
		{
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (FieldInfo fieldInfo in type.GetFields())
				{
					if (fieldInfo.IsDefined(typeof(XmlInheritanceAllowDuplicateNodes), false))
					{
						XmlInheritance.allowDuplicateNodesFieldNames.Add(fieldInfo.Name);
					}
				}
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00073680 File Offset: 0x00071880
		public static void TryRegisterAllFrom(LoadableXmlAsset xmlAsset, ModContentPack mod)
		{
			if (xmlAsset.xmlDoc == null)
			{
				return;
			}
			DeepProfiler.Start("XmlInheritance.TryRegisterAllFrom");
			foreach (object obj in xmlAsset.xmlDoc.DocumentElement.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlInheritance.TryRegister(xmlNode, mod);
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00073704 File Offset: 0x00071904
		public static void TryRegister(XmlNode node, ModContentPack mod)
		{
			XmlAttribute xmlAttribute = node.Attributes["Name"];
			XmlAttribute xmlAttribute2 = node.Attributes["ParentName"];
			if (xmlAttribute == null && xmlAttribute2 == null)
			{
				return;
			}
			List<XmlInheritance.XmlInheritanceNode> list = null;
			if (xmlAttribute != null && XmlInheritance.nodesByName.TryGetValue(xmlAttribute.Value, out list))
			{
				int i = 0;
				while (i < list.Count)
				{
					if (list[i].mod == mod)
					{
						if (mod == null)
						{
							Log.Error("XML error: Could not register node named \"" + xmlAttribute.Value + "\" because this name is already used.", false);
							return;
						}
						Log.Error(string.Concat(new string[]
						{
							"XML error: Could not register node named \"",
							xmlAttribute.Value,
							"\" in mod ",
							mod.ToString(),
							" because this name is already used in this mod."
						}), false);
						return;
					}
					else
					{
						i++;
					}
				}
			}
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = new XmlInheritance.XmlInheritanceNode();
			xmlInheritanceNode.xmlNode = node;
			xmlInheritanceNode.mod = mod;
			XmlInheritance.unresolvedNodes.Add(xmlInheritanceNode);
			if (xmlAttribute != null)
			{
				if (list != null)
				{
					list.Add(xmlInheritanceNode);
					return;
				}
				list = new List<XmlInheritance.XmlInheritanceNode>();
				list.Add(xmlInheritanceNode);
				XmlInheritance.nodesByName.Add(xmlAttribute.Value, list);
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00073824 File Offset: 0x00071A24
		public static void Resolve()
		{
			XmlInheritance.ResolveParentsAndChildNodesLinks();
			XmlInheritance.ResolveXmlNodes();
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00073830 File Offset: 0x00071A30
		public static XmlNode GetResolvedNodeFor(XmlNode originalNode)
		{
			if (originalNode.Attributes["ParentName"] != null)
			{
				XmlInheritance.XmlInheritanceNode xmlInheritanceNode;
				if (XmlInheritance.resolvedNodes.TryGetValue(originalNode, out xmlInheritanceNode))
				{
					return xmlInheritanceNode.resolvedXmlNode;
				}
				if (XmlInheritance.unresolvedNodes.Any((XmlInheritance.XmlInheritanceNode x) => x.xmlNode == originalNode))
				{
					Log.Error("XML error: XML node \"" + originalNode.Name + "\" has not been resolved yet. There's probably a Resolve() call missing somewhere.", false);
				}
				else
				{
					Log.Error("XML error: Tried to get resolved node for node \"" + originalNode.Name + "\" which uses a ParentName attribute, but it is not in a resolved nodes collection, which means that it was never registered or there was an error while resolving it.", false);
				}
			}
			return originalNode;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x000738DC File Offset: 0x00071ADC
		public static void Clear()
		{
			XmlInheritance.resolvedNodes.Clear();
			XmlInheritance.unresolvedNodes.Clear();
			XmlInheritance.nodesByName.Clear();
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000738FC File Offset: 0x00071AFC
		private static void ResolveParentsAndChildNodesLinks()
		{
			for (int i = 0; i < XmlInheritance.unresolvedNodes.Count; i++)
			{
				XmlAttribute xmlAttribute = XmlInheritance.unresolvedNodes[i].xmlNode.Attributes["ParentName"];
				if (xmlAttribute != null)
				{
					XmlInheritance.unresolvedNodes[i].parent = XmlInheritance.GetBestParentFor(XmlInheritance.unresolvedNodes[i], xmlAttribute.Value);
					if (XmlInheritance.unresolvedNodes[i].parent != null)
					{
						XmlInheritance.unresolvedNodes[i].parent.children.Add(XmlInheritance.unresolvedNodes[i]);
					}
				}
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000739A4 File Offset: 0x00071BA4
		private static void ResolveXmlNodes()
		{
			List<XmlInheritance.XmlInheritanceNode> list = (from x in XmlInheritance.unresolvedNodes
			where x.parent == null || x.parent.resolvedXmlNode != null
			select x).ToList<XmlInheritance.XmlInheritanceNode>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(list[i]);
			}
			for (int j = 0; j < XmlInheritance.unresolvedNodes.Count; j++)
			{
				if (XmlInheritance.unresolvedNodes[j].resolvedXmlNode == null)
				{
					Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + XmlInheritance.unresolvedNodes[j].xmlNode.Name + "\". Full node: " + XmlInheritance.unresolvedNodes[j].xmlNode.OuterXml, false);
				}
				else
				{
					XmlInheritance.resolvedNodes.Add(XmlInheritance.unresolvedNodes[j].xmlNode, XmlInheritance.unresolvedNodes[j]);
				}
			}
			XmlInheritance.unresolvedNodes.Clear();
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00073A98 File Offset: 0x00071C98
		private static void ResolveXmlNodesRecursively(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.resolvedXmlNode != null)
			{
				Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + node.xmlNode.Name + "\". Full node: " + node.xmlNode.OuterXml, false);
				return;
			}
			XmlInheritance.ResolveXmlNodeFor(node);
			for (int i = 0; i < node.children.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(node.children[i]);
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00073B08 File Offset: 0x00071D08
		private static XmlInheritance.XmlInheritanceNode GetBestParentFor(XmlInheritance.XmlInheritanceNode node, string parentName)
		{
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = null;
			List<XmlInheritance.XmlInheritanceNode> list;
			if (XmlInheritance.nodesByName.TryGetValue(parentName, out list))
			{
				if (node.mod == null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].mod == null)
						{
							xmlInheritanceNode = list[i];
							break;
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (xmlInheritanceNode == null || list[j].mod.loadOrder < xmlInheritanceNode.mod.loadOrder)
							{
								xmlInheritanceNode = list[j];
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].mod != null && list[k].mod.loadOrder <= node.mod.loadOrder && (xmlInheritanceNode == null || list[k].mod.loadOrder > xmlInheritanceNode.mod.loadOrder))
						{
							xmlInheritanceNode = list[k];
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (list[l].mod == null)
							{
								xmlInheritanceNode = list[l];
								break;
							}
						}
					}
				}
			}
			if (xmlInheritanceNode == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"XML error: Could not find parent node named \"",
					parentName,
					"\" for node \"",
					node.xmlNode.Name,
					"\". Full node: ",
					node.xmlNode.OuterXml
				}), false);
				return null;
			}
			return xmlInheritanceNode;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00073C90 File Offset: 0x00071E90
		private static void ResolveXmlNodeFor(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.parent == null)
			{
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			if (node.parent.resolvedXmlNode == null)
			{
				Log.Error("XML error: Internal error. Tried to resolve node whose parent has not been resolved yet. This means that this method was called in incorrect order.", false);
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			XmlInheritance.CheckForDuplicateNodes(node.xmlNode, node.xmlNode);
			XmlNode xmlNode = node.parent.resolvedXmlNode.CloneNode(true);
			XmlInheritance.RecursiveNodeCopyOverwriteElements(node.xmlNode, xmlNode);
			node.resolvedXmlNode = xmlNode;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00073D10 File Offset: 0x00071F10
		private static void RecursiveNodeCopyOverwriteElements(XmlNode child, XmlNode current)
		{
			DeepProfiler.Start("RecursiveNodeCopyOverwriteElements");
			try
			{
				XmlAttribute xmlAttribute = child.Attributes["Inherit"];
				if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "false")
				{
					while (current.HasChildNodes)
					{
						current.RemoveChild(current.FirstChild);
					}
					using (IEnumerator enumerator = child.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlNode node = (XmlNode)obj;
							XmlNode newChild = current.OwnerDocument.ImportNode(node, true);
							current.AppendChild(newChild);
						}
						return;
					}
				}
				current.Attributes.RemoveAll();
				XmlAttributeCollection attributes = child.Attributes;
				for (int i = 0; i < attributes.Count; i++)
				{
					XmlAttribute node2 = (XmlAttribute)current.OwnerDocument.ImportNode(attributes[i], true);
					current.Attributes.Append(node2);
				}
				List<XmlElement> list = new List<XmlElement>();
				XmlNode xmlNode = null;
				foreach (object obj2 in child)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (xmlNode2.NodeType == XmlNodeType.Text)
					{
						xmlNode = xmlNode2;
					}
					else if (xmlNode2.NodeType == XmlNodeType.Element)
					{
						list.Add((XmlElement)xmlNode2);
					}
				}
				if (xmlNode != null)
				{
					DeepProfiler.Start("RecursiveNodeCopyOverwriteElements - Remove all current nodes");
					for (int j = current.ChildNodes.Count - 1; j >= 0; j--)
					{
						XmlNode xmlNode3 = current.ChildNodes[j];
						if (xmlNode3.NodeType != XmlNodeType.Attribute)
						{
							current.RemoveChild(xmlNode3);
						}
					}
					DeepProfiler.End();
					XmlNode newChild2 = current.OwnerDocument.ImportNode(xmlNode, true);
					current.AppendChild(newChild2);
				}
				else
				{
					if (!list.Any<XmlElement>())
					{
						bool flag = false;
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (((XmlNode)enumerator.Current).NodeType == XmlNodeType.Element)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							goto IL_2F0;
						}
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj3 = enumerator.Current;
								XmlNode xmlNode4 = (XmlNode)obj3;
								if (xmlNode4.NodeType != XmlNodeType.Attribute)
								{
									current.RemoveChild(xmlNode4);
								}
							}
							return;
						}
					}
					for (int k = 0; k < list.Count; k++)
					{
						XmlElement xmlElement = list[k];
						if (XmlInheritance.IsListElement(xmlElement))
						{
							XmlNode newChild3 = current.OwnerDocument.ImportNode(xmlElement, true);
							current.AppendChild(newChild3);
						}
						else
						{
							XmlElement xmlElement2 = current[xmlElement.Name];
							if (xmlElement2 != null)
							{
								XmlInheritance.RecursiveNodeCopyOverwriteElements(xmlElement, xmlElement2);
							}
							else
							{
								XmlNode newChild4 = current.OwnerDocument.ImportNode(xmlElement, true);
								current.AppendChild(newChild4);
							}
						}
					}
					IL_2F0:;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00074094 File Offset: 0x00072294
		private static void CheckForDuplicateNodes(XmlNode node, XmlNode root)
		{
			XmlInheritance.tempUsedNodeNames.Clear();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element && !XmlInheritance.IsListElement(xmlNode))
				{
					if (XmlInheritance.tempUsedNodeNames.Contains(xmlNode.Name))
					{
						Log.Error(string.Concat(new string[]
						{
							"XML error: Duplicate XML node name ",
							xmlNode.Name,
							" in this XML block: ",
							node.OuterXml,
							(node != root) ? ("\n\nRoot node: " + root.OuterXml) : ""
						}), false);
					}
					else
					{
						XmlInheritance.tempUsedNodeNames.Add(xmlNode.Name);
					}
				}
			}
			XmlInheritance.tempUsedNodeNames.Clear();
			foreach (object obj2 in node.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj2;
				if (xmlNode2.NodeType == XmlNodeType.Element)
				{
					XmlInheritance.CheckForDuplicateNodes(xmlNode2, root);
				}
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x000741D8 File Offset: 0x000723D8
		private static bool IsListElement(XmlNode node)
		{
			return node.Name == "li" || (node.ParentNode != null && XmlInheritance.allowDuplicateNodesFieldNames.Contains(node.ParentNode.Name));
		}

		// Token: 0x04000D68 RID: 3432
		private static Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode> resolvedNodes = new Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode>();

		// Token: 0x04000D69 RID: 3433
		private static List<XmlInheritance.XmlInheritanceNode> unresolvedNodes = new List<XmlInheritance.XmlInheritanceNode>();

		// Token: 0x04000D6A RID: 3434
		private static Dictionary<string, List<XmlInheritance.XmlInheritanceNode>> nodesByName = new Dictionary<string, List<XmlInheritance.XmlInheritanceNode>>();

		// Token: 0x04000D6B RID: 3435
		public static HashSet<string> allowDuplicateNodesFieldNames = new HashSet<string>();

		// Token: 0x04000D6C RID: 3436
		private const string NameAttributeName = "Name";

		// Token: 0x04000D6D RID: 3437
		private const string ParentNameAttributeName = "ParentName";

		// Token: 0x04000D6E RID: 3438
		private const string InheritAttributeName = "Inherit";

		// Token: 0x04000D6F RID: 3439
		private static HashSet<string> tempUsedNodeNames = new HashSet<string>();

		// Token: 0x02001489 RID: 5257
		private class XmlInheritanceNode
		{
			// Token: 0x04004DDF RID: 19935
			public XmlNode xmlNode;

			// Token: 0x04004DE0 RID: 19936
			public XmlNode resolvedXmlNode;

			// Token: 0x04004DE1 RID: 19937
			public ModContentPack mod;

			// Token: 0x04004DE2 RID: 19938
			public XmlInheritance.XmlInheritanceNode parent;

			// Token: 0x04004DE3 RID: 19939
			public List<XmlInheritance.XmlInheritanceNode> children = new List<XmlInheritance.XmlInheritanceNode>();
		}
	}
}
