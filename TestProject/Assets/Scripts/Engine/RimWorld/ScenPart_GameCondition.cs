using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_GameCondition : ScenPart
	{
		
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x0018E7F7 File Offset: 0x0018C9F7
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		
		public override string Summary(Scenario scen)
		{
			return this.def.gameCondition.LabelCap + ": " + this.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1") + ")";
		}

		
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f));
		}

		
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		
		private float durationDays;

		
		private string durationDaysBuf;
	}
}
