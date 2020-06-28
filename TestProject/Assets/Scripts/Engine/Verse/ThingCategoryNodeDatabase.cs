using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003AC RID: 940
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x000A9ADA File Offset: 0x000A7CDA
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000A9AE1 File Offset: 0x000A7CE1
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000A9AF0 File Offset: 0x000A7CF0
		public static void FinalizeInit()
		{
			ThingCategoryNodeDatabase.rootNode = ThingCategoryDefOf.Root.treeNode;
			foreach (ThingCategoryDef thingCategoryDef in DefDatabase<ThingCategoryDef>.AllDefs)
			{
				if (thingCategoryDef.parent != null)
				{
					thingCategoryDef.parent.childCategories.Add(thingCategoryDef);
				}
			}
			ThingCategoryNodeDatabase.SetNestLevelRecursive(ThingCategoryNodeDatabase.rootNode, 0);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingCategories != null)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingDef.thingCategories)
					{
						thingCategoryDef2.childThingDefs.Add(thingDef);
					}
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				specialThingFilterDef.parentCategory.childSpecialFilters.Add(specialThingFilterDef);
			}
			if (ThingCategoryNodeDatabase.rootNode.catDef.childCategories.Any<ThingCategoryDef>())
			{
				ThingCategoryNodeDatabase.rootNode.catDef.childCategories[0].treeNode.SetOpen(-1, true);
			}
			ThingCategoryNodeDatabase.allThingCategoryNodes = ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis.ToList<TreeNode_ThingCategory>();
			ThingCategoryNodeDatabase.initialized = true;
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000A9C88 File Offset: 0x000A7E88
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}

		// Token: 0x04001056 RID: 4182
		public static bool initialized;

		// Token: 0x04001057 RID: 4183
		private static TreeNode_ThingCategory rootNode;

		// Token: 0x04001058 RID: 4184
		public static List<TreeNode_ThingCategory> allThingCategoryNodes;
	}
}
