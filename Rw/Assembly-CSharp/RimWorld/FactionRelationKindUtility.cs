using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BED RID: 3053
	public static class FactionRelationKindUtility
	{
		// Token: 0x060048A0 RID: 18592 RVA: 0x0018B1EC File Offset: 0x001893EC
		public static string GetLabel(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate();
			case FactionRelationKind.Neutral:
				return "Neutral".Translate();
			case FactionRelationKind.Ally:
				return "Ally".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x0018B242 File Offset: 0x00189442
		public static Color GetColor(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return ColoredText.RedReadable;
			case FactionRelationKind.Neutral:
				return new Color(0f, 0.75f, 1f);
			case FactionRelationKind.Ally:
				return Color.green;
			default:
				return Color.white;
			}
		}
	}
}
