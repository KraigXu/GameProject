using System;
using Verse;

namespace RimWorld
{
	
	public class StatPart_RoomStat : StatPart
	{
		
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		
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

		
		private RoomStatDef roomStat;

		
		[MustTranslate]
		private string customLabel;

		
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;
	}
}
