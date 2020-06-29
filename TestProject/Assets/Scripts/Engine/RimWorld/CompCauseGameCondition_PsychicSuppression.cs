using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
	{
		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		
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

		
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_PsychicSuppression)condition).gender = this.gender;
		}

		
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst());
		}

		
		public override void RandomizeSettings()
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		
		public Gender gender;
	}
}
