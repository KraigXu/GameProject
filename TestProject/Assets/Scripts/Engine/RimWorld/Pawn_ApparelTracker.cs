using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		
		
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		
		
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		
		
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		
		
		public bool AnyApparel
		{
			get
			{
				return this.wornApparel.Count != 0;
			}
		}

		
		
		public bool AnyApparelLocked
		{
			get
			{
				return !this.lockedApparel.NullOrEmpty<Apparel>();
			}
		}

		
		
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
					IEnumerator<BodyPartRecord> enumerator = this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).GetEnumerator();
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

		
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		
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

		
		public bool IsLocked(Apparel apparel)
		{
			return this.lockedApparel != null && this.lockedApparel.Contains(apparel);
		}

		
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

		
		public void Unlock(Apparel apparel)
		{
			if (!this.IsLocked(apparel))
			{
				return;
			}
			this.lockedApparel.Remove(apparel);
		}

		
		public void LockAll()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.Lock(this.wornApparel[i]);
			}
		}

		
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

		
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		
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

		
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		
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

		
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		
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

		
		private void ApparelChanged()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.SetApparelGraphicsDirty();
				PortraitsCache.SetDirty(this.pawn);
			});
		}

		
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.ApparelChanged();
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
		}

		
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

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public Pawn pawn;

		
		private ThingOwner<Apparel> wornApparel;

		
		private List<Apparel> lockedApparel;

		
		private int lastApparelWearoutTick = -1;

		
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		
		private const float AutoUnlockHealthPctThreshold = 0.5f;

		
		private static readonly List<Apparel> EmptyApparel = new List<Apparel>();

		
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
