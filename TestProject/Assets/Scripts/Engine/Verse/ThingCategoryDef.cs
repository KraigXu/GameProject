using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F0 RID: 240
	public class ThingCategoryDef : Def
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x0001E646 File Offset: 0x0001C846
		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				if (this.parent != null)
				{
					yield return this.parent;
					foreach (ThingCategoryDef thingCategoryDef in this.parent.Parents)
					{
						yield return thingCategoryDef;
					}
					IEnumerator<ThingCategoryDef> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001E656 File Offset: 0x0001C856
		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				yield return this;
				foreach (ThingCategoryDef thingCategoryDef in this.childCategories)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingCategoryDef.ThisAndChildCategoryDefs)
					{
						yield return thingCategoryDef2;
					}
					IEnumerator<ThingCategoryDef> enumerator2 = null;
				}
				List<ThingCategoryDef>.Enumerator enumerator = default(List<ThingCategoryDef>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001E666 File Offset: 0x0001C866
		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
				{
					foreach (ThingDef thingDef in thingCategoryDef.childThingDefs)
					{
						yield return thingDef;
					}
					List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0001E676 File Offset: 0x0001C876
		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
				{
					foreach (SpecialThingFilterDef specialThingFilterDef in thingCategoryDef.childSpecialFilters)
					{
						yield return specialThingFilterDef;
					}
					List<SpecialThingFilterDef>.Enumerator enumerator2 = default(List<SpecialThingFilterDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001E686 File Offset: 0x0001C886
		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef thingCategoryDef in this.Parents)
				{
					foreach (SpecialThingFilterDef specialThingFilterDef in thingCategoryDef.childSpecialFilters)
					{
						yield return specialThingFilterDef;
					}
					List<SpecialThingFilterDef>.Enumerator enumerator2 = default(List<SpecialThingFilterDef>.Enumerator);
				}
				IEnumerator<ThingCategoryDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001E696 File Offset: 0x0001C896
		public bool ContainedInThisOrDescendant(ThingDef thingDef)
		{
			return this.allChildThingDefsCached.Contains(thingDef);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
		public override void ResolveReferences()
		{
			this.allChildThingDefsCached = new HashSet<ThingDef>();
			foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
			{
				foreach (ThingDef item in thingCategoryDef.childThingDefs)
				{
					this.allChildThingDefsCached.Add(item);
				}
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001E73C File Offset: 0x0001C93C
		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001E768 File Offset: 0x0001C968
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00016DC7 File Offset: 0x00014FC7
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x040005A8 RID: 1448
		public ThingCategoryDef parent;

		// Token: 0x040005A9 RID: 1449
		[NoTranslate]
		public string iconPath;

		// Token: 0x040005AA RID: 1450
		public bool resourceReadoutRoot;

		// Token: 0x040005AB RID: 1451
		[Unsaved(false)]
		public TreeNode_ThingCategory treeNode;

		// Token: 0x040005AC RID: 1452
		[Unsaved(false)]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x040005AD RID: 1453
		[Unsaved(false)]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x040005AE RID: 1454
		[Unsaved(false)]
		private HashSet<ThingDef> allChildThingDefsCached;

		// Token: 0x040005AF RID: 1455
		[Unsaved(false)]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x040005B0 RID: 1456
		[Unsaved(false)]
		public Texture2D icon = BaseContent.BadTex;
	}
}
