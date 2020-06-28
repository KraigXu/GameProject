using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011EE RID: 4590
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06006A2E RID: 27182 RVA: 0x00250E74 File Offset: 0x0024F074
		protected override int Tile
		{
			get
			{
				if (!(Find.WorldInterface.inspectPane.mouseoverGizmo is Command_Settle))
				{
					return -1;
				}
				Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
				if (caravan == null)
				{
					return -1;
				}
				return caravan.Tile;
			}
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06006A2F RID: 27183 RVA: 0x002503DA File Offset: 0x0024E5DA
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06006A30 RID: 27184 RVA: 0x00250EB4 File Offset: 0x0024F0B4
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
