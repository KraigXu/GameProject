﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class Pawn_MeleeVerbs : IExposable
	{
		
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x0017A721 File Offset: 0x00178921
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
		}

		
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		
		private void ChooseMeleeVerb(Thing target)
		{
			bool flag = Rand.Chance(0.04f);
			List<VerbEntry> updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(flag);
			bool flag2 = false;
			VerbEntry verbEntry;
			if (updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry))
			{
				flag2 = true;
			}
			else if (flag)
			{
				updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(false);
				flag2 = updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry);
			}
			if (flag2)
			{
				this.SetCurMeleeVerb(verbEntry.verb, target);
				return;
			}
			Log.ErrorOnce(string.Concat(new string[]
			{
				this.pawn.ToStringSafe<Pawn>(),
				" has no available melee attack, spawned=",
				this.pawn.Spawned.ToString(),
				" dead=",
				this.pawn.Dead.ToString(),
				" downed=",
				this.pawn.Downed.ToString(),
				" curJob=",
				this.pawn.CurJob.ToStringSafe<Job>(),
				" verbList=",
				updatedAvailableVerbsList.ToStringSafeEnumerable(),
				" bodyVerbs=",
				this.pawn.verbTracker.AllVerbs.ToStringSafeEnumerable()
			}), this.pawn.thingIDNumber ^ 195867354, false);
			this.SetCurMeleeVerb(null, null);
		}

		
		public bool TryMeleeAttack(Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			if (this.pawn.stances.FullBodyBusy)
			{
				return false;
			}
			if (verbToUse != null)
			{
				if (!verbToUse.IsStillUsableBy(this.pawn))
				{
					return false;
				}
				if (!verbToUse.IsMeleeAttack)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Pawn ",
						this.pawn,
						" tried to melee attack ",
						target,
						" with non melee-attack verb ",
						verbToUse,
						"."
					}), false);
					return false;
				}
			}
			Verb verb;
			if (verbToUse != null)
			{
				verb = verbToUse;
			}
			else
			{
				verb = this.TryGetMeleeVerb(target);
			}
			if (verb == null)
			{
				return false;
			}
			verb.TryStartCastOn(target, surpriseAttack, true);
			return true;
		}

		
		public List<VerbEntry> GetUpdatedAvailableVerbsList(bool terrainTools)
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
			if (!terrainTools)
			{
				List<Verb> allVerbs = this.pawn.verbTracker.AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					//if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs[i]))
					//{
					//	Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs[i]);
					//}
				}
				if (this.pawn.equipment != null)
				{
					List<ThingWithComps> allEquipmentListForReading = this.pawn.equipment.AllEquipmentListForReading;
					for (int j = 0; j < allEquipmentListForReading.Count; j++)
					{
						CompEquippable comp = allEquipmentListForReading[j].GetComp<CompEquippable>();
						if (comp != null)
						{
							List<Verb> allVerbs2 = comp.AllVerbs;
							if (allVerbs2 != null)
							{
								for (int k = 0; k < allVerbs2.Count; k++)
								{
									//if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs2[k]))
									//{
									//	Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs2[k]);
									//}
								}
							}
						}
					}
				}
				if (this.pawn.apparel != null)
				{
					List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
					for (int l = 0; l < wornApparel.Count; l++)
					{
						CompEquippable comp2 = wornApparel[l].GetComp<CompEquippable>();
						if (comp2 != null)
						{
							List<Verb> allVerbs3 = comp2.AllVerbs;
							if (allVerbs3 != null)
							{
								for (int m = 0; m < allVerbs3.Count; m++)
								{
									//if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs3[m]))
									//{
									//	Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs3[m]);
									//}
								}
							}
						}
					}
				}
				using (IEnumerator<Verb> enumerator = this.pawn.health.hediffSet.GetHediffsVerbs().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Verb verb = enumerator.Current;
						//if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb))
						//{
						//	Pawn_MeleeVerbs.verbsToAdd.Add(verb);
						//}
					}
					goto IL_271;
				}
			}
			if (this.pawn.Spawned)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (this.terrainVerbs == null || this.terrainVerbs.def != terrain)
				{
					this.terrainVerbs = Pawn_MeleeVerbs_TerrainSource.Create(this, terrain);
				}
				List<Verb> allVerbs4 = this.terrainVerbs.tracker.AllVerbs;
				for (int n = 0; n < allVerbs4.Count; n++)
				{
					Verb verb2 = allVerbs4[n];
					//if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb2))
					//{
					//	Pawn_MeleeVerbs.verbsToAdd.Add(verb2);
					//}
				}
			}
			IL_271:
			float num = 0f;
			foreach (Verb v in Pawn_MeleeVerbs.verbsToAdd)
			{
				float num2 = VerbUtility.InitialVerbWeight(v, this.pawn);
				if (num2 > num)
				{
					num = num2;
				}
			}
			foreach (Verb verb3 in Pawn_MeleeVerbs.verbsToAdd)
			{
				Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(verb3, this.pawn, Pawn_MeleeVerbs.verbsToAdd, num));
			}
			return Pawn_MeleeVerbs.meleeVerbs;
		}

		
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		
		public void Notify_UsedTerrainBasedVerb()
		{
			this.lastTerrainBasedVerbUseTick = Find.TickManager.TicksGame;
		}

		
		private void SetCurMeleeVerb(Verb v, Thing target)
		{
			this.curMeleeVerb = v;
			this.curMeleeVerbTarget = target;
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.curMeleeVerbUpdateTick = 0;
				return;
			}
			this.curMeleeVerbUpdateTick = Find.TickManager.TicksGame;
		}

		
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.curMeleeVerb != null && !this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				this.curMeleeVerb = null;
			}
			Scribe_References.Look<Verb>(ref this.curMeleeVerb, "curMeleeVerb", false);
			Scribe_Values.Look<int>(ref this.curMeleeVerbUpdateTick, "curMeleeVerbUpdateTick", 0, false);
			Scribe_Deep.Look<Pawn_MeleeVerbs_TerrainSource>(ref this.terrainVerbs, "terrainVerbs", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastTerrainBasedVerbUseTick, "lastTerrainBasedVerbUseTick", -99999, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.terrainVerbs != null)
			{
				this.terrainVerbs.parent = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curMeleeVerb != null && this.curMeleeVerb.BuggedAfterLoading)
			{
				this.curMeleeVerb = null;
				Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had a bugged melee verb after loading.", false);
			}
		}

		
		private Pawn pawn;

		
		private Verb curMeleeVerb;

		
		private Thing curMeleeVerbTarget;

		
		private int curMeleeVerbUpdateTick;

		
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs;

		
		public int lastTerrainBasedVerbUseTick = -99999;

		
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		
		private static List<Verb> verbsToAdd = new List<Verb>();

		
		private const int BestMeleeVerbUpdateInterval = 60;

		
		public const int TerrainBasedVerbUseDelay = 1200;

		
		private const float TerrainBasedVerbChooseChance = 0.04f;
	}
}
