using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A3A RID: 2618
	public static class FlickUtility
	{
		// Token: 0x06003DDD RID: 15837 RVA: 0x0014646C File Offset: 0x0014466C
		public static void UpdateFlickDesignation(Thing t)
		{
			bool flag = false;
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps != null)
			{
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						flag = true;
						break;
					}
				}
			}
			Designation designation = t.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick);
			if (flag && designation == null)
			{
				t.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Flick));
			}
			else if (!flag && designation != null)
			{
				designation.Delete();
			}
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.SwitchFlickingDesignation, Array.Empty<string>());
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0014651C File Offset: 0x0014471C
		public static bool WantsToBeOn(Thing t)
		{
			CompFlickable compFlickable = t.TryGetComp<CompFlickable>();
			if (compFlickable != null && !compFlickable.SwitchIsOn)
			{
				return false;
			}
			CompSchedule compSchedule = t.TryGetComp<CompSchedule>();
			return compSchedule == null || compSchedule.Allowed;
		}
	}
}
