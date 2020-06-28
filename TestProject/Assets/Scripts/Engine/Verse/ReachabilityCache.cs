using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B3 RID: 435
	public class ReachabilityCache
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00044428 File Offset: 0x00042628
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00044438 File Offset: 0x00042638
		public BoolUnknown CachedResultFor(Room A, Room B, TraverseParms traverseParams)
		{
			bool flag;
			if (!this.cacheDict.TryGetValue(new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				return BoolUnknown.Unknown;
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.True;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00044470 File Offset: 0x00042670
		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000444AD File Offset: 0x000426AD
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000444BC File Offset: 0x000426BC
		public void ClearFor(Pawn p)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				if (keyValuePair.Key.TraverseParms.pawn == p)
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00044570 File Offset: 0x00042770
		public void ClearForHostile(Thing hostileTo)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				Pawn pawn = keyValuePair.Key.TraverseParms.pawn;
				if (pawn != null && pawn.HostileTo(hostileTo))
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x0400099E RID: 2462
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x0400099F RID: 2463
		private static List<ReachabilityCache.CachedEntry> tmpCachedEntries = new List<ReachabilityCache.CachedEntry>();

		// Token: 0x020013D1 RID: 5073
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x17001447 RID: 5191
			// (get) Token: 0x060077B4 RID: 30644 RVA: 0x002917AB File Offset: 0x0028F9AB
			// (set) Token: 0x060077B5 RID: 30645 RVA: 0x002917B3 File Offset: 0x0028F9B3
			public int FirstRoomID { get; private set; }

			// Token: 0x17001448 RID: 5192
			// (get) Token: 0x060077B6 RID: 30646 RVA: 0x002917BC File Offset: 0x0028F9BC
			// (set) Token: 0x060077B7 RID: 30647 RVA: 0x002917C4 File Offset: 0x0028F9C4
			public int SecondRoomID { get; private set; }

			// Token: 0x17001449 RID: 5193
			// (get) Token: 0x060077B8 RID: 30648 RVA: 0x002917CD File Offset: 0x0028F9CD
			// (set) Token: 0x060077B9 RID: 30649 RVA: 0x002917D5 File Offset: 0x0028F9D5
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x060077BA RID: 30650 RVA: 0x002917DE File Offset: 0x0028F9DE
			public CachedEntry(int firstRoomID, int secondRoomID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstRoomID < secondRoomID)
				{
					this.FirstRoomID = firstRoomID;
					this.SecondRoomID = secondRoomID;
				}
				else
				{
					this.FirstRoomID = secondRoomID;
					this.SecondRoomID = firstRoomID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x060077BB RID: 30651 RVA: 0x00291810 File Offset: 0x0028FA10
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x060077BC RID: 30652 RVA: 0x0029181A File Offset: 0x0028FA1A
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x060077BD RID: 30653 RVA: 0x00291827 File Offset: 0x0028FA27
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x060077BE RID: 30654 RVA: 0x0029183F File Offset: 0x0028FA3F
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x060077BF RID: 30655 RVA: 0x00291873 File Offset: 0x0028FA73
			public override int GetHashCode()
			{
				return Gen.HashCombineStruct<TraverseParms>(Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID), this.TraverseParms);
			}
		}
	}
}
