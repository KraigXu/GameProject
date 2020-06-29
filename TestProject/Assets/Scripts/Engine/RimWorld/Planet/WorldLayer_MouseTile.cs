using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		
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
