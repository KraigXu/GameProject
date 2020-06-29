using System;
using UnityEngine;

namespace Verse
{
	
	public class DamageFlasher
	{
		
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x000565F9 File Offset: 0x000547F9
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0005660F File Offset: 0x0005480F
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		
		public DamageFlasher(Pawn pawn)
		{
		}

		
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		
		private int lastDamageTick = -9999;

		
		private const int DamagedMatTicksTotal = 16;
	}
}
