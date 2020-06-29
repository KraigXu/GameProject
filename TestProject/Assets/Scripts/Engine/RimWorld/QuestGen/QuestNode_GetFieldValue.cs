using System;
using System.Reflection;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetFieldValue : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		[NoTranslate]
		public SlateRef<object> obj;

		
		[NoTranslate]
		public SlateRef<string> field;

		
		public SlateRef<Type> type;
	}
}
