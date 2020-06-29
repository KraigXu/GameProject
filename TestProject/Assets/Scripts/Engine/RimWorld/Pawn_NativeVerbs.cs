using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Pawn_NativeVerbs : IVerbOwner, IExposable
	{
		
		// (get) Token: 0x0600462D RID: 17965 RVA: 0x0017AFE0 File Offset: 0x001791E0
		public Verb_BeatFire BeatFireVerb
		{
			get
			{
				if (this.cachedBeatFireVerb == null)
				{
					this.cachedBeatFireVerb = (Verb_BeatFire)this.verbTracker.GetVerb(VerbCategory.BeatFire);
				}
				return this.cachedBeatFireVerb;
			}
		}

		
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x0017B007 File Offset: 0x00179207
		public Verb_Ignite IgniteVerb
		{
			get
			{
				if (this.cachedIgniteVerb == null)
				{
					this.cachedIgniteVerb = (Verb_Ignite)this.verbTracker.GetVerb(VerbCategory.Ignite);
				}
				return this.cachedIgniteVerb;
			}
		}

		
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x0017B02E File Offset: 0x0017922E
		VerbTracker IVerbOwner.VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x0017B036 File Offset: 0x00179236
		List<VerbProperties> IVerbOwner.VerbProperties
		{
			get
			{
				this.CheckCreateVerbProperties();
				return this.cachedVerbProperties;
			}
		}

		
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x00019EA1 File Offset: 0x000180A1
		List<Tool> IVerbOwner.Tools
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x0015D5D5 File Offset: 0x0015B7D5
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "NativeVerbs_" + this.pawn.ThingID;
		}

		
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.pawn;
		}

		
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x0017B066 File Offset: 0x00179266
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x0017B06E File Offset: 0x0017926E
		private Thing ConstantCaster { get; }

		
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
			this.verbTracker = new VerbTracker(this);
		}

		
		public void NativeVerbsTick()
		{
			this.verbTracker.VerbsTick();
		}

		
		public bool TryStartIgnite(Thing target)
		{
			if (this.IgniteVerb == null)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn,
					" tried to ignite ",
					target,
					" but has no ignite verb."
				}), 76453432, false);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.IgniteVerb.TryStartCastOn(target, false, true);
		}

		
		public bool TryBeatFire(Fire targetFire)
		{
			if (this.BeatFireVerb == null)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn,
					" tried to beat fire ",
					targetFire,
					" but has no beat fire verb."
				}), 935137531, false);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.BeatFireVerb.TryStartCastOn(targetFire, false, true);
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		
		private void CheckCreateVerbProperties()
		{
			if (this.cachedVerbProperties != null)
			{
				return;
			}
			if (this.pawn.RaceProps.intelligence >= Intelligence.ToolUser)
			{
				this.cachedVerbProperties = new List<VerbProperties>();
				this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
				if (!this.pawn.RaceProps.IsMechanoid)
				{
					this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite));
				}
			}
		}

		
		private Pawn pawn;

		
		public VerbTracker verbTracker;

		
		private Verb_BeatFire cachedBeatFireVerb;

		
		private Verb_Ignite cachedIgniteVerb;

		
		private List<VerbProperties> cachedVerbProperties;
	}
}
