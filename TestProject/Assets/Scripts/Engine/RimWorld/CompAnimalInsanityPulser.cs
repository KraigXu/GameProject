using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CE5 RID: 3301
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x001B0764 File Offset: 0x001AE964
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x001B0771 File Offset: 0x001AE971
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x001B0793 File Offset: 0x001AE993
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x001B07B0 File Offset: 0x001AE9B0
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.ticksToInsanityPulse--;
			if (this.ticksToInsanityPulse <= 0)
			{
				this.DoAnimalInsanityPulse();
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x001B0800 File Offset: 0x001AEA00
		private void DoAnimalInsanityPulse()
		{
			IEnumerable<Pawn> enumerable = from p in this.parent.Map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.Position.InHorDistOf(this.parent.Position, (float)this.Props.radius)
			select p;
			bool flag = false;
			using (IEnumerator<Pawn> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalInsanityPulse".Translate(this.parent.Named("SOURCE")), this.parent, MessageTypeDefOf.ThreatSmall, true);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
				if (this.parent.Map == Find.CurrentMap)
				{
					Find.CameraDriver.shaker.DoShake(4f);
				}
			}
		}

		// Token: 0x04002CBB RID: 11451
		private int ticksToInsanityPulse;
	}
}
