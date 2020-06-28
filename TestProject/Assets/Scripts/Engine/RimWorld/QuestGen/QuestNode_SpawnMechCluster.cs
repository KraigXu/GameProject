using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001192 RID: 4498
	public class QuestNode_SpawnMechCluster : QuestNode
	{
		// Token: 0x0600683B RID: 26683 RVA: 0x00246990 File Offset: 0x00244B90
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

		// Token: 0x0600683C RID: 26684 RVA: 0x00246B4C File Offset: 0x00244D4C
		private MechClusterSketch GenerateSketch(Slate slate)
		{
			return MechClusterGenerator.GenerateClusterSketch(this.points.GetValue(slate) ?? slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), true);
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x00246B9C File Offset: 0x00244D9C
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x04004089 RID: 16521
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400408A RID: 16522
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400408B RID: 16523
		public SlateRef<float?> points;
	}
}
