using System;
using UnityEngine;

namespace Verse
{
	
	public class Hediff_ImplantWithLevel : Hediff_Implant
	{
		
		
		public override string Label
		{
			get
			{
				return this.def.label + " (" + "Level".Translate().ToLower() + " " + this.level + ")";
			}
		}

		
		
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
