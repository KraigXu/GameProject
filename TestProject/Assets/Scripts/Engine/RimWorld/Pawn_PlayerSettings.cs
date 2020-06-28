using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000BBC RID: 3004
	public class Pawn_PlayerSettings : IExposable
	{
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060046F5 RID: 18165 RVA: 0x0018012B File Offset: 0x0017E32B
		// (set) Token: 0x060046F6 RID: 18166 RVA: 0x00180134 File Offset: 0x0017E334
		public Pawn Master
		{
			get
			{
				return this.master;
			}
			set
			{
				if (this.master == value)
				{
					return;
				}
				if (value != null && !this.pawn.training.HasLearned(TrainableDefOf.Obedience))
				{
					Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573, false);
					return;
				}
				bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn);
				this.master = value;
				if (this.pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn)))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x001801B9 File Offset: 0x0017E3B9
		public Area EffectiveAreaRestrictionInPawnCurrentMap
		{
			get
			{
				if (this.areaAllowedInt != null && this.areaAllowedInt.Map != this.pawn.MapHeld)
				{
					return null;
				}
				return this.EffectiveAreaRestriction;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x060046F8 RID: 18168 RVA: 0x001801E3 File Offset: 0x0017E3E3
		public Area EffectiveAreaRestriction
		{
			get
			{
				if (!this.RespectsAllowedArea)
				{
					return null;
				}
				return this.areaAllowedInt;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x001801F5 File Offset: 0x0017E3F5
		// (set) Token: 0x060046FA RID: 18170 RVA: 0x00180200 File Offset: 0x0017E400
		public Area AreaRestriction
		{
			get
			{
				return this.areaAllowedInt;
			}
			set
			{
				if (this.areaAllowedInt == value)
				{
					return;
				}
				this.areaAllowedInt = value;
				if (this.pawn.Spawned && !this.pawn.Drafted && value != null && value == this.EffectiveAreaRestrictionInPawnCurrentMap && value.TrueCount > 0 && this.pawn.jobs != null && this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.AnyTargetOutsideArea(value))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060046FB RID: 18171 RVA: 0x00180297 File Offset: 0x0017E497
		public bool RespectsAllowedArea
		{
			get
			{
				return this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060046FC RID: 18172 RVA: 0x001802CA File Offset: 0x0017E4CA
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060046FD RID: 18173 RVA: 0x00180302 File Offset: 0x0017E502
		public Pawn RespectedMaster
		{
			get
			{
				if (!this.RespectsMaster)
				{
					return null;
				}
				return this.Master;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060046FE RID: 18174 RVA: 0x00180314 File Offset: 0x0017E514
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return this.pawn.IsColonist && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00180334 File Offset: 0x0017E534
		public Pawn_PlayerSettings(Pawn pawn)
		{
			this.pawn = pawn;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.joinTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.joinTick = 0;
			}
			this.Notify_FactionChanged();
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x0018038C File Offset: 0x0017E58C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Values.Look<bool>(ref this.animalsReleased, "animalsReleased", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.medCare, "medCare", MedicalCareCategory.NoCare, false);
			Scribe_References.Look<Area>(ref this.areaAllowedInt, "areaAllowed", false);
			Scribe_References.Look<Pawn>(ref this.master, "master", false);
			Scribe_Values.Look<bool>(ref this.followDrafted, "followDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.followFieldwork, "followFieldwork", false, false);
			Scribe_Values.Look<HostilityResponseMode>(ref this.hostilityResponse, "hostilityResponse", HostilityResponseMode.Flee, false);
			Scribe_Values.Look<bool>(ref this.selfTend, "selfTend", false, false);
			Scribe_Values.Look<int>(ref this.displayOrder, "displayOrder", 0, false);
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x0018044B File Offset: 0x0017E64B
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.Drafted)
			{
				int num = 0;
				bool flag = false;
				foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					if (pawn.training.HasLearned(TrainableDefOf.Release))
					{
						flag = true;
						if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
						{
							num++;
						}
					}
				}
				if (flag)
				{
					Command_Toggle command_Toggle = new Command_Toggle();
					command_Toggle.defaultLabel = "CommandReleaseAnimalsLabel".Translate() + ((num != 0) ? (" (" + num + ")") : "");
					command_Toggle.defaultDesc = "CommandReleaseAnimalsDesc".Translate();
					command_Toggle.icon = TexCommand.ReleaseAnimals;
					command_Toggle.hotKey = KeyBindingDefOf.Misc7;
					command_Toggle.isActive = (() => this.animalsReleased);
					command_Toggle.toggleAction = delegate
					{
						this.animalsReleased = !this.animalsReleased;
						if (this.animalsReleased)
						{
							foreach (Pawn pawn2 in PawnUtility.SpawnedMasteredPawns(this.pawn))
							{
								if (pawn2.caller != null)
								{
									pawn2.caller.Notify_Released();
								}
								pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							}
						}
					};
					if (num == 0)
					{
						command_Toggle.Disable("CommandReleaseAnimalsFail_NoAnimals".Translate());
					}
					yield return command_Toggle;
				}
			}
			yield break;
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0018045B File Offset: 0x0017E65B
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.areaAllowedInt = null;
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x0018046A File Offset: 0x0017E66A
		public void Notify_MadePrisoner()
		{
			this.ResetMedicalCare();
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x00180474 File Offset: 0x0017E674
		public void ResetMedicalCare()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				return;
			}
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				if (this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForColonyAnimal;
					return;
				}
				if (!this.pawn.IsPrisoner)
				{
					this.medCare = Find.PlaySettings.defaultCareForColonyHumanlike;
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForColonyPrisoner;
				return;
			}
			else
			{
				if (this.pawn.Faction == null && this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForNeutralAnimal;
					return;
				}
				if (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					this.medCare = Find.PlaySettings.defaultCareForNeutralFaction;
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
				return;
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x00180563 File Offset: 0x0017E763
		public void Notify_AreaRemoved(Area area)
		{
			if (this.areaAllowedInt == area)
			{
				this.areaAllowedInt = null;
			}
		}

		// Token: 0x040028BD RID: 10429
		private Pawn pawn;

		// Token: 0x040028BE RID: 10430
		private Area areaAllowedInt;

		// Token: 0x040028BF RID: 10431
		public int joinTick = -1;

		// Token: 0x040028C0 RID: 10432
		private Pawn master;

		// Token: 0x040028C1 RID: 10433
		public bool followDrafted;

		// Token: 0x040028C2 RID: 10434
		public bool followFieldwork;

		// Token: 0x040028C3 RID: 10435
		public bool animalsReleased;

		// Token: 0x040028C4 RID: 10436
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		// Token: 0x040028C5 RID: 10437
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		// Token: 0x040028C6 RID: 10438
		public bool selfTend;

		// Token: 0x040028C7 RID: 10439
		public int displayOrder;
	}
}
