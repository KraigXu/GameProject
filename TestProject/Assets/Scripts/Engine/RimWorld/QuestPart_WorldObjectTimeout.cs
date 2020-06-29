using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_WorldObjectTimeout : QuestPart_Delay
	{
		
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x0012F4CE File Offset: 0x0012D6CE
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		
		public WorldObject worldObject;
	}
}
