using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001255 RID: 4693
	public class RoutePlannerWaypoint : WorldObject
	{
		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06006D90 RID: 28048 RVA: 0x00265224 File Offset: 0x00263424
		public override string Label
		{
			get
			{
				WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
				if (worldRoutePlanner.Active)
				{
					int num = worldRoutePlanner.waypoints.IndexOf(this);
					if (num >= 0)
					{
						return base.Label + " " + (num + 1);
					}
				}
				return base.Label;
			}
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x00265270 File Offset: 0x00263470
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			if (worldRoutePlanner.Active)
			{
				int num = worldRoutePlanner.waypoints.IndexOf(this);
				if (num >= 1)
				{
					int ticksToWaypoint = worldRoutePlanner.GetTicksToWaypoint(num);
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("EstimatedTimeToWaypoint".Translate(ticksToWaypoint.ToStringTicksToDays("0.#")));
					if (num >= 2)
					{
						int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
						stringBuilder.AppendLine();
						stringBuilder.Append("EstimatedTimeToWaypointFromPrevious".Translate((ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x00265334 File Offset: 0x00263534
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandRemoveWaypointLabel".Translate(),
				defaultDesc = "CommandRemoveWaypointDesc".Translate(),
				icon = TexCommand.RemoveRoutePlannerWaypoint,
				action = delegate
				{
					Find.WorldRoutePlanner.TryRemoveWaypoint(this, true);
				}
			};
			yield break;
			yield break;
		}
	}
}
