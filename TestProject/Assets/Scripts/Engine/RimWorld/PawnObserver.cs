using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9F RID: 2975
	public class PawnObserver
	{
		// Token: 0x060045B2 RID: 17842 RVA: 0x00178BB8 File Offset: 0x00176DB8
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = Rand.Range(0, 4);
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x00178BD4 File Offset: 0x00176DD4
		public void ObserverInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.intervalsUntilObserve--;
			if (this.intervalsUntilObserve <= 0)
			{
				this.ObserveSurroundingThings();
				this.intervalsUntilObserve = 4 + Rand.RangeInclusive(-1, 1);
			}
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00178C10 File Offset: 0x00176E10
		private void ObserveSurroundingThings()
		{
			if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) || this.pawn.needs.mood == null)
			{
				return;
			}
			Map map = this.pawn.Map;
			int num = 0;
			while ((float)num < 100f)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(map) && GenSight.LineOfSight(intVec, this.pawn.Position, map, true, null, 0, 0))
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						IThoughtGiver thoughtGiver = thingList[i] as IThoughtGiver;
						if (thoughtGiver != null)
						{
							Thought_Memory thought_Memory = thoughtGiver.GiveObservedThought();
							if (thought_Memory != null)
							{
								this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
							}
						}
					}
				}
				num++;
			}
		}

		// Token: 0x0400281B RID: 10267
		private Pawn pawn;

		// Token: 0x0400281C RID: 10268
		private int intervalsUntilObserve;

		// Token: 0x0400281D RID: 10269
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x0400281E RID: 10270
		private const float SampleNumCells = 100f;
	}
}
