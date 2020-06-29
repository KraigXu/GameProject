using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		
		
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		
		
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		
		
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		
		
		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				return mapParent.HasMap && (!this.Reform || (!GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true) && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0));
			}
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (!this.Reform)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate(),
						icon = FormCaravanComp.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "FormCaravan",
						action = delegate
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, false));
						}
					};
				}
				else if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "CommandReformCaravan".Translate();
					command_Action.defaultDesc = "CommandReformCaravanDesc".Translate();
					command_Action.icon = FormCaravanComp.FormCaravanCommand;
					command_Action.hotKey = KeyBindingDefOf.Misc2;
					command_Action.tutorTag = "ReformCaravan";
					command_Action.action = delegate
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, false));
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true))
					{
						command_Action.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return command_Action;
				}
				if (Prefs.DevMode)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Show available exits",
						action = delegate
						{
							foreach (int tile in CaravanExitMapUtility.AvailableExitTilesAt(mapParent.Map))
							{
								Find.WorldDebugDrawer.FlashTile(tile, 0f, null, 10);
							}
						}
					};
				}
			}
			yield break;
		}

		
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);
	}
}
