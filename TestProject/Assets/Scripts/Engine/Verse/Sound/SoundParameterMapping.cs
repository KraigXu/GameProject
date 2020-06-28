using System;

namespace Verse.Sound
{
	// Token: 0x020004DB RID: 1243
	public class SoundParameterMapping
	{
		// Token: 0x0600244B RID: 9291 RVA: 0x000D8CBC File Offset: 0x000D6EBC
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x000D8D10 File Offset: 0x000D6F10
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			string title = ((this.inParam != null) ? this.inParam.Label : "null") + " -> " + ((this.outParam != null) ? this.outParam.Label : "null");
			if (widgetRow.ButtonText("Edit curve", "Edit the curve mapping the in parameter to the out parameter.", true, true))
			{
				Find.WindowStack.Add(new EditWindow_CurveEditor(this.curve, title));
			}
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000D8D88 File Offset: 0x000D6F88
		public void Apply(Sample samp)
		{
			if (this.inParam == null || this.outParam == null)
			{
				return;
			}
			float x = this.inParam.ValueFor(samp);
			float value = this.curve.Evaluate(x);
			this.outParam.SetOn(samp, value);
		}

		// Token: 0x040015F5 RID: 5621
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam;

		// Token: 0x040015F6 RID: 5622
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam;

		// Token: 0x040015F7 RID: 5623
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode;

		// Token: 0x040015F8 RID: 5624
		[EditorHidden]
		public SimpleCurve curve;
	}
}
