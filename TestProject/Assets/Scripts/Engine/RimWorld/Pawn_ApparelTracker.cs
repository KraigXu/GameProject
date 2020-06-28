using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA6 RID: 2982
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x060045D6 RID: 17878 RVA: 0x00179228 File Offset: 0x00177428
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x060045D7 RID: 17879 RVA: 0x00179230 File Offset: 0x00177430
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060045D8 RID: 17880 RVA: 0x0017923D File Offset: 0x0017743D
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x060045D9 RID: 17881 RVA: 0x0017924A File Offset: 0x0017744A
		public bool AnyApparel
		{
			get
			{
				return this.wornApparel.Count != 0;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x060045DA RID: 17882 RVA: 0x0017925A File Offset: 0x0017745A
		public bool AnyApparelLocked
		{
			get
			{
				return !this.lockedApparel.NullOrEmpty<Apparel>();
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060045DB RID: 17883 RVA: 0x0017926C File Offset: 0x0017746C
		public bool AnyApparelUnlocked
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.AnyApparel;
				}
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x001792B8 File Offset: 0x001774B8
		public bool AllApparelLocked
		{
			get
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060045DD RID: 17885 RVA: 0x001792F2 File Offset: 0x001774F2
		public List<Apparel> LockedApparel
		{
			get
			{
				if (this.lockedApparel == null)
				{
					return Pawn_ApparelTracker.EmptyApparel;
				}
				return this.lockedApparel;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x060045DE RID: 17886 RVA: 0x00179308 File Offset: 0x00177508
		public IEnumerable<Apparel> UnlockedApparel
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.WornApparel;
				}
				return from x in this.WornApparel
				where !this.IsLocked(x)
				select x;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x00179340 File Offset: 0x00177540
		public bool PsychologicallyNude
		{
			get
			{
				if (this.pawn.gender == Gender.None)
				{
					return false;
				}
				if (this.pawn.IsWildMan())
				{
					return false;
				}
				bool flag;
				bool flag2;
				this.HasBasicApparel(out flag, out flag2);
				if (!flag)
				{
					bool flag3 = false;
					using (IEnumerator<BodyPartRecord> enumerator = this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsInGroup(BodyPartGroupDefOf.Legs))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						flag = true;
					}
				}
				if (this.pawn.gender == Gender.Male)
				{
					return !flag;
				}
				return this.pawn.gender == Gender.Female && (!flag || !flag2);
			}
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00179408 File Offset: 0x00177608
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x0017942C File Offset: 0x0017762C
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Apparel>>(ref this.wornApparel, "wornApparel", new object[]
			{
				this
			});
			Scribe_Collections.Look<Apparel>(ref this.lockedApparel, "lockedApparel", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastApparelWearoutTick, "lastApparelWearoutTick", 0, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.SortWornApparelIntoDrawOrder();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.lockedApparel != null)
			{
				this.lockedApparel.RemoveAll((Apparel x) => x == null);
			}
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x001794C4 File Offset: 0x001776C4
		public void ApparelTrackerTickRare()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.lastApparelWearoutTick < 0)
			{
				this.lastApparelWearoutTick = ticksGame;
			}
			if (ticksGame - this.lastApparelWearoutTick >= 60000)
			{
				if (!this.pawn.IsWorldPawn())
				{
					for (int i = this.wornApparel.Count - 1; i >= 0; i--)
					{
						this.TakeWearoutDamageForDay(this.wornApparel[i]);
					}
				}
				this.lastApparelWearoutTick = ticksGame;
			}
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x0017953C File Offset: 0x0017773C
		public void ApparelTrackerTick()
		{
			this.wornApparel.ThingOwnerTick(true);
			if (this.pawn.IsColonist && this.pawn.Spawned && !this.pawn.Dead && this.pawn.IsHashIntervalTick(60000) && this.PsychologicallyNude)
			{
				TaleRecorder.RecordTale(TaleDefOf.WalkedNaked, new object[]
				{
					this.pawn
				});
			}
			if (this.lockedApparel != null)
			{
				for (int i = this.lockedApparel.Count - 1; i >= 0; i--)
				{
					if (this.lockedApparel[i].def.useHitPoints && (float)this.lockedApparel[i].HitPoints / (float)this.lockedApparel[i].MaxHitPoints < 0.5f)
					{
						this.Unlock(this.lockedApparel[i]);
					}
				}
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x00179626 File Offset: 0x00177826
		public bool IsLocked(Apparel apparel)
		{
			return this.lockedApparel != null && this.lockedApparel.Contains(apparel);
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x0017963E File Offset: 0x0017783E
		public void Lock(Apparel apparel)
		{
			if (this.IsLocked(apparel))
			{
				return;
			}
			if (this.lockedApparel == null)
			{
				this.lockedApparel = new List<Apparel>();
			}
			this.lockedApparel.Add(apparel);
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x00179669 File Offset: 0x00177869
		public void Unlock(Apparel apparel)
		{
			if (!this.IsLocked(apparel))
			{
				return;
			}
			this.lockedApparel.Remove(apparel);
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x00179684 File Offset: 0x00177884
		public void LockAll()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.Lock(this.wornApparel[i]);
			}
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x001796BC File Offset: 0x001778BC
		private void TakeWearoutDamageForDay(Thing ap)
		{
			int num = GenMath.RoundRandom(ap.def.apparel.wearPerDay);
			if (num > 0)
			{
				ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.Dead)
			{
				Messages.Message("MessageWornApparelDeterioratedAway".Translate(GenLabel.ThingLabel(ap.def, ap.Stuff, 1), this.pawn).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x00179774 File Offset: 0x00177974
		public bool CanWearWithoutDroppingAnything(ThingDef apDef)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(apDef, this.wornApparel[i].def, this.pawn.RaceProps.body))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x001797C4 File Offset: 0x001779C4
		public void Wear(Apparel newApparel, bool dropReplacedApparel = true, bool locked = false)
		{
			if (newApparel.Spawned)
			{
				newApparel.DeSpawn(DestroyMode.Vanish);
			}
			if (!ApparelUtility.HasPartsToWear(this.pawn, newApparel.def))
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" tried to wear ",
					newApparel,
					" but he has no body parts required to wear it."
				}), false);
				return;
			}
			if (EquipmentUtility.IsBiocoded(newApparel) && !EquipmentUtility.IsBiocodedFor(newApparel, this.pawn))
			{
				CompBiocodable compBiocodable = newApparel.TryGetComp<CompBiocodable>();
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" tried to wear ",
					newApparel,
					" but it is biocoded for ",
					compBiocodable.CodedPawnLabel,
					" ."
				}), false);
				return;
			}
			for (int i = this.wornApparel.Count - 1; i >= 0; i--)
			{
				Apparel apparel = this.wornApparel[i];
				if (!ApparelUtility.CanWearTogether(newApparel.def, apparel.def, this.pawn.RaceProps.body))
				{
					if (dropReplacedApparel)
					{
						bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
						Apparel apparel2;
						if (!this.TryDrop(apparel, out apparel2, this.pawn.PositionHeld, forbid))
						{
							Log.Error(this.pawn + " could not drop " + apparel, false);
							return;
						}
					}
					else
					{
						this.Remove(apparel);
					}
				}
			}
			if (newApparel.Wearer != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" is trying to wear ",
					newApparel,
					" but this apparel already has a wearer (",
					newApparel.Wearer,
					"). This may or may not cause bugs."
				}), false);
			}
			this.wornApparel.TryAdd(newApparel, false);
			if (locked)
			{
				this.Lock(newApparel);
			}
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x0017998B File Offset: 0x00177B8B
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x0017999C File Offset: 0x00177B9C
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x001799B2 File Offset: 0x00177BB2
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x001799C8 File Offset: 0x00177BC8
		public bool TryDrop(Apparel ap, out Apparel resultingAp, IntVec3 pos, bool forbid = true)
		{
			if (this.wornApparel.TryDrop(ap, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingAp, null, null))
			{
				if (resultingAp != null)
				{
					resultingAp.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x001799FC File Offset: 0x00177BFC
		public void DropAll(IntVec3 pos, bool forbid = true, bool dropLocked = true)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (dropLocked || !this.IsLocked(this.wornApparel[i]))
				{
					Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
				}
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				Apparel apparel;
				this.TryDrop(Pawn_ApparelTracker.tmpApparelList[j], out apparel, pos, forbid);
			}
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x00179A81 File Offset: 0x00177C81
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00179A8F File Offset: 0x00177C8F
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00179AA0 File Offset: 0x00177CA0
		public bool WouldReplaceLockedApparel(Apparel newApparel)
		{
			if (!this.AnyApparelLocked)
			{
				return false;
			}
			for (int i = 0; i < this.lockedApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(newApparel.def, this.lockedApparel[i].def, this.pawn.RaceProps.body))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x00179B00 File Offset: 0x00177D00
		public void Notify_PawnKilled(DamageInfo? dinfo)
		{
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn))
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (this.wornApparel[i].def.useHitPoints)
					{
						int num = Mathf.RoundToInt((float)this.wornApparel[i].HitPoints * Rand.Range(0.15f, 0.4f));
						this.wornApparel[i].TakeDamage(new DamageInfo(dinfo.Value.Def, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					}
				}
			}
			for (int j = 0; j < this.wornApparel.Count; j++)
			{
				this.wornApparel[j].Notify_PawnKilled();
			}
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x00179BF0 File Offset: 0x00177DF0
		public void Notify_LostBodyPart()
		{
			Pawn_ApparelTracker.tmpApparel.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Pawn_ApparelTracker.tmpApparel.Add(this.wornApparel[i]);
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparel.Count; j++)
			{
				Apparel apparel = Pawn_ApparelTracker.tmpApparel[j];
				if (!ApparelUtility.HasPartsToWear(this.pawn, apparel.def))
				{
					this.Remove(apparel);
				}
			}
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00179C6E File Offset: 0x00177E6E
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00179CA0 File Offset: 0x00177EA0
		public void HasBasicApparel(out bool hasPants, out bool hasShirt)
		{
			hasShirt = false;
			hasPants = false;
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						hasShirt = true;
					}
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						hasPants = true;
					}
					if (hasShirt & hasPants)
					{
						return;
					}
				}
			}
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00179D44 File Offset: 0x00177F44
		public Apparel FirstApparelOnBodyPartGroup(BodyPartGroupDef g)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						return apparel;
					}
				}
			}
			return null;
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x00179DB4 File Offset: 0x00177FB4
		public bool BodyPartGroupIsCovered(BodyPartGroupDef bp)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == bp)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x00179E20 File Offset: 0x00178020
		public IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			for (int i = 0; i < this.wornApparel.Count; i = num + 1)
			{
				foreach (Gizmo gizmo in this.wornApparel[i].GetWornGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x00179E30 File Offset: 0x00178030
		private void ApparelChanged()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.SetApparelGraphicsDirty();
				PortraitsCache.SetDirty(this.pawn);
			});
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x00179E43 File Offset: 0x00178043
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.ApparelChanged();
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00179E78 File Offset: 0x00178078
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.ApparelChanged();
			if (this.pawn.outfits != null && this.pawn.outfits.forcedHandler != null)
			{
				this.pawn.outfits.forcedHandler.SetForced(apparel, false);
			}
			if (this.IsLocked(apparel))
			{
				this.Unlock(apparel);
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x00179EF8 File Offset: 0x001780F8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x00179F00 File Offset: 0x00178100
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0400282C RID: 10284
		public Pawn pawn;

		// Token: 0x0400282D RID: 10285
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x0400282E RID: 10286
		private List<Apparel> lockedApparel;

		// Token: 0x0400282F RID: 10287
		private int lastApparelWearoutTick = -1;

		// Token: 0x04002830 RID: 10288
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x04002831 RID: 10289
		private const float AutoUnlockHealthPctThreshold = 0.5f;

		// Token: 0x04002832 RID: 10290
		private static readonly List<Apparel> EmptyApparel = new List<Apparel>();

		// Token: 0x04002833 RID: 10291
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x04002834 RID: 10292
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
