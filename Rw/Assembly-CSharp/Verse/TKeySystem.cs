using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000211 RID: 529
	public static class TKeySystem
	{
		// Token: 0x06000EE6 RID: 3814 RVA: 0x000549B0 File Offset: 0x00052BB0
		public static void Clear()
		{
			TKeySystem.keys.Clear();
			TKeySystem.tKeyToNormalizedTranslationKey.Clear();
			TKeySystem.translationKeyToTKey.Clear();
			TKeySystem.loadErrors.Clear();
			TKeySystem.treatAsList.Clear();
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000549E4 File Offset: 0x00052BE4
		public static void Parse(XmlDocument document)
		{
			foreach (object obj in document.ChildNodes[0].ChildNodes)
			{
				TKeySystem.ParseDefNode((XmlNode)obj);
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00054A48 File Offset: 0x00052C48
		public static void MarkTreatAsList(XmlNode node)
		{
			TKeySystem.treatAsList.Add(node);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00054A58 File Offset: 0x00052C58
		public static void BuildMappings()
		{
			Dictionary<string, string> tmpTranslationKeyToTKey = new Dictionary<string, string>();
			foreach (TKeySystem.TKeyRef tkeyRef in TKeySystem.keys)
			{
				string normalizedTranslationKey = TKeySystem.GetNormalizedTranslationKey(tkeyRef);
				string text;
				if (TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(tkeyRef.tKeyPath, out text))
				{
					TKeySystem.loadErrors.Add(string.Concat(new string[]
					{
						"Duplicate TKey: ",
						tkeyRef.tKeyPath,
						" -> NEW=",
						normalizedTranslationKey,
						" | OLD",
						text,
						" - Ignoring old"
					}));
				}
				else
				{
					TKeySystem.tKeyToNormalizedTranslationKey.Add(tkeyRef.tKeyPath, normalizedTranslationKey);
					tmpTranslationKeyToTKey.Add(normalizedTranslationKey, tkeyRef.tKeyPath);
				}
			}
			DefInjectionUtility.PossibleDefInjectionTraverser <>9__1;
			foreach (string typeName in (from k in TKeySystem.keys
			select k.defTypeName).Distinct<string>())
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
				DefInjectionUtility.PossibleDefInjectionTraverser action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
					{
						string text2;
						string value;
						if (translationAllowed && !TKeySystem.TryGetNormalizedPath(suggestedPath, out text2) && TKeySystem.TrySuggestTKeyPath(normalizedPath, out value, tmpTranslationKeyToTKey))
						{
							tmpTranslationKeyToTKey.Add(suggestedPath, value);
						}
					});
				}
				DefInjectionUtility.ForEachPossibleDefInjection(typeInAnyAssembly, action, null);
			}
			foreach (KeyValuePair<string, string> keyValuePair in tmpTranslationKeyToTKey)
			{
				TKeySystem.translationKeyToTKey.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00054C24 File Offset: 0x00052E24
		public static bool TryGetNormalizedPath(string tKeyPath, out string normalizedPath)
		{
			return TKeySystem.TryFindShortestReplacementPath(tKeyPath, delegate(string path, out string result)
			{
				return TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(path, out result);
			}, out normalizedPath);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00054C4C File Offset: 0x00052E4C
		private static bool TryFindShortestReplacementPath(string path, TKeySystem.PathMatcher matcher, out string result)
		{
			if (matcher(path, out result))
			{
				return true;
			}
			int num = 100;
			int num2 = path.Length - 1;
			for (;;)
			{
				if (num2 > 0 && path[num2] != '.')
				{
					num2--;
				}
				else
				{
					if (path[num2] == '.')
					{
						string path2 = path.Substring(0, num2);
						if (matcher(path2, out result))
						{
							break;
						}
					}
					num2--;
					num--;
					if (num2 <= 0 || num <= 0)
					{
						goto IL_6D;
					}
				}
			}
			result += path.Substring(num2);
			return true;
			IL_6D:
			result = null;
			return false;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00054CCC File Offset: 0x00052ECC
		public static bool TrySuggestTKeyPath(string translationPath, out string tKeyPath, Dictionary<string, string> lookup = null)
		{
			if (lookup == null)
			{
				lookup = TKeySystem.translationKeyToTKey;
			}
			return TKeySystem.TryFindShortestReplacementPath(translationPath, delegate(string path, out string result)
			{
				return lookup.TryGetValue(path, out result);
			}, out tKeyPath);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00054D0C File Offset: 0x00052F0C
		private static string GetNormalizedTranslationKey(TKeySystem.TKeyRef tKeyRef)
		{
			string text = "";
			XmlNode currentNode;
			for (currentNode = tKeyRef.node; currentNode != tKeyRef.defRootNode; currentNode = currentNode.ParentNode)
			{
				if (currentNode.Name == "li" || TKeySystem.treatAsList.Contains(currentNode.ParentNode))
				{
					text = "." + currentNode.ParentNode.ChildNodes.Cast<XmlNode>().FirstIndexOf((XmlNode n) => n == currentNode) + text;
				}
				else
				{
					text = "." + currentNode.Name + text;
				}
			}
			return tKeyRef.defName + text;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00054DE4 File Offset: 0x00052FE4
		private static void ParseDefNode(XmlNode node)
		{
			string text = (from XmlNode n in node.ChildNodes
			where n.Name == "defName"
			select n.InnerText).FirstOrDefault<string>();
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			TKeySystem.<>c__DisplayClass17_0 <>c__DisplayClass17_;
			<>c__DisplayClass17_.tKeyRefTemplate = default(TKeySystem.TKeyRef);
			<>c__DisplayClass17_.tKeyRefTemplate.defName = text;
			<>c__DisplayClass17_.tKeyRefTemplate.defTypeName = node.Name;
			<>c__DisplayClass17_.tKeyRefTemplate.defRootNode = node;
			TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|17_3(node, ref <>c__DisplayClass17_);
		}

		// Token: 0x04000B03 RID: 2819
		private static List<TKeySystem.TKeyRef> keys = new List<TKeySystem.TKeyRef>();

		// Token: 0x04000B04 RID: 2820
		public static List<string> loadErrors = new List<string>();

		// Token: 0x04000B05 RID: 2821
		private static Dictionary<string, string> tKeyToNormalizedTranslationKey = new Dictionary<string, string>();

		// Token: 0x04000B06 RID: 2822
		private static Dictionary<string, string> translationKeyToTKey = new Dictionary<string, string>();

		// Token: 0x04000B07 RID: 2823
		private static HashSet<XmlNode> treatAsList = new HashSet<XmlNode>();

		// Token: 0x04000B08 RID: 2824
		public const string AttributeName = "TKey";

		// Token: 0x0200141D RID: 5149
		private struct TKeyRef
		{
			// Token: 0x04004C72 RID: 19570
			public string defName;

			// Token: 0x04004C73 RID: 19571
			public string defTypeName;

			// Token: 0x04004C74 RID: 19572
			public XmlNode defRootNode;

			// Token: 0x04004C75 RID: 19573
			public XmlNode node;

			// Token: 0x04004C76 RID: 19574
			public string tKey;

			// Token: 0x04004C77 RID: 19575
			public string tKeyPath;
		}

		// Token: 0x0200141E RID: 5150
		private struct PossibleDefInjection
		{
			// Token: 0x04004C78 RID: 19576
			public string normalizedPath;

			// Token: 0x04004C79 RID: 19577
			public string path;
		}

		// Token: 0x0200141F RID: 5151
		// (Invoke) Token: 0x06007928 RID: 31016
		private delegate bool PathMatcher(string path, out string match);
	}
}
