using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		
		// (get) Token: 0x06006A2B RID: 27179 RVA: 0x00250E5E File Offset: 0x0024F05E
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		
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
