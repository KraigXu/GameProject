using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BD RID: 1981
	public class Pawn_WorkSettings : IExposable
	{
		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x0011C124 File Offset: 0x0011A324
		public bool EverWork
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x0011C12F File Offset: 0x0011A32F
		public List<WorkGiver> WorkGiversInOrderNormal
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderNormal;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x0011C145 File Offset: 0x0011A345
		public List<WorkGiver> WorkGiversInOrderEmergency
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderEmerg;
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0011C15B File Offset: 0x0011A35B
		public Pawn_WorkSettings()
		{
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x0011C180 File Offset: 0x0011A380
		public Pawn_WorkSettings(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x0011C1AC File Offset: 0x0011A3AC
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<WorkTypeDef, int>>(ref this.priorities, "priorities", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.priorities != null)
			{
				List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
				for (int i = 0; i < disabledWorkTypes.Count; i++)
				{
					this.Disable(disabledWorkTypes[i]);
				}
			}
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x0011C209 File Offset: 0x0011A409
		public void EnableAndInitializeIfNotAlreadyInitialized()
		{
			if (this.priorities == null)
			{
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x0011C21C File Offset: 0x0011A41C
		public void EnableAndInitialize()
		{
			if (this.priorities == null)
			{
				this.priorities = new DefMap<WorkTypeDef, int>();
			}
			this.priorities.SetAll(0);
			int num = 0;
			foreach (WorkTypeDef w3 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where !w.alwaysStartActive && !this.pawn.WorkTypeIsDisabled(w)
			orderby this.pawn.skills.AverageOfRelevantSkillsFor(w) descending
			select w)
			{
				this.SetPriority(w3, 3);
				num++;
				if (num >= 6)
				{
					break;
				}
			}
			foreach (WorkTypeDef w2 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where w.alwaysStartActive
			select w)
			{
				if (!this.pawn.WorkTypeIsDisabled(w2))
				{
					this.SetPriority(w2, 3);
				}
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int i = 0; i < disabledWorkTypes.Count; i++)
			{
				this.Disable(disabledWorkTypes[i]);
			}
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x0011C358 File Offset: 0x0011A558
		private void ConfirmInitializedDebug()
		{
			if (this.priorities == null)
			{
				Log.Error(this.pawn + " did not have work settings initialized.", false);
				this.EnableAndInitialize();
			}
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x0011C380 File Offset: 0x0011A580
		public void SetPriority(WorkTypeDef w, int priority)
		{
			this.ConfirmInitializedDebug();
			if (priority != 0 && this.pawn.WorkTypeIsDisabled(w))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to change priority on disabled worktype ",
					w,
					" for pawn ",
					this.pawn
				}), false);
				return;
			}
			if (priority < 0 || priority > 4)
			{
				Log.Message("Trying to set work to invalid priority " + priority, false);
			}
			this.priorities[w] = priority;
			if (priority == 0 && this.pawn.jobs != null)
			{
				this.pawn.jobs.Notify_WorkTypeDisabled(w);
			}
			this.workGiversDirty = true;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x0011C428 File Offset: 0x0011A628
		public int GetPriority(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			int num = this.priorities[w];
			if (num > 0 && !Find.PlaySettings.useWorkPriorities)
			{
				return 3;
			}
			return num;
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x0011C45B File Offset: 0x0011A65B
		public bool WorkIsActive(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			return this.GetPriority(w) > 0;
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x0011C46D File Offset: 0x0011A66D
		public void Disable(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			this.SetPriority(w, 0);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x0011C47D File Offset: 0x0011A67D
		public void DisableAll()
		{
			this.ConfirmInitializedDebug();
			this.priorities.SetAll(0);
			this.workGiversDirty = true;
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x0011C498 File Offset: 0x0011A698
		public void Notify_UseWorkPrioritiesChanged()
		{
			this.workGiversDirty = true;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x0011C4A4 File Offset: 0x0011A6A4
		public void Notify_DisabledWorkTypesChanged()
		{
			if (this.priorities == null)
			{
				return;
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int i = 0; i < disabledWorkTypes.Count; i++)
			{
				this.Disable(disabledWorkTypes[i]);
			}
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x0011C4E8 File Offset: 0x0011A6E8
		private void CacheWorkGiversInOrder()
		{
			Pawn_WorkSettings.wtsByPrio.Clear();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num = 999;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				int priority = this.GetPriority(workTypeDef);
				if (priority > 0)
				{
					if (priority < num)
					{
						if (workTypeDef.workGiversByPriority.Any((WorkGiverDef wg) => !wg.emergency))
						{
							num = priority;
						}
					}
					Pawn_WorkSettings.wtsByPrio.Add(workTypeDef);
				}
			}
			Pawn_WorkSettings.wtsByPrio.InsertionSort(delegate(WorkTypeDef a, WorkTypeDef b)
			{
				float value = (float)(a.naturalPriority + (4 - this.GetPriority(a)) * 100000);
				return ((float)(b.naturalPriority + (4 - this.GetPriority(b)) * 100000)).CompareTo(value);
			});
			this.workGiversInOrderEmerg.Clear();
			for (int j = 0; j < Pawn_WorkSettings.wtsByPrio.Count; j++)
			{
				WorkTypeDef workTypeDef2 = Pawn_WorkSettings.wtsByPrio[j];
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiver worker = workTypeDef2.workGiversByPriority[k].Worker;
					if (worker.def.emergency && this.GetPriority(worker.def.workType) <= num)
					{
						this.workGiversInOrderEmerg.Add(worker);
					}
				}
			}
			this.workGiversInOrderNormal.Clear();
			for (int l = 0; l < Pawn_WorkSettings.wtsByPrio.Count; l++)
			{
				WorkTypeDef workTypeDef3 = Pawn_WorkSettings.wtsByPrio[l];
				for (int m = 0; m < workTypeDef3.workGiversByPriority.Count; m++)
				{
					WorkGiver worker2 = workTypeDef3.workGiversByPriority[m].Worker;
					if (!worker2.def.emergency || this.GetPriority(worker2.def.workType) > num)
					{
						this.workGiversInOrderNormal.Add(worker2);
					}
				}
			}
			this.workGiversDirty = false;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x0011C6B8 File Offset: 0x0011A8B8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WorkSettings for " + this.pawn);
			stringBuilder.AppendLine("Cached emergency WorkGivers in order:");
			for (int i = 0; i < this.WorkGiversInOrderEmergency.Count; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					i,
					": ",
					this.DebugStringFor(this.WorkGiversInOrderEmergency[i].def)
				}));
			}
			stringBuilder.AppendLine("Cached normal WorkGivers in order:");
			for (int j = 0; j < this.WorkGiversInOrderNormal.Count; j++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					j,
					": ",
					this.DebugStringFor(this.WorkGiversInOrderNormal[j].def)
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x0011C7B8 File Offset: 0x0011A9B8
		private string DebugStringFor(WorkGiverDef wg)
		{
			return string.Concat(new object[]
			{
				"[",
				this.GetPriority(wg.workType),
				" ",
				wg.workType.defName,
				"] - ",
				wg.defName,
				" (",
				wg.priorityInType,
				")"
			});
		}

		// Token: 0x04001B98 RID: 7064
		private Pawn pawn;

		// Token: 0x04001B99 RID: 7065
		private DefMap<WorkTypeDef, int> priorities;

		// Token: 0x04001B9A RID: 7066
		private bool workGiversDirty = true;

		// Token: 0x04001B9B RID: 7067
		private List<WorkGiver> workGiversInOrderEmerg = new List<WorkGiver>();

		// Token: 0x04001B9C RID: 7068
		private List<WorkGiver> workGiversInOrderNormal = new List<WorkGiver>();

		// Token: 0x04001B9D RID: 7069
		public const int LowestPriority = 4;

		// Token: 0x04001B9E RID: 7070
		public const int DefaultPriority = 3;

		// Token: 0x04001B9F RID: 7071
		private const int MaxInitialActiveWorks = 6;

		// Token: 0x04001BA0 RID: 7072
		private static List<WorkTypeDef> wtsByPrio = new List<WorkTypeDef>();
	}
}
