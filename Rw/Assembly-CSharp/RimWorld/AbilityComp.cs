using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC2 RID: 2754
	public abstract class AbilityComp
	{
		// Token: 0x0600416C RID: 16748 RVA: 0x0015DDB8 File Offset: 0x0015BFB8
		public virtual void Initialize(AbilityCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x0015DDC4 File Offset: 0x0015BFC4
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

		// Token: 0x0600416E RID: 16750 RVA: 0x00082085 File Offset: 0x00080285
		public virtual bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x040025E9 RID: 9705
		public Ability parent;

		// Token: 0x040025EA RID: 9706
		public AbilityCompProperties props;
	}
}
