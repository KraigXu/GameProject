using System;

namespace Verse
{
	
	public class SubEffecter
	{
		
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		
		public virtual void SubCleanup()
		{
		}

		
		public Effecter parent;

		
		public SubEffecterDef def;
	}
}
