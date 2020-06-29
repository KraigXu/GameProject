using System;
using Verse;

namespace RimWorld.SketchGen
{
	
	public static class SketchGenCore
	{
		
		public static Sketch Generate(SketchResolverDef root, ResolveParams parms)
		{
			if (SketchGenCore.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
				return parms.sketch;
			}
			SketchGenCore.working = true;
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
				SketchGenCore.working = false;
			}
			return sketch;
		}

		
		private static bool working;
	}
}
