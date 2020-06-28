using System;
using System.Reflection;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001126 RID: 4390
	public class QuestNode_GetFieldValue : QuestNode
	{
		// Token: 0x060066AF RID: 26287 RVA: 0x0023F1F6 File Offset: 0x0023D3F6
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x0023F200 File Offset: 0x0023D400
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060066B1 RID: 26289 RVA: 0x0023F210 File Offset: 0x0023D410
		private void SetVars(Slate slate)
		{
			object obj = (this.type.GetValue(slate) != null) ? ConvertHelper.Convert(this.obj.GetValue(slate), this.type.GetValue(slate)) : this.obj.GetValue(slate);
			FieldInfo fieldInfo = obj.GetType().GetField(this.field.GetValue(slate), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fieldInfo == null)
			{
				Log.Error("QuestNode error: " + obj.GetType().Name + " doesn't have a field named " + this.field.GetValue(slate), false);
				return;
			}
			slate.Set<object>(this.storeAs.GetValue(slate), fieldInfo.GetValue(obj), false);
		}

		// Token: 0x04003ED8 RID: 16088
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003ED9 RID: 16089
		[NoTranslate]
		public SlateRef<object> obj;

		// Token: 0x04003EDA RID: 16090
		[NoTranslate]
		public SlateRef<string> field;

		// Token: 0x04003EDB RID: 16091
		public SlateRef<Type> type;
	}
}
