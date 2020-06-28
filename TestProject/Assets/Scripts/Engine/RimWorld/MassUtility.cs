using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCB RID: 4043
	public static class MassUtility
	{
		// Token: 0x06006115 RID: 24853 RVA: 0x0021B107 File Offset: 0x00219307
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x0021B114 File Offset: 0x00219314
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x0021B124 File Offset: 0x00219324
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x0021B133 File Offset: 0x00219333
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true);
		}

		// Token: 0x06006119 RID: 24857 RVA: 0x0021B14C File Offset: 0x0021934C
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true));
		}

		// Token: 0x0600611A RID: 24858 RVA: 0x0021B166 File Offset: 0x00219366
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x0021B180 File Offset: 0x00219380
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x0021B190 File Offset: 0x00219390
		public static float GearMass(Pawn p)
		{
			float num = 0f;
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					num += wornApparel[i].GetStatValue(StatDefOf.Mass, true);
				}
			}
			if (p.equipment != null)
			{
				foreach (ThingWithComps thing in p.equipment.AllEquipmentListForReading)
				{
					num += thing.GetStatValue(StatDefOf.Mass, true);
				}
			}
			return num;
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x0021B23C File Offset: 0x0021943C
		public static float InventoryMass(Pawn p)
		{
			float num = 0f;
			for (int i = 0; i < p.inventory.innerContainer.Count; i++)
			{
				Thing thing = p.inventory.innerContainer[i];
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Mass, true);
			}
			return num;
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x0021B294 File Offset: 0x00219494
		public static float Capacity(Pawn p, StringBuilder explanation = null)
		{
			if (!MassUtility.CanEverCarryAnything(p))
			{
				return 0f;
			}
			float num = p.BodySize * 35f;
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("  - " + p.LabelShortCap + ": " + num.ToStringMassOffset());
			}
			return num;
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x0021B2F2 File Offset: 0x002194F2
		public static bool CanEverCarryAnything(Pawn p)
		{
			return p.RaceProps.ToolUser || p.RaceProps.packAnimal;
		}

		// Token: 0x04003B25 RID: 15141
		public const float MassCapacityPerBodySize = 35f;
	}
}
