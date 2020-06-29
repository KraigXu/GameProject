using System;
using System.Collections.Generic;

namespace Verse
{
	
	public static class DebugActionCategories
	{
		
		static DebugActionCategories()
		{
			DebugActionCategories.categoryOrders.Add("Incidents", 100);
			DebugActionCategories.categoryOrders.Add("Quests", 200);
			DebugActionCategories.categoryOrders.Add("Quests (old)", 250);
			DebugActionCategories.categoryOrders.Add("Translation", 300);
			DebugActionCategories.categoryOrders.Add("General", 400);
			DebugActionCategories.categoryOrders.Add("Pawns", 500);
			DebugActionCategories.categoryOrders.Add("Spawning", 600);
			DebugActionCategories.categoryOrders.Add("Map management", 700);
			DebugActionCategories.categoryOrders.Add("Autotests", 800);
			DebugActionCategories.categoryOrders.Add("Mods", 900);
		}

		
		public static int GetOrderFor(string category)
		{
			int result;
			if (DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		
		public const string Incidents = "Incidents";

		
		public const string Quests = "Quests";

		
		public const string QuestsOld = "Quests (old)";

		
		public const string Translation = "Translation";

		
		public const string General = "General";

		
		public const string Pawns = "Pawns";

		
		public const string Spawning = "Spawning";

		
		public const string MapManagement = "Map management";

		
		public const string Autotests = "Autotests";

		
		public const string Mods = "Mods";

		
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
