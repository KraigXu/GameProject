using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompForbiddable : ThingComp
	{
		
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x001B41C1 File Offset: 0x001B23C1
		// (set) Token: 0x06005143 RID: 20803 RVA: 0x001B41CC File Offset: 0x001B23CC
		public bool Forbidden
		{
			get
			{
				return this.forbiddenInt;
			}
			set
			{
				if (value == this.forbiddenInt)
				{
					return;
				}
				this.forbiddenInt = value;
				if (this.parent.Spawned)
				{
					if (this.forbiddenInt)
					{
						this.parent.Map.listerHaulables.Notify_Forbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Forbidden(this.parent);
					}
					else
					{
						this.parent.Map.listerHaulables.Notify_Unforbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Unforbidden(this.parent);
					}
					if (this.parent is Building_Door)
					{
						this.parent.Map.reachability.ClearCache();
					}
				}
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		
		public override void PostDraw()
		{
			if (this.forbiddenInt)
			{
				if (this.parent is Blueprint || this.parent is Frame)
				{
					if (this.parent.def.size.x > 1 || this.parent.def.size.z > 1)
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenBig);
						return;
					}
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.Forbidden);
					return;
				}
				else
				{
					if (this.parent.def.category == ThingCategory.Building)
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenBig);
						return;
					}
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.Forbidden);
				}
			}
		}

		
		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent is Building && this.parent.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
			command_Toggle.icon = TexCommand.ForbidOff;
			command_Toggle.isActive = (() => !this.Forbidden);
			command_Toggle.defaultLabel = "CommandAllow".TranslateWithBackup("DesignatorUnforbid");
			command_Toggle.activateIfAmbiguous = false;
			if (this.forbiddenInt)
			{
				command_Toggle.defaultDesc = "CommandForbiddenDesc".TranslateWithBackup("DesignatorUnforbidDesc");
			}
			else
			{
				command_Toggle.defaultDesc = "CommandNotForbiddenDesc".TranslateWithBackup("DesignatorForbidDesc");
			}
			if (this.parent.def.IsDoor)
			{
				command_Toggle.tutorTag = "ToggleForbidden-Door";
				command_Toggle.toggleAction = delegate
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ForbiddingDoors, KnowledgeAmount.SpecificInteraction);
				};
			}
			else
			{
				command_Toggle.tutorTag = "ToggleForbidden";
				command_Toggle.toggleAction = delegate
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
				};
			}
			yield return command_Toggle;
			yield break;
		}

		
		private bool forbiddenInt;
	}
}
