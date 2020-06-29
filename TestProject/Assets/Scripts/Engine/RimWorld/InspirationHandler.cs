using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class InspirationHandler : IExposable
	{
		
		// (get) Token: 0x06002AC8 RID: 10952 RVA: 0x000F9BD1 File Offset: 0x000F7DD1
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000F9BDC File Offset: 0x000F7DDC
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		
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

		
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curState != null)
			{
				this.curState.pawn = this.pawn;
			}
		}

		
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

		
		[Obsolete("Will be removed in a future game release and replaced with TryStartInspiration_NewTemp.")]
		public bool TryStartInspiration(InspirationDef def)
		{
			return this.TryStartInspiration_NewTemp(def, null);
		}

		
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

		
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		
		public void Reset()
		{
			this.curState = null;
		}

		
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

		
		private InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		
		public Pawn pawn;

		
		private Inspiration curState;

		
		private const int CheckStartInspirationIntervalTicks = 100;

		
		private const float MinMood = 0.5f;

		
		private const float StartInspirationMTBDaysAtMaxMood = 10f;
	}
}
