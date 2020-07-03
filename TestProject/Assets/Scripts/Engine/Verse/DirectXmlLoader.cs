using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	
	public static class DirectXmlLoader
	{
		
		public static LoadableXmlAsset[] XmlAssetsInModFolder(ModContentPack mod, string folderPath, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int k = 0; k < list.Count; k++)
			{
				string text = list[k];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, folderPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						string key = fileInfo.FullName.Substring(text.Length + 1);
						if (!dictionary.ContainsKey(key))
						{
							dictionary.Add(key, fileInfo);
						}
					}
				}
			}
			if (dictionary.Count == 0)
			{
				return DirectXmlLoader.emptyXmlAssetsArray;
			}
			List<FileInfo> fileList = dictionary.Values.ToList<FileInfo>();
			LoadableXmlAsset[] assets = new LoadableXmlAsset[fileList.Count];
			GenThreading.ParallelFor(0, fileList.Count, delegate(int i)
			{
				FileInfo fileInfo2 = fileList[i];
				LoadableXmlAsset loadableXmlAsset = new LoadableXmlAsset(fileInfo2.Name, fileInfo2.Directory.FullName, File.ReadAllText(fileInfo2.FullName));
				loadableXmlAsset.mod = mod;
				assets[i] = loadableXmlAsset;
			}, -1);
			return assets;
		}

		
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			XmlInheritance.Clear();
			List<LoadableXmlAsset> assets = new List<LoadableXmlAsset>();
			object[] array = Resources.LoadAll<TextAsset>(folderPath);
			foreach (TextAsset textAsset in array)
			{
				LoadableXmlAsset loadableXmlAsset = new LoadableXmlAsset(textAsset.name, "", textAsset.text);
				XmlInheritance.TryRegisterAllFrom(loadableXmlAsset, null);
				assets.Add(loadableXmlAsset);
			}
			XmlInheritance.Resolve();
			int j;
			for (int i = 0; i < assets.Count; i = j + 1)
			{
				foreach (T t in DirectXmlLoader.AllGameItemsFromAsset<T>(assets[i]))
				{
					yield return t;
				}
				IEnumerator<T> enumerator = null;
				j = i;
			}
			XmlInheritance.Clear();
			yield break;
			yield break;
		}

		
		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!new FileInfo(filePath).Exists)
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(File.ReadAllText(filePath), filePath, resolveCrossRefs);
		}

		
		public static T ItemFromXmlFile<T>(VirtualDirectory directory, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!directory.FileExists(filePath))
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(directory.ReadAllText(filePath), filePath, resolveCrossRefs);
		}

		
		public static T ItemFromXmlString<T>(string xmlContent, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlString with resolveCrossRefs=true while loading is already in progress (forgot to resolve or clear cross refs from previous loading?).", false);
			}
			T result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlContent);
				T t = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, false);
				if (resolveCrossRefs)
				{
					try
					{
						DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
					}
					finally
					{
						DirectXmlCrossRefLoader.Clear();
					}
				}
				result = t;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString()+". Type is  "+typeof(T) , false);
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		
		public static Def DefFromNode(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return null;
			}
			XmlAttribute xmlAttribute = node.Attributes["Abstract"];
			if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
			{
				return null;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(node.Name, null);
			if (typeInAnyAssembly == null)
			{
				return null;
			}
			if (!typeof(Def).IsAssignableFrom(typeInAnyAssembly))
			{
				return null;
			}
			Func<XmlNode, bool, object> objectFromXmlMethod = DirectXmlToObject.GetObjectFromXmlMethod(typeInAnyAssembly);
			Def result = null;
			try
			{
				result = (Def)objectFromXmlMethod(node, true);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading def from file ",
					(loadingAsset != null) ? loadingAsset.name : "(unknown)",
					": ",
					ex
				}), false);
			}
			return result;
		}

		
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			if (asset.xmlDoc == null)
			{
				yield break;
			}
			XmlNodeList xmlNodeList = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
			bool gotData = false;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlAttribute xmlAttribute = xmlNode.Attributes["Abstract"];
				if (xmlAttribute == null || !(xmlAttribute.Value.ToLower() == "true"))
				{
					T t;
					try
					{
						t = DirectXmlToObject.ObjectFromXml<T>(xmlNode, true);
						gotData = true;
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception loading data from file ",
							asset.name,
							": ",
							ex
						}), false);
						continue;
					}
					yield return t;
				}
			}
			IEnumerator enumerator = null;
			if (!gotData)
			{
				Log.Error(string.Concat(new object[]
				{
					"Found no usable data when trying to get ",
					typeof(T),
					"s from file ",
					asset.name
				}), false);
			}
			yield break;
			yield break;
		}

		
		private static LoadableXmlAsset[] emptyXmlAssetsArray = new LoadableXmlAsset[0];
	}
}
