using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		
		// (get) Token: 0x06006EE5 RID: 28389 RVA: 0x0026A889 File Offset: 0x00268A89
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		
		// (get) Token: 0x06006EE6 RID: 28390 RVA: 0x0026A896 File Offset: 0x00268A96
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		
		// (get) Token: 0x06006EE7 RID: 28391 RVA: 0x0026A8A3 File Offset: 0x00268AA3
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		
		// (get) Token: 0x06006EE8 RID: 28392 RVA: 0x0026A8C8 File Offset: 0x00268AC8
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
