﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	
	public static class ThingCategoryNodeDatabase
	{
		
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x000A9ADA File Offset: 0x000A7CDA
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		
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

		
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}

		
		public static bool initialized;

		
		private static TreeNode_ThingCategory rootNode;

		
		public static List<TreeNode_ThingCategory> allThingCategoryNodes;
	}
}
