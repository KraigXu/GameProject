using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SetAndRestore : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo(this.name.GetValue(slate));
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				slate.Restore(restoreInfo);
			}
			return result;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo(this.name.GetValue(slate));
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			try
			{
				this.node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> name;

		
		public SlateRef<object> value;

		
		public QuestNode node;
	}
}
