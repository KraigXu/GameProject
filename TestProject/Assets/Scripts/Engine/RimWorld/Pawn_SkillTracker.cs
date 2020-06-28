using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBD RID: 3005
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x06004708 RID: 18184 RVA: 0x00180604 File Offset: 0x0017E804
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00180680 File Offset: 0x0017E880
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.skills.RemoveAll((SkillRecord x) => x == null) != 0)
				{
					Log.Error("Some skills were null after loading for " + this.pawn.ToStringSafe<Pawn>(), false);
				}
				if (this.skills.RemoveAll((SkillRecord x) => x.def == null) != 0)
				{
					Log.Error("Some skills had null def after loading for " + this.pawn.ToStringSafe<Pawn>(), false);
				}
				List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < this.skills.Count; j++)
					{
						if (this.skills[j].def == allDefsListForReading[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had no " + allDefsListForReading[i].ToStringSafe<SkillDef>() + " skill. Adding.", false);
						this.skills.Add(new SkillRecord(this.pawn, allDefsListForReading[i]));
					}
				}
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x001807F8 File Offset: 0x0017E9F8
		public SkillRecord GetSkill(SkillDef skillDef)
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				if (this.skills[i].def == skillDef)
				{
					return this.skills[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Did not find skill of def ",
				skillDef,
				", returning ",
				this.skills[0]
			}), false);
			return this.skills[0];
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x0018087C File Offset: 0x0017EA7C
		public void SkillsTick()
		{
			if (this.pawn.IsHashIntervalTick(200))
			{
				if (GenLocalDate.HourInteger(this.pawn) == 0 && (this.lastXpSinceMidnightResetTimestamp < 0 || Find.TickManager.TicksGame - this.lastXpSinceMidnightResetTimestamp >= 30000))
				{
					for (int i = 0; i < this.skills.Count; i++)
					{
						this.skills[i].xpSinceMidnight = 0f;
					}
					this.lastXpSinceMidnightResetTimestamp = Find.TickManager.TicksGame;
				}
				for (int j = 0; j < this.skills.Count; j++)
				{
					this.skills[j].Interval();
				}
			}
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0018092F File Offset: 0x0017EB2F
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00180940 File Offset: 0x0017EB40
		public float AverageOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return 3f;
			}
			float num = 0f;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				num += (float)this.GetSkill(workDef.relevantSkills[i]).Level;
			}
			return num / (float)workDef.relevantSkills.Count;
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x001809A8 File Offset: 0x0017EBA8
		public Passion MaxPassionOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return Passion.None;
			}
			Passion passion = Passion.None;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				Passion passion2 = this.GetSkill(workDef.relevantSkills[i]).passion;
				if (passion2 > passion)
				{
					passion = passion2;
				}
			}
			return passion;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x001809FC File Offset: 0x0017EBFC
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x040028C8 RID: 10440
		private Pawn pawn;

		// Token: 0x040028C9 RID: 10441
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x040028CA RID: 10442
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
