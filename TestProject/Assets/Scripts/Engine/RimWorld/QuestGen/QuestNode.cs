using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public abstract class QuestNode
	{
		
		public QuestNode()
		{
			this.myTypeShort = base.GetType().Name;
			if (this.myTypeShort.StartsWith("QuestNode_"))
			{
				this.myTypeShort = this.myTypeShort.Substring("QuestNode_".Length);
			}
		}

		
		public float SelectionWeight(Slate slate)
		{
			float? value = this.selectionWeight.GetValue(slate);
			if (value == null)
			{
				return 1f;
			}
			return value.GetValueOrDefault();
		}

		
		public void Run()
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(this.ToString());
			}
			try
			{
				this.RunInt();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nSlate vars:\n",
					QuestGen.slate.ToString()
				}), false);
			}
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
		}

		
		public bool TestRun(Slate slate)
		{
			bool result;
			try
			{
				Action<QuestNode, Slate> action;
				if (slate.TryGet<Action<QuestNode, Slate>>("testRunCallback", out action, false) && action != null)
				{
					action(this, slate);
				}
				result = this.TestRunInt(slate);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nSlate vars:\n",
					slate.ToString()
				}), false);
				result = false;
			}
			return result;
		}

		
		protected abstract void RunInt();

		
		protected abstract bool TestRunInt(Slate slate);

		
		public override string ToString()
		{
			return base.GetType().Name;
		}

		
		[Unsaved(false)]
		[TranslationHandle]
		public string myTypeShort;

		
		public SlateRef<float?> selectionWeight;
	}
}
