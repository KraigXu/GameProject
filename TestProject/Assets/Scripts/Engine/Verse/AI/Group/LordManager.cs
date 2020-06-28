using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020005CE RID: 1486
	public sealed class LordManager : IExposable
	{
		// Token: 0x06002974 RID: 10612 RVA: 0x000F44D4 File Offset: 0x000F26D4
		public LordManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000F44F0 File Offset: 0x000F26F0
		public void ExposeData()
		{
			Scribe_Collections.Look<Lord>(ref this.lords, "lords", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].lordManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = 0; j < this.lords.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.lords[j]);
				}
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000F4578 File Offset: 0x000F2778
		public void LordManagerTick()
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				try
				{
					this.lords[i].LordTick();
				}
				catch (Exception ex)
				{
					Lord lord = this.lords[i];
					Log.Error(string.Format("Exception while ticking lord with job {0}: \r\n{1}", (lord == null) ? "NULL" : lord.LordJob.ToString(), ex.ToString()), false);
				}
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				LordToil curLordToil = this.lords[j].CurLordToil;
				if (curLordToil == null || curLordToil.ShouldFail)
				{
					this.RemoveLord(this.lords[j]);
				}
			}
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000F4644 File Offset: 0x000F2844
		public void LordManagerUpdate()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugDraw();
				}
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000F4680 File Offset: 0x000F2880
		public void LordManagerOnGUI()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugOnGUI();
				}
			}
			if (DebugViewSettings.drawDuties)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				foreach (Pawn pawn in this.map.mapPawns.AllPawns)
				{
					if (pawn.Spawned)
					{
						string text = "";
						if (!pawn.Dead && pawn.mindState.duty != null)
						{
							text = pawn.mindState.duty.ToString();
						}
						if (pawn.InMentalState)
						{
							text = text + "\nMentalState=" + pawn.MentalState.ToString();
						}
						Vector2 vector = pawn.DrawPos.MapToUIPosition();
						Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), text);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000F47B8 File Offset: 0x000F29B8
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
			Find.SignalManager.RegisterReceiver(newLord);
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000F47D8 File Offset: 0x000F29D8
		public void RemoveLord(Lord oldLord)
		{
			this.lords.Remove(oldLord);
			Find.SignalManager.DeregisterReceiver(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000F47F8 File Offset: 0x000F29F8
		public Lord LordOf(Pawn p)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					if (lord.ownedPawns[j] == p)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000F4850 File Offset: 0x000F2A50
		public Lord LordOf(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedBuildings.Count; j++)
				{
					if (lord.ownedBuildings[j] == b)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000F48A8 File Offset: 0x000F2AA8
		public void LogLords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Lords =======");
			stringBuilder.AppendLine("Count: " + this.lords.Count);
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				stringBuilder.AppendLine();
				stringBuilder.Append("#" + (i + 1) + ": ");
				if (lord.LordJob == null)
				{
					stringBuilder.AppendLine("no-job");
				}
				else
				{
					stringBuilder.AppendLine(lord.LordJob.GetType().Name);
				}
				stringBuilder.Append("Current toil: ");
				if (lord.CurLordToil == null)
				{
					stringBuilder.AppendLine("null");
				}
				else
				{
					stringBuilder.AppendLine(lord.CurLordToil.GetType().Name);
				}
				stringBuilder.AppendLine("Members (count: " + lord.ownedPawns.Count + "):");
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"  ",
						lord.ownedPawns[j].LabelShort,
						" (",
						lord.ownedPawns[j].Faction,
						")"
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040018F2 RID: 6386
		public Map map;

		// Token: 0x040018F3 RID: 6387
		public List<Lord> lords = new List<Lord>();
	}
}
