using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	
	public static class BackstoryDatabase
	{
		
		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		
		public static void ReloadAllBackstories()
		{
			foreach (Backstory backstory in DirectXmlLoader.LoadXmlDataInResourcesFolder<Backstory>("Backstories/Shuffled"))
			{
				DeepProfiler.Start("Backstory.PostLoad");
				try
				{
					backstory.PostLoad();
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Backstory.ResolveReferences");
				try
				{
					backstory.ResolveReferences();
				}
				finally
				{
					DeepProfiler.End();
				}
				foreach (string str in backstory.ConfigErrors(false))
				{
					Log.Error(backstory.title + ": " + str, false);
				}
				DeepProfiler.Start("AddBackstory");
				try
				{
					BackstoryDatabase.AddBackstory(backstory);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			SolidBioDatabase.LoadAllBios();
		}

		
		public static void AddBackstory(Backstory bs)
		{
			if (!BackstoryDatabase.allBackstories.ContainsKey(bs.identifier))
			{
				BackstoryDatabase.allBackstories.Add(bs.identifier, bs);
				BackstoryDatabase.shuffleableBackstoryList.Clear();
				return;
			}
			if (bs == BackstoryDatabase.allBackstories[bs.identifier])
			{
				Log.Error("Tried to add the same backstory twice " + bs.identifier, false);
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"Backstory ",
				bs.title,
				" has same unique save key ",
				bs.identifier,
				" as old backstory ",
				BackstoryDatabase.allBackstories[bs.identifier].title
			}), false);
		}

		
		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		
		public static string GetIdentifierClosestMatch(string identifier, bool closestMatchWarning = true)
		{
			if (BackstoryDatabase.allBackstories.ContainsKey(identifier))
			{
				return identifier;
			}
			string b = BackstoryDatabase.StripNumericSuffix(identifier);
			foreach (KeyValuePair<string, Backstory> keyValuePair in BackstoryDatabase.allBackstories)
			{
				Backstory value = keyValuePair.Value;
				if (BackstoryDatabase.StripNumericSuffix(value.identifier) == b)
				{
					if (closestMatchWarning)
					{
						Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + value.identifier, false);
					}
					return value.identifier;
				}
			}
			Log.Warning("Couldn't find exact match for backstory " + identifier + ", or any close match.", false);
			return identifier;
		}

		
		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		
		public static List<Backstory> ShuffleableBackstoryList(BackstorySlot slot, BackstoryCategoryFilter group)
		{
			Pair<BackstorySlot, BackstoryCategoryFilter> key = new Pair<BackstorySlot, BackstoryCategoryFilter>(slot, group);
			if (!BackstoryDatabase.shuffleableBackstoryList.ContainsKey(key))
			{
				BackstoryDatabase.shuffleableBackstoryList[key] = (from bs in BackstoryDatabase.allBackstories.Values
				where bs.shuffleable && bs.slot == slot && @group.Matches(bs)
				select bs).ToList<Backstory>();
			}
			return BackstoryDatabase.shuffleableBackstoryList[key];
		}

		
		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		
		private static Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>> shuffleableBackstoryList = new Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>>();

		
		private static Regex regex = new Regex("^[^0-9]*");
	}
}
