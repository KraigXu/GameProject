using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C4D RID: 3149
	public class Blueprint_Install : Blueprint
	{
		// Token: 0x17000D33 RID: 3379
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

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x00195915 File Offset: 0x00193B15
		public Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004B26 RID: 19238 RVA: 0x00195922 File Offset: 0x00193B22
		public override Graphic Graphic
		{
			get
			{
				return this.ThingToInstall.def.installBlueprintDef.graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x00195944 File Offset: 0x00193B44
		protected override float WorkTotal
		{
			get
			{
				return 150f;
			}
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x0019594B File Offset: 0x00193B4B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x00195975 File Offset: 0x00193B75
		public override ThingDef EntityToBuildStuff()
		{
			return this.ThingToInstall.Stuff;
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x00195982 File Offset: 0x00193B82
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			Log.Error("Called MaterialsNeeded on a Blueprint_Install.", false);
			return new List<ThingDefCountClass>();
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x00195994 File Offset: 0x00193B94
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

		// Token: 0x06004B2C RID: 19244 RVA: 0x001959BC File Offset: 0x00193BBC
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

		// Token: 0x06004B2D RID: 19245 RVA: 0x00195A08 File Offset: 0x00193C08
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
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

		// Token: 0x06004B2E RID: 19246 RVA: 0x00195A18 File Offset: 0x00193C18
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

		// Token: 0x06004B2F RID: 19247 RVA: 0x00195ABD File Offset: 0x00193CBD
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x00195ACD File Offset: 0x00193CCD
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

		// Token: 0x04002A80 RID: 10880
		private MinifiedThing miniToInstall;

		// Token: 0x04002A81 RID: 10881
		private Building buildingToReinstall;
	}
}
