﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000840 RID: 2112
	public class ThoughtWorker_BedroomJealous : ThoughtWorker
	{
		// Token: 0x06003490 RID: 13456 RVA: 0x0012025C File Offset: 0x0011E45C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			float num = 0f;
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom != null)
			{
				num = ownedRoom.GetStat(RoomStatDefOf.Impressiveness);
			}
			List<Pawn> list = p.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			Pawn pawn = null;
			float num2 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].HostFaction == null && p.RaceProps.Humanlike && list[i].ownership != null)
				{
					Room ownedRoom2 = list[i].ownership.OwnedRoom;
					if (ownedRoom2 != null)
					{
						float stat = ownedRoom2.GetStat(RoomStatDefOf.Impressiveness);
						if (stat - num >= Mathf.Abs(num * 0.1f) && (pawn == null || stat > num2))
						{
							pawn = list[i];
							num2 = stat;
						}
					}
				}
			}
			if (pawn != null)
			{
				return ThoughtState.ActiveWithReason(pawn.LabelShort);
			}
			return false;
		}
	}
}
