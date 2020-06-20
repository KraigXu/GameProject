using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6F RID: 2927
	public static class BackstoryDatabase
	{
		// Token: 0x06004486 RID: 17542 RVA: 0x0017278D File Offset: 0x0017098D
		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0017279C File Offset: 0x0017099C
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

		// Token: 0x06004488 RID: 17544 RVA: 0x001728AC File Offset: 0x00170AAC
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

		// Token: 0x06004489 RID: 17545 RVA: 0x00172964 File Offset: 0x00170B64
		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0017297C File Offset: 0x00170B7C
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

		// Token: 0x0600448B RID: 17547 RVA: 0x00172A3C File Offset: 0x00170C3C
		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x00172A7C File Offset: 0x00170C7C
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

		// Token: 0x0600448D RID: 17549 RVA: 0x00172AF3 File Offset: 0x00170CF3
		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x0400271A RID: 10010
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		// Token: 0x0400271B RID: 10011
		private static Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>> shuffleableBackstoryList = new Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>>();

		// Token: 0x0400271C RID: 10012
		private static Regex regex = new Regex("^[^0-9]*");
	}
}
