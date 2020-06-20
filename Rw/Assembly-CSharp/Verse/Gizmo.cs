using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000394 RID: 916
	public abstract class Gizmo
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x000A58C5 File Offset: 0x000A3AC5
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06001AF7 RID: 6903
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x06001AF8 RID: 6904 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06001AF9 RID: 6905
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06001AFA RID: 6906 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000A58CE File Offset: 0x000A3ACE
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000A58D6 File Offset: 0x000A3AD6
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x000A58DF File Offset: 0x000A3ADF
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x04000FF8 RID: 4088
		public bool disabled;

		// Token: 0x04000FF9 RID: 4089
		public string disabledReason;

		// Token: 0x04000FFA RID: 4090
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x04000FFB RID: 4091
		public float order;

		// Token: 0x04000FFC RID: 4092
		public const float Height = 75f;
	}
}
