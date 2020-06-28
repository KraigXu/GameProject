using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC9 RID: 3017
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06004774 RID: 18292 RVA: 0x00183E64 File Offset: 0x00182064
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x00183E6C File Offset: 0x0018206C
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x00183E88 File Offset: 0x00182088
		public void ExposeData()
		{
			Scribe_Collections.Look<Thought_Memory>(ref this.memories, "memories", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.memories.Count - 1; i >= 0; i--)
				{
					if (this.memories[i].def == null)
					{
						this.memories.RemoveAt(i);
					}
					else
					{
						this.memories[i].pawn = this.pawn;
					}
				}
			}
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x00183F04 File Offset: 0x00182104
		public void MemoryThoughtInterval()
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				this.memories[i].ThoughtInterval();
			}
			this.RemoveExpiredMemories();
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x00183F40 File Offset: 0x00182140
		private void RemoveExpiredMemories()
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.ShouldDiscard)
				{
					this.RemoveMemory(thought_Memory);
					if (thought_Memory.def.nextThought != null)
					{
						this.TryGainMemory(thought_Memory.def.nextThought, null);
					}
				}
			}
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x00183FA0 File Offset: 0x001821A0
		public void TryGainMemoryFast(ThoughtDef mem)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				return;
			}
			this.TryGainMemory(mem, null);
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x00183FC7 File Offset: 0x001821C7
		public void TryGainMemory(ThoughtDef def, Pawn otherPawn = null)
		{
			if (!def.IsMemory)
			{
				Log.Error(def + " is not a memory thought.", false);
				return;
			}
			this.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(def), otherPawn);
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x00183FF8 File Offset: 0x001821F8
		public void TryGainMemory(Thought_Memory newThought, Pawn otherPawn = null)
		{
			if (!ThoughtUtility.CanGetThought(this.pawn, newThought.def))
			{
				return;
			}
			if (newThought is Thought_MemorySocial && newThought.otherPawn == null && otherPawn == null)
			{
				Log.Error("Can't gain social thought " + newThought.def + " because its otherPawn is null and otherPawn passed to this method is also null. Social thoughts must have otherPawn.", false);
				return;
			}
			newThought.pawn = this.pawn;
			newThought.otherPawn = otherPawn;
			bool flag;
			if (!newThought.TryMergeWithExistingMemory(out flag))
			{
				this.memories.Add(newThought);
			}
			if (newThought.def.stackLimitForSameOtherPawn >= 0)
			{
				while (this.NumMemoriesInGroup(newThought) > newThought.def.stackLimitForSameOtherPawn)
				{
					this.RemoveMemory(this.OldestMemoryInGroup(newThought));
				}
			}
			if (newThought.def.stackLimit >= 0)
			{
				while (this.NumMemoriesOfDef(newThought.def) > newThought.def.stackLimit)
				{
					this.RemoveMemory(this.OldestMemoryOfDef(newThought.def));
				}
			}
			if (newThought.def.thoughtToMake != null)
			{
				this.TryGainMemory(newThought.def.thoughtToMake, newThought.otherPawn);
			}
			if (flag && newThought.def.showBubble && this.pawn.Spawned && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				MoteMaker.MakeMoodThoughtBubble(this.pawn, newThought);
			}
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x00184138 File Offset: 0x00182338
		public Thought_Memory OldestMemoryInGroup(Thought_Memory group)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.GroupsWith(group) && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0018418C File Offset: 0x0018238C
		public Thought_Memory OldestMemoryOfDef(ThoughtDef def)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x001841E0 File Offset: 0x001823E0
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.", false);
			}
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x00184210 File Offset: 0x00182410
		public int NumMemoriesInGroup(Thought_Memory group)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].GroupsWith(group))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x00184250 File Offset: 0x00182450
		public int NumMemoriesOfDef(ThoughtDef def)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x00184290 File Offset: 0x00182490
		public Thought_Memory GetFirstMemoryOfDef(ThoughtDef def)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					return this.memories[i];
				}
			}
			return null;
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x001842D8 File Offset: 0x001824D8
		public void RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef def, Pawn otherPawn)
		{
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def && x.otherPawn == otherPawn));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x00184330 File Offset: 0x00182530
		public void RemoveMemoriesWhereOtherPawnIs(Pawn otherPawn)
		{
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.otherPawn == otherPawn));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x00184384 File Offset: 0x00182584
		public void RemoveMemoriesOfDef(ThoughtDef def)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.", false);
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x001843FC File Offset: 0x001825FC
		public void RemoveMemoriesOfDefIf(ThoughtDef def, Func<Thought_Memory, bool> predicate)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.", false);
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def && predicate(x)));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x00184478 File Offset: 0x00182678
		public bool AnyMemoryConcerns(Pawn otherPawn)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x001844B2 File Offset: 0x001826B2
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}

		// Token: 0x0400291B RID: 10523
		public Pawn pawn;

		// Token: 0x0400291C RID: 10524
		private List<Thought_Memory> memories = new List<Thought_Memory>();
	}
}
