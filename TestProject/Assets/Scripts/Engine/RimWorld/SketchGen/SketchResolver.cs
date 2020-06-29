using System;
using Verse;

namespace RimWorld.SketchGen
{
	
	public abstract class SketchResolver
	{
		
		public void Resolve(ResolveParams parms)
		{
			try
			{
				this.ResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception resolving ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nParms:\n",
					parms.ToString()
				}), false);
			}
		}

		
		public bool CanResolve(ResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nParms:\n",
					parms.ToString()
				}), false);
				result = false;
			}
			return result;
		}

		
		protected abstract void ResolveInt(ResolveParams parms);

		
		protected abstract bool CanResolveInt(ResolveParams parms);
	}
}
