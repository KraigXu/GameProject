using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Pawn_GuiltTracker : IExposable
	{
		
		// (get) Token: 0x06004725 RID: 18213 RVA: 0x001811E9 File Offset: 0x0017F3E9
		public bool IsGuilty
		{
			get
			{
				return this.TicksUntilInnocent > 0;
			}
		}

		
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x001811F4 File Offset: 0x0017F3F4
		public int TicksUntilInnocent
		{
			get
			{
				return Mathf.Max(0, this.lastGuiltyTick + 60000 - Find.TickManager.TicksGame);
			}
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastGuiltyTick, "lastGuiltyTick", -99999, false);
		}

		
		public void Notify_Guilty()
		{
			this.lastGuiltyTick = Find.TickManager.TicksGame;
		}

		
		public int lastGuiltyTick = -99999;

		
		private const int GuiltyDuration = 60000;
	}
}
