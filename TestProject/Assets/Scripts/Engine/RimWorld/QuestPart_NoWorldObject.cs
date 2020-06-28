using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095B RID: 2395
	public class QuestPart_NoWorldObject : QuestPartActivable
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060038AE RID: 14510 RVA: 0x0012EFAA File Offset: 0x0012D1AA
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

		// Token: 0x060038AF RID: 14511 RVA: 0x0012EFBA File Offset: 0x0012D1BA
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.worldObject == null || !this.worldObject.Spawned)
			{
				base.Complete();
			}
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x0012EFDD File Offset: 0x0012D1DD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x0012EFF8 File Offset: 0x0012D1F8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			Site site = Find.WorldObjects.Sites.FirstOrDefault<Site>();
			if (site != null)
			{
				this.worldObject = site;
				return;
			}
			Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			if (randomPlayerHomeMap != null)
			{
				this.worldObject = randomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x0400217B RID: 8571
		public WorldObject worldObject;
	}
}
