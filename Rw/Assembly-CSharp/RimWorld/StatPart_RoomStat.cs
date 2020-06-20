using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100C RID: 4108
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x06006252 RID: 25170 RVA: 0x00221652 File Offset: 0x0021F852
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x00221660 File Offset: 0x0021F860
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					val *= room.GetStat(this.roomStat);
				}
			}
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x00221698 File Offset: 0x0021F898
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					string str;
					if (!this.customLabel.NullOrEmpty())
					{
						str = this.customLabel;
					}
					else
					{
						str = this.roomStat.LabelCap;
					}
					return str + ": x" + room.GetStat(this.roomStat).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x04003BF8 RID: 15352
		private RoomStatDef roomStat;

		// Token: 0x04003BF9 RID: 15353
		[MustTranslate]
		private string customLabel;

		// Token: 0x04003BFA RID: 15354
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;
	}
}
