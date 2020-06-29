using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class TreeNode_ThingCategory : TreeNode
	{
		
		
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		
		
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		
		
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.catDef.ThisAndChildCategoryDefs)
				{
					yield return thingCategoryDef.treeNode;
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		
		
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.catDef.childCategories)
				{
					yield return thingCategoryDef.treeNode;
				}
				List<ThingCategoryDef>.Enumerator enumerator = default(List<ThingCategoryDef>.Enumerator);
				yield break;
				yield break;
			}
		}

		
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		
		public override string ToString()
		{
			return this.catDef.defName;
		}

		
		public ThingCategoryDef catDef;
	}
}
