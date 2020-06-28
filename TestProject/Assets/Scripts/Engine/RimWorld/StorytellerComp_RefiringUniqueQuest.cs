using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A18 RID: 2584
	public class StorytellerComp_RefiringUniqueQuest : StorytellerComp
	{
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003D44 RID: 15684 RVA: 0x0013B2B7 File Offset: 0x001394B7
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003D45 RID: 15685 RVA: 0x00143C8A File Offset: 0x00141E8A
		private StorytellerCompProperties_RefiringUniqueQuest Props
		{
			get
			{
				return (StorytellerCompProperties_RefiringUniqueQuest)this.props;
			}
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00143C97 File Offset: 0x00141E97
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			Quest quest = null;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].root == this.Props.incident.questScriptDef && (quest == null || questsListForReading[i].appearanceTick > quest.appearanceTick))
				{
					quest = questsListForReading[i];
					break;
				}
			}
			bool flag;
			if (quest == null)
			{
				if (this.generateSkipped)
				{
					flag = ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f);
				}
				else
				{
					flag = (this.IntervalsPassed == (int)(this.Props.minDaysPassed * 60f) + 1);
				}
			}
			else
			{
				flag = (this.Props.refireEveryDays >= 0f && (quest.State != QuestState.EndedSuccess && quest.State != QuestState.Ongoing && quest.State != QuestState.NotYetAccepted && quest.cleanupTick >= 0 && this.IntervalsPassed == (int)((float)quest.cleanupTick + this.Props.refireEveryDays * 60000f) / 1000));
			}
			if (flag)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00143CAE File Offset: 0x00141EAE
		public override void Initialize()
		{
			base.Initialize();
			if ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f)
			{
				this.generateSkipped = true;
			}
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00143CD6 File Offset: 0x00141ED6
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}

		// Token: 0x040023C3 RID: 9155
		private bool generateSkipped;
	}
}
