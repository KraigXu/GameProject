using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SpawnMechCluster : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_MechCluster questPart_MechCluster = new QuestPart_MechCluster();
			questPart_MechCluster.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_MechCluster.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_MechCluster.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_MechCluster.sketch = this.GenerateSketch(slate);
			QuestGen.quest.AddPart(questPart_MechCluster);
			string text = "";
			if (questPart_MechCluster.sketch.pawns != null)
			{
				text += PawnUtility.PawnKindsToLineList(from m in questPart_MechCluster.sketch.pawns
				select m.kindDef, "  - ");
			}
			string[] array = (from t in questPart_MechCluster.sketch.buildingsSketch.Things
			where GenHostility.IsDefMechClusterThreat(t.def)
			group t by t.def.label).Select(delegate(IGrouping<string, SketchThing> grp)
			{
				int num = grp.Count<SketchThing>();
				return num + " " + ((num > 1) ? Find.ActiveLanguageWorker.Pluralize(grp.Key, num) : grp.Key);
			}).ToArray<string>();
			if (array.Any<string>())
			{
				if (text != "")
				{
					text += "\n";
				}
				text += array.ToLineList("  - ");
			}
			if (text != "")
			{
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allThreats", text)
				});
			}
		}

		
		private MechClusterSketch GenerateSketch(Slate slate)
		{
			return MechClusterGenerator.GenerateClusterSketch(this.points.GetValue(slate) ?? slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), true);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		[NoTranslate]
		public SlateRef<string> tag;

		
		public SlateRef<float?> points;
	}
}
