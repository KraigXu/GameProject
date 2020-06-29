using System;
using UnityEngine;

namespace RimWorld.Planet
{
	
	public class Caravan_Tweener
	{
		
		// (get) Token: 0x06006C40 RID: 27712 RVA: 0x0025BAA8 File Offset: 0x00259CA8
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		
		// (get) Token: 0x06006C41 RID: 27713 RVA: 0x0025BAB0 File Offset: 0x00259CB0
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		
		// (get) Token: 0x06006C42 RID: 27714 RVA: 0x0025BAC3 File Offset: 0x00259CC3
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		
		private Caravan caravan;

		
		private Vector3 tweenedPos = Vector3.zero;

		
		private Vector3 lastTickSpringPos;

		
		private const float SpringTightness = 0.09f;
	}
}
