using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091F RID: 2335
	public class WorldObjectDef : Def
	{
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06003772 RID: 14194 RVA: 0x00129BF3 File Offset: 0x00127DF3
		public Material Material
		{
			get
			{
				if (this.texture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					this.material = MaterialPool.MatFrom(this.texture, ShaderDatabase.WorldOverlayTransparentLit, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.material;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003773 RID: 14195 RVA: 0x00129C33 File Offset: 0x00127E33
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x00129C6C File Offset: 0x00127E6C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x00129D18 File Offset: 0x00127F18
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x00129D53 File Offset: 0x00127F53
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			if (this.expandMore && !this.expandingIcon)
			{
				yield return "has expandMore but doesn't have any expanding icon";
			}
			yield break;
			yield break;
		}

		// Token: 0x040020D2 RID: 8402
		public Type worldObjectClass = typeof(WorldObject);

		// Token: 0x040020D3 RID: 8403
		public bool canHaveFaction = true;

		// Token: 0x040020D4 RID: 8404
		public bool saved = true;

		// Token: 0x040020D5 RID: 8405
		public bool canBePlayerHome;

		// Token: 0x040020D6 RID: 8406
		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		// Token: 0x040020D7 RID: 8407
		public bool allowCaravanIncidentsWhichGenerateMap;

		// Token: 0x040020D8 RID: 8408
		public bool isTempIncidentMapOwner;

		// Token: 0x040020D9 RID: 8409
		public List<IncidentTargetTagDef> IncidentTargetTags;

		// Token: 0x040020DA RID: 8410
		public bool selectable = true;

		// Token: 0x040020DB RID: 8411
		public bool neverMultiSelect;

		// Token: 0x040020DC RID: 8412
		public MapGeneratorDef mapGenerator;

		// Token: 0x040020DD RID: 8413
		public List<Type> inspectorTabs;

		// Token: 0x040020DE RID: 8414
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x040020DF RID: 8415
		public bool useDynamicDrawer;

		// Token: 0x040020E0 RID: 8416
		public bool expandingIcon;

		// Token: 0x040020E1 RID: 8417
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x040020E2 RID: 8418
		public float expandingIconPriority;

		// Token: 0x040020E3 RID: 8419
		[NoTranslate]
		public string texture;

		// Token: 0x040020E4 RID: 8420
		[Unsaved(false)]
		private Material material;

		// Token: 0x040020E5 RID: 8421
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x040020E6 RID: 8422
		public bool expandMore;

		// Token: 0x040020E7 RID: 8423
		public bool blockExitGridUntilBattleIsWon;
	}
}
