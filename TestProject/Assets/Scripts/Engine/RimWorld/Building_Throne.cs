using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Building_Throne : Building
	{
		
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x0019D54C File Offset: 0x0019B74C
		public static IEnumerable<RoyalTitleDef> AllTitlesForThroneStature
		{
			get
			{
				return from title in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where title.MinThroneRoomImpressiveness > 0f
				orderby title.MinThroneRoomImpressiveness
				select title;
			}
		}

		
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x0019D5A8 File Offset: 0x0019B7A8
		public Pawn AssignedPawn
		{
			get
			{
				if (!ModLister.RoyaltyInstalled)
				{
					Log.ErrorOnce("Thrones are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 1222185, false);
					return null;
				}
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		
		// (get) Token: 0x06004D28 RID: 19752 RVA: 0x0019D5FB File Offset: 0x0019B7FB
		public CompAssignableToPawn_Throne CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Throne>();
			}
		}

		
		// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0019D604 File Offset: 0x0019B804
		public RoyalTitleDef TitleStature
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_Passable);
				if (room == null || room.OutdoorsForWork)
				{
					return null;
				}
				float stat = room.GetStat(RoomStatDefOf.Impressiveness);
				RoyalTitleDef result = null;
				foreach (RoyalTitleDef royalTitleDef in Building_Throne.AllTitlesForThroneStature)
				{
					if (stat <= royalTitleDef.MinThroneRoomImpressiveness)
					{
						break;
					}
					result = royalTitleDef;
				}
				return result;
			}
		}

		
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			Room room = this.GetRoom(RegionType.Set_Passable);
			Pawn p = (this.CompAssignableToPawn.AssignedPawnsForReading.Count == 1) ? this.CompAssignableToPawn.AssignedPawnsForReading[0] : null;
			RoyalTitleDef titleStature = this.TitleStature;
			text += "\n" + "ThroneTitleStature".Translate((titleStature == null) ? "None".Translate() : (titleStature.GetLabelCapFor(p) + " " + "ThroneRoomImpressivenessInfo".Translate(titleStature.MinThroneRoomImpressiveness.ToString())));
			string text2 = RoomRoleWorker_ThroneRoom.Validate(room);
			if (text2 != null)
			{
				return text + "\n" + text2;
			}
			Building_Throne.tmpTitles.Clear();
			Building_Throne.tmpTitles.AddRange(Building_Throne.AllTitlesForThroneStature);
			int num = Building_Throne.tmpTitles.IndexOf(titleStature);
			int num2 = num - 1;
			int num3 = num + 1;
			if (num2 >= 0)
			{
				text += "\n" + "ThronePrevTitleStature".Translate(Building_Throne.tmpTitles[num2].GetLabelCapFor(p)) + " " + "ThroneRoomImpressivenessInfo".Translate(Building_Throne.tmpTitles[num2].MinThroneRoomImpressiveness.ToString());
			}
			if (num3 < Building_Throne.tmpTitles.Count)
			{
				text += "\n" + "ThroneNextTitleStature".Translate(Building_Throne.tmpTitles[num3].GetLabelCapFor(p)) + " " + "ThroneRoomImpressivenessInfo".Translate(Building_Throne.tmpTitles[num3].MinThroneRoomImpressiveness.ToString());
			}
			return text;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Thrones are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.  See rules on the Ludeon forum for more info.", 1222185, false);
				yield break;
			}

			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		
		private static List<RoyalTitleDef> tmpTitles = new List<RoyalTitleDef>();
	}
}
