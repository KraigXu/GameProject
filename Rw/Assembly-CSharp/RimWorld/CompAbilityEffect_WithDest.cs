using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE1 RID: 2785
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x0015F918 File Offset: 0x0015DB18
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		// Token: 0x17000BAC RID: 2988
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

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x00010306 File Offset: 0x0000E506
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x0015F933 File Offset: 0x0015DB33
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x0015F933 File Offset: 0x0015DB33
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x00019EA1 File Offset: 0x000180A1
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x0015F940 File Offset: 0x0015DB40
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x00019EA1 File Offset: 0x000180A1
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x0015F948 File Offset: 0x0015DB48
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

		// Token: 0x060041CF RID: 16847 RVA: 0x0015FA9C File Offset: 0x0015DC9C
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

		// Token: 0x060041D0 RID: 16848 RVA: 0x0015FB88 File Offset: 0x0015DD88
		public virtual bool CanHitTarget(LocalTargetInfo target)
		{
			return target.IsValid && target.Cell.DistanceTo(target.Cell) <= this.Props.range && this.CanPlaceSelectedTargetAt(target) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(this.selectedTarget.Cell, target.Cell, this.parent.pawn.Map, false, null, 0, 0));
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x0015FC0A File Offset: 0x0015DE0A
		public bool ValidateTarget(LocalTargetInfo target)
		{
			return this.CanHitTarget(target);
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0015FC14 File Offset: 0x0015DE14
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

		// Token: 0x060041D3 RID: 16851 RVA: 0x0015FC88 File Offset: 0x0015DE88
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

		// Token: 0x060041D4 RID: 16852 RVA: 0x0015FCBD File Offset: 0x0015DEBD
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.parent.QueueCastingJob(this.selectedTarget, target);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x0015FCD1 File Offset: 0x0015DED1
		public void SetTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x0015FCDA File Offset: 0x0015DEDA
		public virtual void SelectDestination()
		{
			Find.Targeter.BeginTargeting(this, null);
		}

		// Token: 0x04002616 RID: 9750
		protected LocalTargetInfo selectedTarget;

		// Token: 0x04002617 RID: 9751
		private List<IntVec3> cells = new List<IntVec3>();
	}
}
