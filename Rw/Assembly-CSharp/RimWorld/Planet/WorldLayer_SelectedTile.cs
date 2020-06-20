using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011ED RID: 4589
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06006A2B RID: 27179 RVA: 0x00250E5E File Offset: 0x0024F05E
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06006A2C RID: 27180 RVA: 0x00250E6A File Offset: 0x0024F06A
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
