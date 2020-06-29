using System;
using UnityEngine;

namespace Verse
{
	
	public class Hediff_ImplantWithLevel : Hediff_Implant
	{
		
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0005B9AC File Offset: 0x00059BAC
		public override string Label
		{
			get
			{
				return this.def.label + " (" + "Level".Translate().ToLower() + " " + this.level + ")";
			}
		}

		
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0005BA09 File Offset: 0x00059C09
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		
		public override void Tick()
		{
			base.Tick();
			this.Severity = (float)this.level;
		}

		
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
		}

		
		public int level = 1;
	}
}
