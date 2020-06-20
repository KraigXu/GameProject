using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200111A RID: 4378
	public class QuestNode_GenerateThingSet : QuestNode
	{
		// Token: 0x0600667F RID: 26239 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x0023E738 File Offset: 0x0023C938
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = this.totalMarketValueRange.GetValue(slate);
			Thing value = this.factionOf.GetValue(slate);
			parms.makingFaction = ((value == null) ? null : value.Faction);
			parms.qualityGenerator = this.qualityGenerator.GetValue(slate);
			List<Thing> list = this.thingSetMaker.GetValue(slate).root.Generate(parms);
			QuestGen.slate.Set<List<Thing>>(this.storeAs.GetValue(slate), list, false);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
		}

		// Token: 0x04003EA7 RID: 16039
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EA8 RID: 16040
		public SlateRef<ThingSetMakerDef> thingSetMaker;

		// Token: 0x04003EA9 RID: 16041
		public SlateRef<FloatRange?> totalMarketValueRange;

		// Token: 0x04003EAA RID: 16042
		public SlateRef<Thing> factionOf;

		// Token: 0x04003EAB RID: 16043
		public SlateRef<QualityGenerator?> qualityGenerator;
	}
}
