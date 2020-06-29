using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		
		
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		
		
		public TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetLocations = true
				};
			}
		}

		
		
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		
		
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		
		
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		
		
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		
		
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		
		
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		
		
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		
		
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		
		
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		
		public LocalTargetInfo GetDestination(LocalTargetInfo target)
		{
			Map map = this.parent.pawn.Map;
			switch (this.Props.destination)
			{
			case AbilityEffectDestination.Caster:
				return new LocalTargetInfo(this.parent.pawn.InteractionCell);
			case AbilityEffectDestination.RandomInRange:
			{
				this.cells.Clear();
				int num = GenRadial.NumCellsInRadius(this.Props.randomRange.max);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = GenRadial.RadialPattern[i];
					if (intVec.DistanceTo(IntVec3.Zero) >= this.Props.randomRange.min)
					{
						IntVec3 intVec2 = target.Cell + intVec;
						if (intVec2.Standable(map) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(target.Cell, intVec2, map, false, null, 0, 0)))
						{
							this.cells.Add(intVec2);
						}
					}
				}
				if (this.cells.Any<IntVec3>())
				{
					return new LocalTargetInfo(this.cells.RandomElement<IntVec3>());
				}
				Messages.Message("NoValidDestinationFound".Translate(this.parent.def.LabelCap), MessageTypeDefOf.RejectInput, true);
				return LocalTargetInfo.Invalid;
			}
			case AbilityEffectDestination.Selected:
				return target;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		
		protected bool CanPlaceSelectedTargetAt(LocalTargetInfo target)
		{
			if (this.selectedTarget.Pawn != null)
			{
				return !target.Cell.Impassable(this.parent.pawn.Map) && target.Cell.Walkable(this.parent.pawn.Map);
			}
			Building edifice = target.Cell.GetEdifice(this.parent.pawn.Map);
			Building_Door building_Door;
			if (edifice != null && edifice.def.surfaceType != SurfaceType.Item && edifice.def.surfaceType != SurfaceType.Eat && ((building_Door = (edifice as Building_Door)) == null || !building_Door.Open))
			{
				return false;
			}
			List<Thing> thingList = target.Cell.GetThingList(this.parent.pawn.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Item)
				{
					return false;
				}
			}
			return true;
		}

		
		public virtual bool CanHitTarget(LocalTargetInfo target)
		{
			return target.IsValid && target.Cell.DistanceTo(target.Cell) <= this.Props.range && this.CanPlaceSelectedTargetAt(target) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(this.selectedTarget.Cell, target.Cell, this.parent.pawn.Map, false, null, 0, 0));
		}

		
		public bool ValidateTarget(LocalTargetInfo target)
		{
			return this.CanHitTarget(target);
		}

		
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (this.Props.requiresLineOfSight)
			{
				GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.selectedTarget.Cell, c, this.parent.pawn.Map, false, null, 0, 0) && this.CanPlaceSelectedTargetAt(c));
			}
			else
			{
				GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range);
			}
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		
		public void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				icon = this.parent.def.uiIcon;
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.parent.QueueCastingJob(this.selectedTarget, target);
		}

		
		public void SetTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target;
		}

		
		public virtual void SelectDestination()
		{
			Find.Targeter.BeginTargeting(this, null);
		}

		
		protected LocalTargetInfo selectedTarget;

		
		private List<IntVec3> cells = new List<IntVec3>();
	}
}
