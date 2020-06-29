using System;
using UnityEngine;

namespace RimWorld
{
	
	public class MainTabWindow_Factions : MainTabWindow
	{
		
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}

		
		private Vector2 scrollPosition = Vector2.zero;

		
		private float scrollViewHeight;
	}
}
