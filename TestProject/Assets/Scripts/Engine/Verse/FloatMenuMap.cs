using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037E RID: 894
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06001A82 RID: 6786 RVA: 0x000A331C File Offset: 0x000A151C
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000A3330 File Offset: 0x000A1530
		public override void DoWindowContents(Rect inRect)
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			if (Time.frameCount % 3 == 0)
			{
				List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn);
				List<FloatMenuOption> list2 = list;
				Vector3 vector = this.clickPos;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuMap.StillValid(this.options[i], list, pawn, ref list2, ref vector))
					{
						this.options[i].Disabled = true;
					}
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000A33E0 File Offset: 0x000A15E0
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Pawn forPawn)
		{
			List<FloatMenuOption> list = null;
			Vector3 vector = new Vector3(-9999f, -9999f, -9999f);
			return FloatMenuMap.StillValid(opt, curOpts, forPawn, ref list, ref vector);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000A3414 File Offset: 0x000A1614
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Pawn forPawn, ref List<FloatMenuOption> cachedChoices, ref Vector3 cachedChoicesForPos)
		{
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateClickTarget.Spawned)
				{
					return false;
				}
				Vector3 vector = opt.revalidateClickTarget.Position.ToVector3Shifted();
				List<FloatMenuOption> list;
				if (vector == cachedChoicesForPos)
				{
					list = cachedChoices;
				}
				else
				{
					cachedChoices = FloatMenuMakerMap.ChoicesAtFor(vector, forPawn);
					cachedChoicesForPos = vector;
					list = cachedChoices;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000A34D4 File Offset: 0x000A16D4
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (!opt.Disabled && (pawn == null || !FloatMenuMap.StillValid(opt, FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn), pawn)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x000A351F File Offset: 0x000A171F
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x04000F8F RID: 3983
		private Vector3 clickPos;

		// Token: 0x04000F90 RID: 3984
		public const int RevalidateEveryFrame = 3;
	}
}
