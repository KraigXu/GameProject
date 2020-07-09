using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GenerateThingSet : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<ThingSetMakerDef> thingSetMaker;

		
		public SlateRef<FloatRange?> totalMarketValueRange;

		
		public SlateRef<Thing> factionOf;

		
		public SlateRef<QualityGenerator?> qualityGenerator;
	}
}
