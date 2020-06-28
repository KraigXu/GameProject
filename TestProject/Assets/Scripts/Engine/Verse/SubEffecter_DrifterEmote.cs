﻿using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200004C RID: 76
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		// Token: 0x060003AB RID: 939 RVA: 0x000132A9 File Offset: 0x000114A9
		public SubEffecter_DrifterEmote(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000132B4 File Offset: 0x000114B4
		protected void MakeMote(TargetInfo A)
		{
			Vector3 vector = A.HasThing ? A.Thing.DrawPos : A.Cell.ToVector3Shifted();
			if (vector.ShouldSpawnMotesAt(A.Map))
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
					mote.Scale = this.def.scale.RandomInRange;
					mote.exactPosition = vector + Gen.RandomHorizontalVector(this.def.positionRadius);
					mote.rotationRate = this.def.rotationRate.RandomInRange;
					mote.exactRotation = this.def.rotation.RandomInRange;
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
						moteThrown.SetVelocity(this.def.angle.RandomInRange, this.def.speed.RandomInRange);
					}
					if (A.HasThing)
					{
						mote.Attach(A);
					}
					GenSpawn.Spawn(mote, vector.ToIntVec3(), A.Map, WipeMode.Vanish);
				}
			}
		}
	}
}
