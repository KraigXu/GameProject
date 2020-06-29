﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_Wallraise : CompAbilityEffect
	{
		
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x0015F307 File Offset: 0x0015D507
		public new CompProperties_AbilityWallraise Props
		{
			get
			{
				return (CompProperties_AbilityWallraise)this.props;
			}
		}

		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			List<Thing> list = new List<Thing>();
			list.AddRange(this.AffectedCells(target, map).SelectMany((IntVec3 c) => from t in c.GetThingList(map)
			where t.def.category == ThingCategory.Item
			select t));
			foreach (Thing thing in list)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			foreach (IntVec3 intVec in this.AffectedCells(target, map))
			{
				GenSpawn.Spawn(ThingDefOf.RaisedRocks, intVec, map, WipeMode.Vanish);
				MoteMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Wallraise.DustColor);
				if (intVec != target.Cell)
				{
					MoteMaker.MakeStaticMote(intVec, this.parent.pawn.Map, ThingDefOf.Mote_PsycastSkipEffect, 1f);
				}
			}
			foreach (Thing thing2 in list)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec3 = thing2.Position + GenRadial.RadialPattern[i];
					if (intVec3.InBounds(map) && intVec3.Walkable(map) && map.thingGrid.ThingsListAtFast(intVec3).Count <= 0)
					{
						intVec2 = intVec3;
						break;
					}
				}
				if (intVec2 != IntVec3.Invalid)
				{
					GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
				}
				else
				{
					GenPlace.TryPlaceThing(thing2, thing2.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, true);
		}

		
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red);
		}

		
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			foreach (IntVec2 intVec in this.Props.pattern)
			{
				IntVec3 intVec2 = target.Cell + new IntVec3(intVec.x, 0, intVec.z);
				if (intVec2.InBounds(map))
				{
					yield return intVec2;
				}
			}
			List<IntVec2>.Enumerator enumerator = default(List<IntVec2>.Enumerator);
			yield break;
			yield break;
		}

		
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => c.Filled(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => !c.Standable(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityUnwalkable".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 4f);
	}
}
