using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001019 RID: 4121
	public class StatWorker_MeleeArmorPenetration : StatWorker
	{
		// Token: 0x060062B8 RID: 25272 RVA: 0x00224640 File Offset: 0x00222840
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x060062B9 RID: 25273 RVA: 0x0022465D File Offset: 0x0022285D
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				Log.Error("Getting MeleeArmorPenetration stat for " + req.Def + " without concrete pawn. This always returns 0.", false);
			}
			return this.GetArmorPenetration(req, applyPostProcess);
		}

		// Token: 0x060062BA RID: 25274 RVA: 0x0022468C File Offset: 0x0022288C
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x060062BB RID: 25275 RVA: 0x002246A8 File Offset: 0x002228A8
		private float GetArmorPenetration(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedArmorPenetration(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2;
		}
	}
}
