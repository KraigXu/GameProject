using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class TreeNode_ThingCategory : TreeNode
	{
		
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000A9CF4 File Offset: 0x000A7EF4
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x000A9D01 File Offset: 0x000A7F01
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x000A9D0E File Offset: 0x000A7F0E
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

		
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x000A9D1E File Offset: 0x000A7F1E
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
