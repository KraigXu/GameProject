using System;
using Verse;

namespace RimWorld.SketchGen
{
	
	public static class SketchGen
	{
		
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

		
		private static bool working;
	}
}
