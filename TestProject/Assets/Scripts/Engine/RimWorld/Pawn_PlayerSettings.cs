using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class Pawn_PlayerSettings : IExposable
	{
		
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

		
		// (get) Token: 0x060046FB RID: 18171 RVA: 0x00180297 File Offset: 0x0017E497
		public bool RespectsAllowedArea
		{
			get
			{
				return this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		
		// (get) Token: 0x060046FC RID: 18172 RVA: 0x001802CA File Offset: 0x0017E4CA
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		
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

		
		// (get) Token: 0x060046FE RID: 18174 RVA: 0x00180314 File Offset: 0x0017E514
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return this.pawn.IsColonist && this.pawn.HostFaction == null;
			}
		}

		
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

		
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.areaAllowedInt = null;
		}

		
		public void Notify_MadePrisoner()
		{
			this.ResetMedicalCare();
		}

		
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

		
		public void Notify_AreaRemoved(Area area)
		{
			if (this.areaAllowedInt == area)
			{
				this.areaAllowedInt = null;
			}
		}

		
		private Pawn pawn;

		
		private Area areaAllowedInt;

		
		public int joinTick = -1;

		
		private Pawn master;

		
		public bool followDrafted;

		
		public bool followFieldwork;

		
		public bool animalsReleased;

		
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		
		public bool selfTend;

		
		public int displayOrder;
	}
}
