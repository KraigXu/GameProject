﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		
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

		
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		
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

		
		private int initialBlueprintsCount = -1;
	}
}
