using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA9 RID: 2985
	public class Pawn_MeleeVerbs : IExposable
	{
		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x0017A721 File Offset: 0x00178921
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x0017A729 File Offset: 0x00178929
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x0017A743 File Offset: 0x00178943
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x0017A75C File Offset: 0x0017895C
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x0017A7C0 File Offset: 0x001789C0
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

		// Token: 0x06004615 RID: 17941 RVA: 0x0017A92C File Offset: 0x00178B2C
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

		// Token: 0x06004616 RID: 17942 RVA: 0x0017A9D0 File Offset: 0x00178BD0
		public List<VerbEntry> GetUpdatedAvailableVerbsList(bool terrainTools)
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
			if (!terrainTools)
			{
				List<Verb> allVerbs = this.pawn.verbTracker.AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs[i]))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs[i]);
					}
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
									if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs2[k]))
									{
										Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs2[k]);
									}
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
									if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs3[m]))
									{
										Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs3[m]);
									}
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
						if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb))
						{
							Pawn_MeleeVerbs.verbsToAdd.Add(verb);
						}
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
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb2))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(verb2);
					}
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

		// Token: 0x06004617 RID: 17943 RVA: 0x0017AD14 File Offset: 0x00178F14
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x0017AD14 File Offset: 0x00178F14
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x0017AD1E File Offset: 0x00178F1E
		public void Notify_UsedTerrainBasedVerb()
		{
			this.lastTerrainBasedVerbUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x0017AD30 File Offset: 0x00178F30
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

		// Token: 0x0600461B RID: 17947 RVA: 0x0017AD60 File Offset: 0x00178F60
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

		// Token: 0x0400283B RID: 10299
		private Pawn pawn;

		// Token: 0x0400283C RID: 10300
		private Verb curMeleeVerb;

		// Token: 0x0400283D RID: 10301
		private Thing curMeleeVerbTarget;

		// Token: 0x0400283E RID: 10302
		private int curMeleeVerbUpdateTick;

		// Token: 0x0400283F RID: 10303
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs;

		// Token: 0x04002840 RID: 10304
		public int lastTerrainBasedVerbUseTick = -99999;

		// Token: 0x04002841 RID: 10305
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		// Token: 0x04002842 RID: 10306
		private static List<Verb> verbsToAdd = new List<Verb>();

		// Token: 0x04002843 RID: 10307
		private const int BestMeleeVerbUpdateInterval = 60;

		// Token: 0x04002844 RID: 10308
		public const int TerrainBasedVerbUseDelay = 1200;

		// Token: 0x04002845 RID: 10309
		private const float TerrainBasedVerbChooseChance = 0.04f;
	}
}
