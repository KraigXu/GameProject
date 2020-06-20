using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE1 RID: 3297
	public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
	{
		// Token: 0x06004FFD RID: 20477 RVA: 0x001AF88D File Offset: 0x001ADA8D
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x001AF89D File Offset: 0x001ADA9D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x001AF8B7 File Offset: 0x001ADAB7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.gender.GetLabel(false),
				action = delegate
				{
					if (this.gender == Gender.Female)
					{
						this.gender = Gender.Male;
					}
					else
					{
						this.gender = Gender.Female;
					}
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield break;
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x001AF8C8 File Offset: 0x001ADAC8
		public override void CompTick()
		{
			base.CompTick();
			if (!base.Active || base.MyTile == -1)
			{
				return;
			}
			foreach (Caravan caravan in Find.World.worldObjects.Caravans)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(caravan.Tile, base.MyTile) < (float)base.Props.worldRange)
				{
					foreach (Pawn pawn in caravan.pawns)
					{
						GameCondition_PsychicSuppression.CheckPawn(pawn, this.gender);
					}
				}
			}
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x001AF9A0 File Offset: 0x001ADBA0
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_PsychicSuppression)condition).gender = this.gender;
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x001AF9BC File Offset: 0x001ADBBC
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst());
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x001AFA1B File Offset: 0x001ADC1B
		public override void RandomizeSettings()
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x04002CB5 RID: 11445
		public Gender gender;
	}
}
