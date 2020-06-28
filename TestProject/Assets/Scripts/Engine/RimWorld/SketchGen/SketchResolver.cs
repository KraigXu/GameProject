using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001085 RID: 4229
	public abstract class SketchResolver
	{
		// Token: 0x06006461 RID: 25697 RVA: 0x0022C918 File Offset: 0x0022AB18
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

		// Token: 0x06006462 RID: 25698 RVA: 0x0022C98C File Offset: 0x0022AB8C
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

		// Token: 0x06006463 RID: 25699
		protected abstract void ResolveInt(ResolveParams parms);

		// Token: 0x06006464 RID: 25700
		protected abstract bool CanResolveInt(ResolveParams parms);
	}
}
