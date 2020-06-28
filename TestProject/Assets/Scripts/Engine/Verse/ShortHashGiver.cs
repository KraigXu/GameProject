using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200006D RID: 109
	public static class ShortHashGiver
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00016AA4 File Offset: 0x00014CA4
		public static void GiveAllShortHashes()
		{
			ShortHashGiver.takenHashesPerDeftype.Clear();
			List<Def> list = new List<Def>();
			foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
			{
				IEnumerable enumerable = (IEnumerable)typeof(DefDatabase<>).MakeGenericType(new Type[]
				{
					type
				}).GetProperty("AllDefs").GetGetMethod().Invoke(null, null);
				list.Clear();
				foreach (object obj in enumerable)
				{
					Def item = (Def)obj;
					list.Add(item);
				}
				list.SortBy((Def d) => d.defName);
				for (int i = 0; i < list.Count; i++)
				{
					ShortHashGiver.GiveShortHash(list[i], type);
				}
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00016BC8 File Offset: 0x00014DC8
		private static void GiveShortHash(Def def, Type defType)
		{
			if (def.shortHash != 0)
			{
				Log.Error(def + " already has short hash.", false);
				return;
			}
			HashSet<ushort> hashSet;
			if (!ShortHashGiver.takenHashesPerDeftype.TryGetValue(defType, out hashSet))
			{
				hashSet = new HashSet<ushort>();
				ShortHashGiver.takenHashesPerDeftype.Add(defType, hashSet);
			}
			ushort num = (ushort)(GenText.StableStringHash(def.defName) % 65535);
			int num2 = 0;
			while (num == 0 || hashSet.Contains(num))
			{
				num += 1;
				num2++;
				if (num2 > 5000)
				{
					Log.Message("Short hashes are saturated. There are probably too many Defs.", false);
				}
			}
			def.shortHash = num;
			hashSet.Add(num);
		}

		// Token: 0x04000169 RID: 361
		private static Dictionary<Type, HashSet<ushort>> takenHashesPerDeftype = new Dictionary<Type, HashSet<ushort>>();
	}
}
