using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_NoWorldObject : QuestPartActivable
	{
		
		// (get) Token: 0x060038AE RID: 14510 RVA: 0x0012EFAA File Offset: 0x0012D1AA
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.worldObject == null || !this.worldObject.Spawned)
			{
				base.Complete();
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		
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

		
		public WorldObject worldObject;
	}
}
