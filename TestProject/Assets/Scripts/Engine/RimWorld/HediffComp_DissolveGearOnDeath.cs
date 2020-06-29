using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class HediffComp_DissolveGearOnDeath : HediffComp
	{
		
		// (get) Token: 0x0600641B RID: 25627 RVA: 0x0022AE6B File Offset: 0x0022906B
		public HediffCompProperties_DissolveGearOnDeath Props
		{
			get
			{
				return (HediffCompProperties_DissolveGearOnDeath)this.props;
			}
		}

		
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			if (this.Props.injuryCreatedOnDeath != null)
			{
				List<BodyPartRecord> list = new List<BodyPartRecord>(from part in base.Pawn.RaceProps.body.AllParts
				where part.coverageAbs > 0f && !base.Pawn.health.hediffSet.PartIsMissing(part)
				select part);
				int num = Mathf.Min(this.Props.injuryCount.RandomInRange, list.Count);
				for (int i = 0; i < num; i++)
				{
					int index = Rand.Range(0, list.Count);
					BodyPartRecord part2 = list[index];
					list.RemoveAt(index);
					base.Pawn.health.AddHediff(this.Props.injuryCreatedOnDeath, part2, null, null);
				}
			}
		}

		
		public override void Notify_PawnKilled()
		{
			base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			if (!base.Pawn.Spawned)
			{
				return;
			}
			if (this.Props.mote != null)
			{
				Vector3 drawPos = base.Pawn.DrawPos;
				for (int i = 0; i < this.Props.moteCount; i++)
				{
					Vector2 vector = Rand.InsideUnitCircle * this.Props.moteOffsetRange.RandomInRange * (float)Rand.Sign;
					MoteMaker.MakeStaticMote(new Vector3(drawPos.x + vector.x, drawPos.y, drawPos.z + vector.y), base.Pawn.Map, this.Props.mote, 1f);
				}
			}
			if (this.Props.filth != null)
			{
				FilthMaker.TryMakeFilth(base.Pawn.Position, base.Pawn.Map, this.Props.filth, this.Props.filthCount, FilthSourceFlags.None);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(SoundInfo.InMap(base.Pawn, MaintenanceType.None));
			}
		}
	}
}
