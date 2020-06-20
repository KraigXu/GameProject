using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E27 RID: 3623
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x0600578E RID: 22414 RVA: 0x001D163B File Offset: 0x001CF83B
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x000255BF File Offset: 0x000237BF
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x000255BF File Offset: 0x000237BF
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
