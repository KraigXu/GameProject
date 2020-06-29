using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class ThingCategoryDef : Def
	{
		
		
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

		
		public bool ContainedInThisOrDescendant(ThingDef thingDef)
		{
			return this.allChildThingDefsCached.Contains(thingDef);
		}

		
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

		
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		
		public ThingCategoryDef parent;

		
		[NoTranslate]
		public string iconPath;

		
		public bool resourceReadoutRoot;

		
		[Unsaved(false)]
		public TreeNode_ThingCategory treeNode;

		
		[Unsaved(false)]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		
		[Unsaved(false)]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		
		[Unsaved(false)]
		private HashSet<ThingDef> allChildThingDefsCached;

		
		[Unsaved(false)]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		
		[Unsaved(false)]
		public Texture2D icon = BaseContent.BadTex;
	}
}
