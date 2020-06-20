using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Spirit
{

	public static class LoadedModManager
	{

		//// Token: 0x170002AD RID: 685
		//// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0004F809 File Offset: 0x0004DA09
		//public static List<ModContentPack> RunningModsListForReading
		//{
		//	get
		//	{
		//		return LoadedModManager.runningMods;
		//	}
		//}

		//// Token: 0x170002AE RID: 686
		//// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0004F809 File Offset: 0x0004DA09
		//public static IEnumerable<ModContentPack> RunningMods
		//{
		//	get
		//	{
		//		return LoadedModManager.runningMods;
		//	}
		//}

		//// Token: 0x170002AF RID: 687
		//// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0004F810 File Offset: 0x0004DA10
		//public static List<Def> PatchedDefsForReading
		//{
		//	get
		//	{
		//		return LoadedModManager.patchedDefs;
		//	}
		//}

		//// Token: 0x170002B0 RID: 688
		//// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0004F817 File Offset: 0x0004DA17
		//public static IEnumerable<Mod> ModHandles
		//{
		//	get
		//	{
		//		return LoadedModManager.runningModClasses.Values;
		//	}
		//}

		//// Token: 0x06000DF4 RID: 3572 RVA: 0x0004F824 File Offset: 0x0004DA24
		//public static void LoadAllActiveMods()
		//{
		//	DeepProfiler.Start("XmlInheritance.Clear()");
		//	try
		//	{
		//		XmlInheritance.Clear();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("InitializeMods()");
		//	try
		//	{
		//		LoadedModManager.InitializeMods();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("LoadModContent()");
		//	try
		//	{
		//		LoadedModManager.LoadModContent();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("CreateModClasses()");
		//	try
		//	{
		//		LoadedModManager.CreateModClasses();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	List<LoadableXmlAsset> xmls = null;
		//	DeepProfiler.Start("LoadModXML()");
		//	try
		//	{
		//		xmls = LoadedModManager.LoadModXML();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	Dictionary<XmlNode, LoadableXmlAsset> assetlookup = new Dictionary<XmlNode, LoadableXmlAsset>();
		//	XmlDocument xmlDocument = null;
		//	DeepProfiler.Start("CombineIntoUnifiedXML()");
		//	try
		//	{
		//		xmlDocument = LoadedModManager.CombineIntoUnifiedXML(xmls, assetlookup);
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	TKeySystem.Clear();
		//	DeepProfiler.Start("TKeySystem.Parse()");
		//	try
		//	{
		//		TKeySystem.Parse(xmlDocument);
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("ApplyPatches()");
		//	try
		//	{
		//		LoadedModManager.ApplyPatches(xmlDocument, assetlookup);
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("ParseAndProcessXML()");
		//	try
		//	{
		//		LoadedModManager.ParseAndProcessXML(xmlDocument, assetlookup);
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("ClearCachedPatches()");
		//	try
		//	{
		//		LoadedModManager.ClearCachedPatches();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("XmlInheritance.Clear()");
		//	try
		//	{
		//		XmlInheritance.Clear();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//}

		//// Token: 0x06000DF5 RID: 3573 RVA: 0x0004F9D0 File Offset: 0x0004DBD0
		//public static void InitializeMods()
		//{
		//	int num = 0;
		//	foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>())
		//	{
		//		DeepProfiler.Start("Initializing " + modMetaData);
		//		try
		//		{
		//			if (!modMetaData.RootDir.Exists)
		//			{
		//				ModsConfig.SetActive(modMetaData.PackageId, false);
		//				Log.Warning(string.Concat(new object[]
		//				{
		//					"Failed to find active mod ",
		//					modMetaData.Name,
		//					"(",
		//					modMetaData.PackageIdPlayerFacing,
		//					") at ",
		//					modMetaData.RootDir
		//				}), false);
		//			}
		//			else
		//			{
		//				ModContentPack item = new ModContentPack(modMetaData.RootDir, modMetaData.PackageId, modMetaData.PackageIdPlayerFacing, num, modMetaData.Name);
		//				num++;
		//				LoadedModManager.runningMods.Add(item);
		//			}
		//		}
		//		catch (Exception arg)
		//		{
		//			Log.Error("Error initializing mod: " + arg, false);
		//			ModsConfig.SetActive(modMetaData.PackageId, false);
		//		}
		//		finally
		//		{
		//			DeepProfiler.End();
		//		}
		//	}
		//}

		//// Token: 0x06000DF6 RID: 3574 RVA: 0x0004FB08 File Offset: 0x0004DD08
		//public static void LoadModContent()
		//{
		//	LongEventHandler.ExecuteWhenFinished(delegate
		//	{
		//		DeepProfiler.Start("LoadModContent");
		//	});
		//	for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
		//	{
		//		ModContentPack modContentPack = LoadedModManager.runningMods[i];
		//		DeepProfiler.Start("Loading " + modContentPack + " content");
		//		try
		//		{
		//			modContentPack.ReloadContent();
		//		}
		//		catch (Exception ex)
		//		{
		//			Log.Error(string.Concat(new object[]
		//			{
		//				"Could not reload mod content for mod ",
		//				modContentPack.PackageIdPlayerFacing,
		//				": ",
		//				ex
		//			}), false);
		//		}
		//		finally
		//		{
		//			DeepProfiler.End();
		//		}
		//	}
		//	LongEventHandler.ExecuteWhenFinished(delegate
		//	{
		//		DeepProfiler.End();
		//		for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
		//		{
		//			ModContentPack modContentPack2 = LoadedModManager.runningMods[j];
		//			if (!modContentPack2.AnyContentLoaded())
		//			{
		//				Log.Error("Mod " + modContentPack2.Name + " did not load any content. Following load folders were used:\n" + modContentPack2.foldersToLoadDescendingOrder.ToLineList("  - "), false);
		//			}
		//		}
		//	});
		//}

		//// Token: 0x06000DF7 RID: 3575 RVA: 0x0004FBF0 File Offset: 0x0004DDF0
		//public static void CreateModClasses()
		//{
		//	using (IEnumerator<Type> enumerator = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
		//	{
		//		while (enumerator.MoveNext())
		//		{
		//			Type type = enumerator.Current;
		//			DeepProfiler.Start("Loading " + type + " mod class");
		//			try
		//			{
		//				if (!LoadedModManager.runningModClasses.ContainsKey(type))
		//				{
		//					ModContentPack modContentPack = (from modpack in LoadedModManager.runningMods
		//													 where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
		//													 select modpack).FirstOrDefault<ModContentPack>();
		//					LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, new object[]
		//					{
		//						modContentPack
		//					});
		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				Log.Error(string.Concat(new object[]
		//				{
		//					"Error while instantiating a mod of type ",
		//					type,
		//					": ",
		//					ex
		//				}), false);
		//			}
		//			finally
		//			{
		//				DeepProfiler.End();
		//			}
		//		}
		//	}
		//}

		//// Token: 0x06000DF8 RID: 3576 RVA: 0x0004FD18 File Offset: 0x0004DF18
		//public static List<LoadableXmlAsset> LoadModXML()
		//{
		//	List<LoadableXmlAsset> list = new List<LoadableXmlAsset>();
		//	for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
		//	{
		//		ModContentPack modContentPack = LoadedModManager.runningMods[i];
		//		DeepProfiler.Start("Loading " + modContentPack);
		//		try
		//		{
		//			list.AddRange(modContentPack.LoadDefs());
		//		}
		//		catch (Exception ex)
		//		{
		//			Log.Error(string.Concat(new object[]
		//			{
		//				"Could not load defs for mod ",
		//				modContentPack.PackageIdPlayerFacing,
		//				": ",
		//				ex
		//			}), false);
		//		}
		//		finally
		//		{
		//			DeepProfiler.End();
		//		}
		//	}
		//	return list;
		//}

		//// Token: 0x06000DF9 RID: 3577 RVA: 0x0004FDC0 File Offset: 0x0004DFC0
		//public static void ApplyPatches(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		//{
		//	foreach (PatchOperation patchOperation in LoadedModManager.runningMods.SelectMany((ModContentPack rm) => rm.Patches))
		//	{
		//		try
		//		{
		//			patchOperation.Apply(xmlDoc);
		//		}
		//		catch (Exception arg)
		//		{
		//			Log.Error("Error in patch.Apply(): " + arg, false);
		//		}
		//	}
		//}

		//// Token: 0x06000DFA RID: 3578 RVA: 0x0004FE54 File Offset: 0x0004E054
		//public static XmlDocument CombineIntoUnifiedXML(List<LoadableXmlAsset> xmls, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		//{
		//	XmlDocument xmlDocument = new XmlDocument();
		//	xmlDocument.AppendChild(xmlDocument.CreateElement("Defs"));
		//	foreach (LoadableXmlAsset loadableXmlAsset in xmls)
		//	{
		//		if (loadableXmlAsset.xmlDoc == null || loadableXmlAsset.xmlDoc.DocumentElement == null)
		//		{
		//			Log.Error(string.Format("{0}: unknown parse failure", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name), false);
		//		}
		//		else
		//		{
		//			if (loadableXmlAsset.xmlDoc.DocumentElement.Name != "Defs")
		//			{
		//				Log.Error(string.Format("{0}: root element named {1}; should be named Defs", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name, loadableXmlAsset.xmlDoc.DocumentElement.Name), false);
		//			}
		//			foreach (object obj in loadableXmlAsset.xmlDoc.DocumentElement.ChildNodes)
		//			{
		//				XmlNode node = (XmlNode)obj;
		//				XmlNode xmlNode = xmlDocument.ImportNode(node, true);
		//				assetlookup[xmlNode] = loadableXmlAsset;
		//				xmlDocument.DocumentElement.AppendChild(xmlNode);
		//			}
		//		}
		//	}
		//	return xmlDocument;
		//}

		//// Token: 0x06000DFB RID: 3579 RVA: 0x0004FFD8 File Offset: 0x0004E1D8
		//public static void ParseAndProcessXML(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		//{
		//	XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
		//	List<XmlNode> list = new List<XmlNode>();
		//	foreach (object obj in childNodes)
		//	{
		//		list.Add(obj as XmlNode);
		//	}
		//	DeepProfiler.Start("Loading asset nodes " + list.Count);
		//	try
		//	{
		//		for (int i = 0; i < list.Count; i++)
		//		{
		//			if (list[i].NodeType == XmlNodeType.Element)
		//			{
		//				LoadableXmlAsset loadableXmlAsset = null;
		//				DeepProfiler.Start("assetlookup.TryGetValue");
		//				try
		//				{
		//					assetlookup.TryGetValue(list[i], out loadableXmlAsset);
		//				}
		//				finally
		//				{
		//					DeepProfiler.End();
		//				}
		//				DeepProfiler.Start("XmlInheritance.TryRegister");
		//				try
		//				{
		//					XmlInheritance.TryRegister(list[i], (loadableXmlAsset != null) ? loadableXmlAsset.mod : null);
		//				}
		//				finally
		//				{
		//					DeepProfiler.End();
		//				}
		//			}
		//		}
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	DeepProfiler.Start("XmlInheritance.Resolve()");
		//	try
		//	{
		//		XmlInheritance.Resolve();
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//	LoadedModManager.runningMods.FirstOrDefault<ModContentPack>();
		//	DeepProfiler.Start("Loading defs for " + list.Count + " nodes");
		//	try
		//	{
		//		foreach (XmlNode xmlNode in list)
		//		{
		//			LoadableXmlAsset loadableXmlAsset2 = assetlookup.TryGetValue(xmlNode, null);
		//			Def def = DirectXmlLoader.DefFromNode(xmlNode, loadableXmlAsset2);
		//			if (def != null)
		//			{
		//				ModContentPack modContentPack = (loadableXmlAsset2 != null) ? loadableXmlAsset2.mod : null;
		//				if (modContentPack != null)
		//				{
		//					modContentPack.AddDef(def, loadableXmlAsset2.name);
		//				}
		//				else
		//				{
		//					LoadedModManager.patchedDefs.Add(def);
		//				}
		//			}
		//		}
		//	}
		//	finally
		//	{
		//		DeepProfiler.End();
		//	}
		//}

		//// Token: 0x06000DFC RID: 3580 RVA: 0x000501E0 File Offset: 0x0004E3E0
		//public static void ClearCachedPatches()
		//{
		//	foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
		//	{
		//		foreach (PatchOperation patchOperation in modContentPack.Patches)
		//		{
		//			try
		//			{
		//				patchOperation.Complete(modContentPack.Name);
		//			}
		//			catch (Exception arg)
		//			{
		//				Log.Error("Error in patch.Complete(): " + arg, false);
		//			}
		//		}
		//		modContentPack.ClearPatchesCache();
		//	}
		//}

		//// Token: 0x06000DFD RID: 3581 RVA: 0x00050298 File Offset: 0x0004E498
		//public static void ClearDestroy()
		//{
		//	foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
		//	{
		//		try
		//		{
		//			modContentPack.ClearDestroy();
		//		}
		//		catch (Exception arg)
		//		{
		//			Log.Error("Error in mod.ClearDestroy(): " + arg, false);
		//		}
		//	}
		//	LoadedModManager.runningMods.Clear();
		//}

		//// Token: 0x06000DFE RID: 3582 RVA: 0x00050318 File Offset: 0x0004E518
		//public static T GetMod<T>() where T : Mod
		//{
		//	return LoadedModManager.GetMod(typeof(T)) as T;
		//}

		//// Token: 0x06000DFF RID: 3583 RVA: 0x00050334 File Offset: 0x0004E534
		//public static Mod GetMod(Type type)
		//{
		//	if (LoadedModManager.runningModClasses.ContainsKey(type))
		//	{
		//		return LoadedModManager.runningModClasses[type];
		//	}
		//	return (from kvp in LoadedModManager.runningModClasses
		//			where type.IsAssignableFrom(kvp.Key)
		//			select kvp).FirstOrDefault<KeyValuePair<Type, Mod>>().Value;
		//}

		//// Token: 0x06000E00 RID: 3584 RVA: 0x00050394 File Offset: 0x0004E594
		//private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		//{
		//	return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		//}

		//// Token: 0x06000E01 RID: 3585 RVA: 0x000503B4 File Offset: 0x0004E5B4
		//public static T ReadModSettings<T>(string modIdentifier, string modHandleName) where T : ModSettings, new()
		//{
		//	string settingsFilename = LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName);
		//	T t = default(T);
		//	try
		//	{
		//		if (File.Exists(settingsFilename))
		//		{
		//			Scribe.loader.InitLoading(settingsFilename);
		//			try
		//			{
		//				Scribe_Deep.Look<T>(ref t, "ModSettings", Array.Empty<object>());
		//			}
		//			finally
		//			{
		//				Scribe.loader.FinalizeLoading();
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Log.Warning(string.Format("Caught exception while loading mod settings data for {0}. Generating fresh settings. The exception was: {1}", modIdentifier, ex.ToString()), false);
		//		t = default(T);
		//	}
		//	if (t == null)
		//	{
		//		t = Activator.CreateInstance<T>();
		//	}
		//	return t;
		//}

		//// Token: 0x06000E02 RID: 3586 RVA: 0x00050454 File Offset: 0x0004E654
		//public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		//{
		//	Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
		//	try
		//	{
		//		Scribe_Deep.Look<ModSettings>(ref settings, "ModSettings", Array.Empty<object>());
		//	}
		//	finally
		//	{
		//		Scribe.saver.FinalizeSaving();
		//	}
		//}

		//// Token: 0x04000A9A RID: 2714
		//private static List<ModContentPack> runningMods = new List<ModContentPack>();

		//// Token: 0x04000A9B RID: 2715
		//private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();

		//// Token: 0x04000A9C RID: 2716
		//private static List<Def> patchedDefs = new List<Def>();
	}
}
