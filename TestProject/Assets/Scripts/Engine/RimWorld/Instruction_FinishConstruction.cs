using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1C RID: 3868
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x0020BD9C File Offset: 0x00209F9C
		protected override float ProgressPercent
		{
			get
			{
				if (this.initialBlueprintsCount < 0)
				{
					this.initialBlueprintsCount = this.ConstructionNeeders().Count<Thing>();
				}
				if (this.initialBlueprintsCount == 0)
				{
					return 1f;
				}
				return 1f - (float)this.ConstructionNeeders().Count<Thing>() / (float)this.initialBlueprintsCount;
			}
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x0020BDEC File Offset: 0x00209FEC
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x0020BE48 File Offset: 0x0020A048
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (this.ConstructionNeeders().Count<Thing>() < 3)
			{
				foreach (Thing thing in this.ConstructionNeeders())
				{
					GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
				}
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400336D RID: 13165
		private int initialBlueprintsCount = -1;
	}
}
