using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D15 RID: 3349
	public class CompGiveThoughtToAllMapPawnsOnDestroy : ThingComp
	{
		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x001B4927 File Offset: 0x001B2B27
		private CompProperties_GiveThoughtToAllMapPawnsOnDestroy Props
		{
			get
			{
				return (CompProperties_GiveThoughtToAllMapPawnsOnDestroy)this.props;
			}
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001B4934 File Offset: 0x001B2B34
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				if (!this.Props.message.NullOrEmpty())
				{
					Messages.Message(this.Props.message, new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NegativeEvent, true);
				}
				foreach (Pawn pawn in previousMap.mapPawns.AllPawnsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(this.Props.thought, null);
						}
					}
				}
			}
		}
	}
}
