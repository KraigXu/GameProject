using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F17 RID: 3863
	public class Instruction_DownRaider : Lesson_Instruction
	{
		// Token: 0x06005E9E RID: 24222 RVA: 0x0020B8B6 File Offset: 0x00209AB6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.coverCells, "coverCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06005E9F RID: 24223 RVA: 0x0020B8D4 File Offset: 0x00209AD4
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = Find.TutorialState.sandbagsRect.ContractedBy(1);
			this.coverCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect.EdgeCells)
			{
				if (intVec.x != cellRect.CenterCell.x && intVec.z != cellRect.CenterCell.z)
				{
					this.coverCells.Add(intVec);
				}
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = base.Map;
			incidentParms.points = PawnKindDefOf.Drifter.combatPower;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			incidentParms.raidForceOneIncap = true;
			incidentParms.raidNeverFleeIndividual = true;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}

		// Token: 0x06005EA0 RID: 24224 RVA: 0x0020B9C8 File Offset: 0x00209BC8
		private bool AllColonistsInCover()
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				if (!this.coverCells.Contains(pawn.Position))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005EA1 RID: 24225 RVA: 0x0020BA38 File Offset: 0x00209C38
		public override void LessonOnGUI()
		{
			if (!this.AllColonistsInCover())
			{
				TutorUtility.DrawCellRectOnGUI(Find.TutorialState.sandbagsRect, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06005EA2 RID: 24226 RVA: 0x0020BA64 File Offset: 0x00209C64
		public override void LessonUpdate()
		{
			if (!this.AllColonistsInCover())
			{
				for (int i = 0; i < this.coverCells.Count; i++)
				{
					Vector3 position = this.coverCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
					Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
				}
			}
			if (base.Map.mapPawns.PawnsInFaction(Faction.OfPlayer).Any((Pawn p) => p.Downed))
			{
				foreach (Pawn pawn in base.Map.mapPawns.AllPawns)
				{
					if (pawn.HostileTo(Faction.OfPlayer))
					{
						HealthUtility.DamageUntilDowned(pawn, true);
					}
				}
			}
			if ((from p in base.Map.mapPawns.AllPawnsSpawned
			where p.HostileTo(Faction.OfPlayer) && !p.Downed
			select p).Count<Pawn>() == 0)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400336B RID: 13163
		private List<IntVec3> coverCells;
	}
}
