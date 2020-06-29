using System;
using System.Collections.Generic;

namespace Verse
{
	
	public static class DebugActionsUtility
	{
		
		public static void DustPuffFrom(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.Drawer.Notify_DebugAffected();
			}
		}

		
		public static IEnumerable<float> PointsOptions(bool extended)
		{
			if (!extended)
			{
				yield return 35f;
				yield return 70f;
				yield return 100f;
				yield return 150f;
				yield return 200f;
				yield return 350f;
				yield return 500f;
				yield return 700f;
				yield return 1000f;
				yield return 1200f;
				yield return 1500f;
				yield return 2000f;
				yield return 3000f;
				yield return 4000f;
				yield return 5000f;
			}
			else
			{
				for (int i = 20; i < 100; i += 10)
				{
					yield return (float)i;
				}
				for (int i = 100; i < 500; i += 25)
				{
					yield return (float)i;
				}
				for (int i = 500; i < 1500; i += 50)
				{
					yield return (float)i;
				}
				for (int i = 1500; i <= 5000; i += 100)
				{
					yield return (float)i;
				}
			}
			yield return 6000f;
			yield return 7000f;
			yield return 8000f;
			yield return 9000f;
			yield return 10000f;
			yield break;
		}
	}
}
