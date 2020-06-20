using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000381 RID: 897
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x06001A9C RID: 6812 RVA: 0x000A3C00 File Offset: 0x000A1E00
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000A3C14 File Offset: 0x000A1E14
		public override void DoWindowContents(Rect inRect)
		{
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (caravan == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			if (Time.frameCount % 3 == 0)
			{
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan);
				List<FloatMenuOption> list2 = list;
				Vector2 vector = this.clickPos;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuWorld.StillValid(this.options[i], list, caravan, ref list2, ref vector))
					{
						this.options[i].Disabled = true;
					}
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000A3CC4 File Offset: 0x000A1EC4
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan)
		{
			List<FloatMenuOption> list = null;
			Vector2 vector = new Vector2(-9999f, -9999f);
			return FloatMenuWorld.StillValid(opt, curOpts, forCaravan, ref list, ref vector);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000A3CF0 File Offset: 0x000A1EF0
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan, ref List<FloatMenuOption> cachedChoices, ref Vector2 cachedChoicesForPos)
		{
			if (opt.revalidateWorldClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateWorldClickTarget.Spawned)
				{
					return false;
				}
				Vector2 vector = opt.revalidateWorldClickTarget.ScreenPos();
				vector.y = (float)UI.screenHeight - vector.y;
				List<FloatMenuOption> list;
				if (vector == cachedChoicesForPos)
				{
					list = cachedChoices;
				}
				else
				{
					cachedChoices = FloatMenuMakerWorld.ChoicesAtFor(vector, forCaravan);
					cachedChoicesForPos = vector;
					list = cachedChoices;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000A3DB4 File Offset: 0x000A1FB4
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (!opt.Disabled && (caravan == null || !FloatMenuWorld.StillValid(opt, FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan), caravan)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000A351F File Offset: 0x000A171F
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x04000FBB RID: 4027
		private Vector2 clickPos;

		// Token: 0x04000FBC RID: 4028
		private const int RevalidateEveryFrame = 3;
	}
}
