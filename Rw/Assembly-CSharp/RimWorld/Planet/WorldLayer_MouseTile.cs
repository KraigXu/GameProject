using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E9 RID: 4585
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06006A1B RID: 27163 RVA: 0x0025049C File Offset: 0x0024E69C
		protected override int Tile
		{
			get
			{
				if (Find.World.UI.selector.dragBox.IsValidAndActive)
				{
					return -1;
				}
				if (Find.WorldTargeter.IsTargeting)
				{
					return -1;
				}
				if (Find.ScreenshotModeHandler.Active)
				{
					return -1;
				}
				return GenWorld.MouseTile(false);
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06006A1C RID: 27164 RVA: 0x002504E8 File Offset: 0x0024E6E8
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
