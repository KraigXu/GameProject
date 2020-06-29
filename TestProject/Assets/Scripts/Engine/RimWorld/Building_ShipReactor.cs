using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Building_ShipReactor : Building
	{
		
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.charlonsReactor)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "ReactorDestroyed");
			}
			base.Destroy(mode);
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.charlonsReactor, "charlonsReactor", false, false);
		}

		
		public bool charlonsReactor;
	}
}
