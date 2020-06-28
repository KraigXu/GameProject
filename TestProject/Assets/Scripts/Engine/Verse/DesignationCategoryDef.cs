using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020000AA RID: 170
	public class DesignationCategoryDef : Def
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001AFF0 File Offset: 0x000191F0
		public IEnumerable<Designator> ResolvedAllowedDesignators
		{
			get
			{
				GameRules rules = Current.Game.Rules;
				int num;
				for (int i = 0; i < this.resolvedDesignators.Count; i = num + 1)
				{
					Designator designator = this.resolvedDesignators[i];
					if (rules == null || rules.DesignatorAllowed(designator))
					{
						yield return designator;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001B000 File Offset: 0x00019200
		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001B008 File Offset: 0x00019208
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + this.defName + "-Closed";
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001B03C File Offset: 0x0001923C
		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			foreach (Type type in this.specialDesignatorClasses)
			{
				Designator designator = null;
				try
				{
					designator = (Designator)Activator.CreateInstance(type);
					designator.isOrder = true;
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"DesignationCategoryDef",
						this.defName,
						" could not instantiate special designator from class ",
						type,
						".\n Exception: \n",
						ex.ToString()
					}), false);
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this
			select tDef;
			Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> dictionary = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();
			foreach (BuildableDef buildableDef in enumerable)
			{
				if (buildableDef.designatorDropdown != null)
				{
					if (!dictionary.ContainsKey(buildableDef.designatorDropdown))
					{
						dictionary[buildableDef.designatorDropdown] = new Designator_Dropdown();
						this.resolvedDesignators.Add(dictionary[buildableDef.designatorDropdown]);
					}
					dictionary[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				else
				{
					this.resolvedDesignators.Add(new Designator_Build(buildableDef));
				}
			}
		}

		// Token: 0x04000370 RID: 880
		public List<Type> specialDesignatorClasses = new List<Type>();

		// Token: 0x04000371 RID: 881
		public int order;

		// Token: 0x04000372 RID: 882
		public bool showPowerGrid;

		// Token: 0x04000373 RID: 883
		[Unsaved(false)]
		private List<Designator> resolvedDesignators = new List<Designator>();

		// Token: 0x04000374 RID: 884
		[Unsaved(false)]
		public KeyBindingCategoryDef bindingCatDef;

		// Token: 0x04000375 RID: 885
		[Unsaved(false)]
		public string cachedHighlightClosedTag;
	}
}
