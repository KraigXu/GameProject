using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001110 RID: 4368
	public class QuestNode_SubScript : QuestNode
	{
		// Token: 0x06006660 RID: 26208 RVA: 0x0023DC18 File Offset: 0x0023BE18
		protected override bool TestRunInt(Slate slate)
		{
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, slate);
			if (!value.NullOrEmpty())
			{
				slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			bool result;
			try
			{
				result = this.def.GetValue(slate).root.TestRun(slate);
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, slate);
			}
			return result;
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x0023DCAC File Offset: 0x0023BEAC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, QuestGen.slate);
			if (!value.NullOrEmpty())
			{
				QuestGen.slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			try
			{
				this.def.GetValue(slate).Run();
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					QuestGen.slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, QuestGen.slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, QuestGen.slate);
			}
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x0023DD50 File Offset: 0x0023BF50
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				" (",
				this.def,
				")"
			});
		}

		// Token: 0x04003E7A RID: 15994
		[TranslationHandle(Priority = 100)]
		public SlateRef<QuestScriptDef> def;

		// Token: 0x04003E7B RID: 15995
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x04003E7C RID: 15996
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x04003E7D RID: 15997
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x04003E7E RID: 15998
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();
	}
}
