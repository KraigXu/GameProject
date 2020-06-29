using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_Waterskip : CompAbilityEffect
	{
		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			foreach (IntVec3 intVec in this.AffectedCells(target, map))
			{
				List<Thing> thingList = intVec.GetThingList(map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					if (thingList[i] is Fire)
					{
						thingList[i].Destroy(DestroyMode.Vanish);
					}
				}
				if (!intVec.Filled(map))
				{
					FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Water, 1, FilthSourceFlags.None);
				}
				Mote mote = MoteMaker.MakeStaticMote(intVec.ToVector3Shifted(), map, ThingDefOf.Mote_WaterskipSplashParticles, 1f);
				mote.rotationRate = Rand.Range(-30f, 30f);
				mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
				if (intVec != target.Cell)
				{
					MoteMaker.MakeStaticMote(intVec, this.parent.pawn.Map, ThingDefOf.Mote_PsycastSkipEffect, 1f);
				}
			}
		}

		
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				yield break;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, this.parent.def.EffectRadius, true))
			{
				if (intVec.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, intVec, map, true, null))
				{
					yield return intVec;
				}
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red);
		}

		
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
