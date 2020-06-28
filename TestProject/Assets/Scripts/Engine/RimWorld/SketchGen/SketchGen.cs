using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001083 RID: 4227
	public static class SketchGen
	{
		// Token: 0x06006459 RID: 25689 RVA: 0x0022C35C File Offset: 0x0022A55C
		public static Sketch Generate(SketchResolverDef root, ResolveParams parms)
		{
			if (SketchGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
				return parms.sketch;
			}
			SketchGen.working = true;
			Sketch sketch;
			try
			{
				root.Resolve(parms);
				sketch = parms.sketch;
			}
			catch (Exception arg)
			{
				Log.Error("Error in SketchGen: " + arg, false);
				sketch = parms.sketch;
			}
			finally
			{
				SketchGen.working = false;
			}
			return sketch;
		}

		// Token: 0x04003D1E RID: 15646
		private static bool working;
	}
}
