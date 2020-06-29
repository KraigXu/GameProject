using System;
using Verse;

namespace RimWorld
{
	
	public abstract class AbilityComp
	{
		
		public virtual void Initialize(AbilityCompProperties props)
		{
			this.props = props;
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.pawn.Position : IntVec3.Invalid,
				")"
			});
		}

		
		public virtual bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		
		public Ability parent;

		
		public AbilityCompProperties props;
	}
}
