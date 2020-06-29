using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x0015F918 File Offset: 0x0015DB18
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x0015F925 File Offset: 0x0015DB25
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

		
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x00010306 File Offset: 0x0000E506
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x0015F933 File Offset: 0x0015DB33
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x0015F933 File Offset: 0x0015DB33
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x00019EA1 File Offset: 0x000180A1
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x0015F940 File Offset: 0x0015DB40
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x00019EA1 File Offset: 0x000180A1
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
