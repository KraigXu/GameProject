using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class Blueprint_Install : Blueprint
	{
		
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x001958EB File Offset: 0x00193AEB
		public Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				if (this.miniToInstall != null)
				{
					return this.miniToInstall;
				}
				if (this.buildingToReinstall != null)
				{
					return this.buildingToReinstall;
				}
				throw new InvalidOperationException("Nothing to install.");
			}
		}

		
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x00195915 File Offset: 0x00193B15
		public Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		
		// (get) Token: 0x06004B26 RID: 19238 RVA: 0x00195922 File Offset: 0x00193B22
		public override Graphic Graphic
		{
			get
			{
				return this.ThingToInstall.def.installBlueprintDef.graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			}
		}

		
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x00195944 File Offset: 0x00193B44
		protected override float WorkTotal
		{
			get
			{
				return 150f;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		
		public override ThingDef EntityToBuildStuff()
		{
			return this.ThingToInstall.Stuff;
		}

		
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			Log.Error("Called MaterialsNeeded on a Blueprint_Install.", false);
			return new List<ThingDefCountClass>();
		}

		
		protected override Thing MakeSolidThing()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (this.miniToInstall != null)
			{
				this.miniToInstall.InnerThing = null;
				this.miniToInstall.Destroy(DestroyMode.Vanish);
			}
			return thingToInstall;
		}

		
		public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			Map map = base.Map;
			bool flag = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
			if (flag)
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
			}
			return flag;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.ThingToInstall.def, this.ThingToInstall.Stuff);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildFacilityCommandUtility.BuildFacilityCommands(this.ThingToInstall.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.buildingToReinstall != null)
			{
				GenDraw.DrawLineBetween(this.buildingToReinstall.TrueCenter(), this.TrueCenter());
			}
			if (this.ThingToInstall.def.drawPlaceWorkersWhileInstallBlueprintSelected && this.ThingToInstall.def.PlaceWorkers != null)
			{
				List<PlaceWorker> placeWorkers = this.ThingToInstall.def.PlaceWorkers;
				for (int i = 0; i < placeWorkers.Count; i++)
				{
					placeWorkers[i].DrawGhost(this.ThingToInstall.def, base.Position, base.Rotation, Color.white, this.ThingToInstall);
				}
			}
		}

		
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		
		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.", false);
				return;
			}
			this.miniToInstall = null;
			this.buildingToReinstall = buildingToReinstall;
		}

		
		private MinifiedThing miniToInstall;

		
		private Building buildingToReinstall;
	}
}
