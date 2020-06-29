using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Verse
{
    
    public static class TKeySystem
    {
        
        public static void Clear()
        {
            TKeySystem.keys.Clear();
            TKeySystem.tKeyToNormalizedTranslationKey.Clear();
            TKeySystem.translationKeyToTKey.Clear();
            TKeySystem.loadErrors.Clear();
            TKeySystem.treatAsList.Clear();
        }

        
        public static void Parse(XmlDocument document)
        {
            foreach (object obj in document.ChildNodes[0].ChildNodes)
            {
                TKeySystem.ParseDefNode((XmlNode)obj);
            }
        }

        
        public static void MarkTreatAsList(XmlNode node)
        {
            TKeySystem.treatAsList.Add(node);
        }

        
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

            foreach (string typeName in (from k in TKeySystem.keys
                                         select k.defTypeName).Distinct<string>())
            {
                Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
                DefInjectionUtility.PossibleDefInjectionTraverser action = delegate (string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
                 {
                     string text2;
                     string value;
                     if (translationAllowed && !TKeySystem.TryGetNormalizedPath(suggestedPath, out text2) && TKeySystem.TrySuggestTKeyPath(normalizedPath, out value, tmpTranslationKeyToTKey))
                     {
                         tmpTranslationKeyToTKey.Add(suggestedPath, value);
                     }
                 };
                DefInjectionUtility.ForEachPossibleDefInjection(typeInAnyAssembly, action, null);
            }
            foreach (KeyValuePair<string, string> keyValuePair in tmpTranslationKeyToTKey)
            {
                TKeySystem.translationKeyToTKey.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        
        public static bool TryGetNormalizedPath(string tKeyPath, out string normalizedPath)
        {
            return TKeySystem.TryFindShortestReplacementPath(tKeyPath, delegate (string path, out string result)
            {
                return TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(path, out result);
            }, out normalizedPath);
        }

        
        private static bool TryFindShortestReplacementPath(string path, TKeySystem.PathMatcher matcher, out string result)
        {
            if (matcher(path, out result))
            {
                return true;
            }
            int num = 100;
            int num2 = path.Length - 1;
            for (; ; )
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

        
        public static bool TrySuggestTKeyPath(string translationPath, out string tKeyPath, Dictionary<string, string> lookup = null)
        {
            if (lookup == null)
            {
                lookup = TKeySystem.translationKeyToTKey;
            }
            return TKeySystem.TryFindShortestReplacementPath(translationPath, delegate (string path, out string result)
            {
                return lookup.TryGetValue(path, out result);
            }, out tKeyPath);
        }

        
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

        private static void ParseDefNode(XmlNode node)
        {
            string text = (from XmlNode n in node.ChildNodes
                           where n.Name == "defName"
                           select n.InnerText).FirstOrDefault<string>();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }


            //TKeySystem. c__DisplayClass17_0  c__DisplayClass17_;

            // c__DisplayClass17_.tKeyRefTemplate = default(TKeySystem.TKeyRef);

            // c__DisplayClass17_.tKeyRefTemplate.defName = text;

            // c__DisplayClass17_.tKeyRefTemplate.defTypeName = node.Name;

            // c__DisplayClass17_.tKeyRefTemplate.defRootNode = node;
            //TKeySystem.< ParseDefNode > g__CrawlNodesRecursive | 17_3(node, ref  c__DisplayClass17_);
        }


        private static List<TKeySystem.TKeyRef> keys = new List<TKeySystem.TKeyRef>();

        
        public static List<string> loadErrors = new List<string>();

        
        private static Dictionary<string, string> tKeyToNormalizedTranslationKey = new Dictionary<string, string>();

        
        private static Dictionary<string, string> translationKeyToTKey = new Dictionary<string, string>();

        
        private static HashSet<XmlNode> treatAsList = new HashSet<XmlNode>();

        
        public const string AttributeName = "TKey";

        
        private struct TKeyRef
        {
            
            public string defName;

            
            public string defTypeName;

            
            public XmlNode defRootNode;

            
            public XmlNode node;

            
            public string tKey;

            
            public string tKeyPath;
        }

        
        private struct PossibleDefInjection
        {
            
            public string normalizedPath;

            
            public string path;
        }

        
        
        private delegate bool PathMatcher(string path, out string match);
    }
}
