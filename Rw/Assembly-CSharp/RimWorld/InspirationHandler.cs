using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200061C RID: 1564
	public class InspirationHandler : IExposable
	{
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002AC8 RID: 10952 RVA: 0x000F9BD1 File Offset: 0x000F7DD1
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000F9BDC File Offset: 0x000F7DDC
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002ACA RID: 10954 RVA: 0x000F9BE4 File Offset: 0x000F7DE4
		public InspirationDef CurStateDef
		{
			get
			{
				if (this.curState == null)
				{
					return null;
				}
				return this.curState.def;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000F9BFC File Offset: 0x000F7DFC
		private float StartInspirationMTBDays
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return -1f;
				}
				float curLevel = this.pawn.needs.mood.CurLevel;
				if (curLevel < 0.5f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.5f, 1f, 210f, 10f, curLevel);
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000F9C5F File Offset: 0x000F7E5F
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000F9C6E File Offset: 0x000F7E6E
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curState != null)
			{
				this.curState.pawn = this.pawn;
			}
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000F9CA6 File Offset: 0x000F7EA6
		public void InspirationHandlerTick()
		{
			if (this.curState != null)
			{
				this.curState.InspirationTick();
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				this.CheckStartRandomInspiration();
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000F9CD0 File Offset: 0x000F7ED0
		[Obsolete("Will be removed in a future game release and replaced with TryStartInspiration_NewTemp.")]
		public bool TryStartInspiration(InspirationDef def)
		{
			return this.TryStartInspiration_NewTemp(def, null);
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000F9CDC File Offset: 0x000F7EDC
		public bool TryStartInspiration_NewTemp(InspirationDef def, string reason = null)
		{
			if (this.Inspired)
			{
				return false;
			}
			if (!def.Worker.InspirationCanOccur(this.pawn))
			{
				return false;
			}
			this.curState = (Inspiration)Activator.CreateInstance(def.inspirationClass);
			this.curState.def = def;
			this.curState.pawn = this.pawn;
			this.curState.reason = reason;
			this.curState.PostStart();
			return true;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000F9D54 File Offset: 0x000F7F54
		public void EndInspiration(Inspiration inspiration)
		{
			if (inspiration == null)
			{
				return;
			}
			if (this.curState != inspiration)
			{
				Log.Error("Tried to end inspiration " + inspiration.ToStringSafe<Inspiration>() + " but current inspiration is " + this.curState.ToStringSafe<Inspiration>(), false);
				return;
			}
			this.curState = null;
			inspiration.PostEnd();
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000F9DA2 File Offset: 0x000F7FA2
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000F9DC6 File Offset: 0x000F7FC6
		public void Reset()
		{
			this.curState = null;
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000F9DD0 File Offset: 0x000F7FD0
		private void CheckStartRandomInspiration()
		{
			if (this.Inspired)
			{
				return;
			}
			float startInspirationMTBDays = this.StartInspirationMTBDays;
			if (startInspirationMTBDays < 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(startInspirationMTBDays, 60000f, 100f))
			{
				InspirationDef randomAvailableInspirationDef = this.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					this.TryStartInspiration_NewTemp(randomAvailableInspirationDef, "LetterInspirationBeginThanksToHighMoodPart".Translate());
				}
			}
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000F9E29 File Offset: 0x000F8029
		private InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		// Token: 0x04001973 RID: 6515
		public Pawn pawn;

		// Token: 0x04001974 RID: 6516
		private Inspiration curState;

		// Token: 0x04001975 RID: 6517
		private const int CheckStartInspirationIntervalTicks = 100;

		// Token: 0x04001976 RID: 6518
		private const float MinMood = 0.5f;

		// Token: 0x04001977 RID: 6519
		private const float StartInspirationMTBDaysAtMaxMood = 10f;
	}
}
