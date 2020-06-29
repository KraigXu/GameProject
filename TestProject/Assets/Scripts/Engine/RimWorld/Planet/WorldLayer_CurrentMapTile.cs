using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		
		// (get) Token: 0x06006A11 RID: 27153 RVA: 0x002503BB File Offset: 0x0024E5BB
		protected override int Tile
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return -1;
				}
				if (Find.CurrentMap == null)
				{
					return -1;
				}
				return Find.CurrentMap.Tile;
			}
		}

		
		// (get) Token: 0x06006A12 RID: 27154 RVA: 0x002503DA File Offset: 0x0024E5DA
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
