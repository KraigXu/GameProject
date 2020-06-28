using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAC RID: 2988
	public class Pawn_NativeVerbs : IVerbOwner, IExposable
	{
		// Token: 0x17000C6F RID: 3183
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

		// Token: 0x17000C70 RID: 3184
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

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x0017B02E File Offset: 0x0017922E
		VerbTracker IVerbOwner.VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x0017B036 File Offset: 0x00179236
		List<VerbProperties> IVerbOwner.VerbProperties
		{
			get
			{
				this.CheckCreateVerbProperties();
				return this.cachedVerbProperties;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x00019EA1 File Offset: 0x000180A1
		List<Tool> IVerbOwner.Tools
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x0015D5D5 File Offset: 0x0015B7D5
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x0017B044 File Offset: 0x00179244
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "NativeVerbs_" + this.pawn.ThingID;
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x0017B05B File Offset: 0x0017925B
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.pawn;
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x0017B066 File Offset: 0x00179266
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x0017B06E File Offset: 0x0017926E
		private Thing ConstantCaster { get; }

		// Token: 0x06004637 RID: 17975 RVA: 0x0017B076 File Offset: 0x00179276
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x0017B091 File Offset: 0x00179291
		public void NativeVerbsTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x0017B0A0 File Offset: 0x001792A0
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

		// Token: 0x0600463A RID: 17978 RVA: 0x0017B114 File Offset: 0x00179314
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

		// Token: 0x0600463B RID: 17979 RVA: 0x0017B185 File Offset: 0x00179385
		public void ExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0017B1A8 File Offset: 0x001793A8
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

		// Token: 0x0400284B RID: 10315
		private Pawn pawn;

		// Token: 0x0400284C RID: 10316
		public VerbTracker verbTracker;

		// Token: 0x0400284D RID: 10317
		private Verb_BeatFire cachedBeatFireVerb;

		// Token: 0x0400284E RID: 10318
		private Verb_Ignite cachedIgniteVerb;

		// Token: 0x0400284F RID: 10319
		private List<VerbProperties> cachedVerbProperties;
	}
}
