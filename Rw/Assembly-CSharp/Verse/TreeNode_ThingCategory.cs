using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003AD RID: 941
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000A9CF4 File Offset: 0x000A7EF4
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x000A9D01 File Offset: 0x000A7F01
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000559 RID: 1369
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

		// Token: 0x1700055A RID: 1370
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

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000A9D2E File Offset: 0x000A7F2E
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000A9D3D File Offset: 0x000A7F3D
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x04001059 RID: 4185
		public ThingCategoryDef catDef;
	}
}
