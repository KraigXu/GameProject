using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				float num = inRect.height - 79f;
				FactionUIUtility.DrawRelatedFactionInfo(inRect, this.faction, ref num);
			}
		}

		
		private Faction faction;

		
		private const float RelatedFactionInfoSize = 79f;
	}
}
