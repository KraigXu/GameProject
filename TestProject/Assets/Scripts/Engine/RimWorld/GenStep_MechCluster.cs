using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class GenStep_MechCluster : GenStep
	{
		
		
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(GenStep_MechCluster.DefaultPointsRange.RandomInRange, map, false);
			IntVec3 center = IntVec3.Invalid;
			CellRect cellRect;
			if (MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				center = cellRect.ExpandedBy(20).MaxBy((IntVec3 x) => MechClusterUtility.GetClusterPositionScore(x, map, sketch));
			}
			if (!center.IsValid)
			{
				center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0f);
			}
			List<Thing> list = MechClusterUtility.SpawnCluster(center, map, sketch, false, false, null);
			List<Pawn> list2 = new List<Pawn>();
			foreach (Thing thing in list)
			{
				if (thing is Pawn)
				{
					list2.Add((Pawn)thing);
				}
			}
			if (!list2.Any<Pawn>())
			{
				return;
			}
			GenStep_SleepingMechanoids.SendMechanoidsToSleepImmediately(list2);
		}

		
		public const int ExtraRangeToRectOfInterest = 20;

		
		public static readonly FloatRange DefaultPointsRange = new FloatRange(750f, 2500f);
	}
}
