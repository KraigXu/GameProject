using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000238 RID: 568
	public class Hediff_ImplantWithLevel : Hediff_Implant
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0005B9AC File Offset: 0x00059BAC
		public override string Label
		{
			get
			{
				return this.def.label + " (" + "Level".Translate().ToLower() + " " + this.level + ")";
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0005BA09 File Offset: 0x00059C09
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0005BA14 File Offset: 0x00059C14
		public override void Tick()
		{
			base.Tick();
			this.Severity = (float)this.level;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0005BA29 File Offset: 0x00059C29
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0005BA56 File Offset: 0x00059C56
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0005BA6F File Offset: 0x00059C6F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
		}

		// Token: 0x04000BC9 RID: 3017
		public int level = 1;
	}
}
