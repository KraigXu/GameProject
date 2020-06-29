﻿using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WITab_Planet : WITab
	{
		
		
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		
		
		private string Desc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PlanetSeed".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.seedString);
				stringBuilder.Append("PlanetCoverageShort".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.planetCoverage.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, WITab_Planet.WinSize.x, WITab_Planet.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, Find.World.info.name);
			Rect rect2 = rect;
			rect2.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.Desc);
		}

		
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);
	}
}
