using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001199 RID: 4505
	public abstract class QuestNode
	{
		// Token: 0x06006850 RID: 26704 RVA: 0x00247044 File Offset: 0x00245244
		public QuestNode()
		{
			this.myTypeShort = base.GetType().Name;
			if (this.myTypeShort.StartsWith("QuestNode_"))
			{
				this.myTypeShort = this.myTypeShort.Substring("QuestNode_".Length);
			}
		}

		// Token: 0x06006851 RID: 26705 RVA: 0x00247098 File Offset: 0x00245298
		public float SelectionWeight(Slate slate)
		{
			float? value = this.selectionWeight.GetValue(slate);
			if (value == null)
			{
				return 1f;
			}
			return value.GetValueOrDefault();
		}

		// Token: 0x06006852 RID: 26706 RVA: 0x002470C8 File Offset: 0x002452C8
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

		// Token: 0x06006853 RID: 26707 RVA: 0x0024715C File Offset: 0x0024535C
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

		// Token: 0x06006854 RID: 26708
		protected abstract void RunInt();

		// Token: 0x06006855 RID: 26709
		protected abstract bool TestRunInt(Slate slate);

		// Token: 0x06006856 RID: 26710 RVA: 0x002471E8 File Offset: 0x002453E8
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x040040A5 RID: 16549
		[Unsaved(false)]
		[TranslationHandle]
		public string myTypeShort;

		// Token: 0x040040A6 RID: 16550
		public SlateRef<float?> selectionWeight;
	}
}
