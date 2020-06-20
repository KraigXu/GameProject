using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C61 RID: 3169
	public class Building_ShipReactor : Building
	{
		// Token: 0x06004BDF RID: 19423 RVA: 0x00198B5D File Offset: 0x00196D5D
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.charlonsReactor)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "ReactorDestroyed");
			}
			base.Destroy(mode);
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x00198B88 File Offset: 0x00196D88
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x00198B98 File Offset: 0x00196D98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.charlonsReactor, "charlonsReactor", false, false);
		}

		// Token: 0x04002AD4 RID: 10964
		public bool charlonsReactor;
	}
}
