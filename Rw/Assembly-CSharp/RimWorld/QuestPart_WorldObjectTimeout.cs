using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000960 RID: 2400
	public class QuestPart_WorldObjectTimeout : QuestPart_Delay
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x0012F4CE File Offset: 0x0012D6CE
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0012F4E0 File Offset: 0x0012D6E0
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.worldObject)
			{
				Site site = target as Site;
				if (site != null)
				{
					for (int i = 0; i < site.parts.Count; i++)
					{
						if (site.parts[i].def.handlesWorldObjectTimeoutInspectString)
						{
							return null;
						}
					}
				}
				return "WorldObjectTimeout".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0012F555 File Offset: 0x0012D755
		protected override void DelayFinished()
		{
			QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			if (this.worldObject != null)
			{
				base.Complete(this.worldObject.Named("SUBJECT"));
				return;
			}
			base.Complete();
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0012F587 File Offset: 0x0012D787
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0012F5A0 File Offset: 0x0012D7A0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x04002186 RID: 8582
		public WorldObject worldObject;
	}
}
