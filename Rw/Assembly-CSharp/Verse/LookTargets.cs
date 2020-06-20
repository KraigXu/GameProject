using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200040E RID: 1038
	public class LookTargets : IExposable
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x00019EA1 File Offset: 0x000180A1
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x000BEED0 File Offset: 0x000BD0D0
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x000BEEEB File Offset: 0x000BD0EB
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x000BEEFC File Offset: 0x000BD0FC
		public GlobalTargetInfo PrimaryTarget
		{
			get
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i].IsValid)
					{
						return this.targets[i];
					}
				}
				if (this.targets.Count != 0)
				{
					return this.targets[0];
				}
				return GlobalTargetInfo.Invalid;
			}
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000BEF61 File Offset: 0x000BD161
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000BEF79 File Offset: 0x000BD179
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000BEF8C File Offset: 0x000BD18C
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000BEFB0 File Offset: 0x000BD1B0
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000BEFD4 File Offset: 0x000BD1D4
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000BEFFA File Offset: 0x000BD1FA
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000BF01E File Offset: 0x000BD21E
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000BF040 File Offset: 0x000BD240
		public LookTargets(params GlobalTargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000BF084 File Offset: 0x000BD284
		public LookTargets(IEnumerable<TargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				IList<TargetInfo> list = targets as IList<TargetInfo>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
					return;
				}
				foreach (TargetInfo target in targets)
				{
					this.targets.Add(target);
				}
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000BF120 File Offset: 0x000BD320
		public LookTargets(params TargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000BF166 File Offset: 0x000BD366
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000BF180 File Offset: 0x000BD380
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000BF19A File Offset: 0x000BD39A
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000BF1B4 File Offset: 0x000BD3B4
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x000BF1CE File Offset: 0x000BD3CE
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000BF1E8 File Offset: 0x000BD3E8
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000BF202 File Offset: 0x000BD402
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000BF21C File Offset: 0x000BD41C
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000BF224 File Offset: 0x000BD424
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000BF22C File Offset: 0x000BD42C
		public static implicit operator LookTargets(TargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000BF24F File Offset: 0x000BD44F
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000BF257 File Offset: 0x000BD457
		public static implicit operator LookTargets(GlobalTargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000BF275 File Offset: 0x000BD475
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000BF27D File Offset: 0x000BD47D
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000BF285 File Offset: 0x000BD485
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000BF28D File Offset: 0x000BD48D
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000BF295 File Offset: 0x000BD495
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000BF29D File Offset: 0x000BD49D
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000BF2A5 File Offset: 0x000BD4A5
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000BF2AD File Offset: 0x000BD4AD
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x000BF2B8 File Offset: 0x000BD4B8
		public static bool SameTargets(LookTargets a, LookTargets b)
		{
			if (a == null)
			{
				return b == null || !b.Any;
			}
			if (b == null)
			{
				return a == null || !a.Any;
			}
			if (a.targets.Count != b.targets.Count)
			{
				return false;
			}
			for (int i = 0; i < a.targets.Count; i++)
			{
				if (a.targets[i] != b.targets[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000BF33C File Offset: 0x000BD53C
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000BF374 File Offset: 0x000BD574
		private void AppendThingTargets<T>(IEnumerable<T> things) where T : Thing
		{
			if (things == null)
			{
				return;
			}
			IList<T> list = things as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in things)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000BF408 File Offset: 0x000BD608
		private void AppendWorldObjectTargets<T>(IEnumerable<T> worldObjects) where T : WorldObject
		{
			if (worldObjects == null)
			{
				return;
			}
			IList<T> list = worldObjects as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in worldObjects)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000BF49C File Offset: 0x000BD69C
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x040012EC RID: 4844
		public List<GlobalTargetInfo> targets;
	}
}
