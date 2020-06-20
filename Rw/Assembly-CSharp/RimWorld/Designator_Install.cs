using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3D RID: 3645
	public class Designator_Install : Designator_Place
	{
		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06005827 RID: 22567 RVA: 0x001D42F4 File Offset: 0x001D24F4
		private Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing is MinifiedThing)
				{
					return singleSelectedThing;
				}
				Building building = singleSelectedThing as Building;
				if (building != null && building.def.Minifiable)
				{
					return singleSelectedThing;
				}
				return null;
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06005828 RID: 22568 RVA: 0x001D4330 File Offset: 0x001D2530
		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06005829 RID: 22569 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x001D433D File Offset: 0x001D253D
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x0600582B RID: 22571 RVA: 0x001D434A File Offset: 0x001D254A
		public override string Label
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstall".Translate();
				}
				return "CommandReinstall".Translate();
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x001D4378 File Offset: 0x001D2578
		public override string Desc
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstallDesc".Translate();
				}
				return "CommandReinstallDesc".Translate();
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x00017A00 File Offset: 0x00015C00
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x0600582E RID: 22574 RVA: 0x001D43A6 File Offset: 0x001D25A6
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x001D43BC File Offset: 0x001D25BC
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.order = -10f;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x001D43EF File Offset: 0x001D25EF
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x001D43FC File Offset: 0x001D25FC
		public override void ProcessInput(Event ev)
		{
			Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(miniToInstallOrBuildingToReinstall);
				if (!((ThingDef)this.PlacingDef).rotatable)
				{
					this.placingRot = Rot4.North;
				}
			}
			base.ProcessInput(ev);
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x001D4440 File Offset: 0x001D2640
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing) && c.GetThingList(base.Map).Find((Thing x) => x.Position == c && x.Rotation == this.placingRot && x.def == this.PlacingDef) != null)
			{
				return new AcceptanceReport("IdenticalThingExists".Translate());
			}
			return GenConstruct.CanPlaceBlueprintAt(this.PlacingDef, c, this.placingRot, base.Map, false, this.MiniToInstallOrBuildingToReinstall, this.ThingToInstall, null);
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x001D44EC File Offset: 0x001D26EC
		public override void DesignateSingleCell(IntVec3 c)
		{
			GenSpawn.WipeExistingThings(c, this.placingRot, this.PlacingDef.installBlueprintDef, base.Map, DestroyMode.Deconstruct);
			MinifiedThing minifiedThing = this.MiniToInstallOrBuildingToReinstall as MinifiedThing;
			if (minifiedThing != null)
			{
				GenConstruct.PlaceBlueprintForInstall(minifiedThing, c, base.Map, this.placingRot, Faction.OfPlayer);
			}
			else
			{
				GenConstruct.PlaceBlueprintForReinstall((Building)this.MiniToInstallOrBuildingToReinstall, c, base.Map, this.placingRot, Faction.OfPlayer);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x001D4590 File Offset: 0x001D2790
		protected override void DrawGhost(Color ghostCol)
		{
			ThingDef def;
			if ((def = (this.PlacingDef as ThingDef)) != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, def, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
			}
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint, this.ThingToInstall);
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x001D4604 File Offset: 0x001D2804
		protected override bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return this.ThingToInstall != thing && !GenThing.CloserThingBetween(def, a, b, map, this.ThingToInstall);
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x001D4625 File Offset: 0x001D2825
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
