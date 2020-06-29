using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Designator_Install : Designator_Place
	{
		
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

		
		// (get) Token: 0x06005828 RID: 22568 RVA: 0x001D4330 File Offset: 0x001D2530
		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		
		// (get) Token: 0x06005829 RID: 22569 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x001D433D File Offset: 0x001D253D
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		
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

		
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x00017A00 File Offset: 0x00015C00
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		
		// (get) Token: 0x0600582E RID: 22574 RVA: 0x001D43A6 File Offset: 0x001D25A6
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.order = -10f;
		}

		
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		
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

		
		protected override bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return this.ThingToInstall != thing && !GenThing.CloserThingBetween(def, a, b, map, this.ThingToInstall);
		}

		
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
