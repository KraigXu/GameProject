using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public static class LetterMaker
	{
		
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(def + " is not a choice letter.", false);
				return null;
			}
			ChoiceLetter choiceLetter = (ChoiceLetter)LetterMaker.MakeLetter(def);
			choiceLetter.label = label;
			choiceLetter.text = text;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			return choiceLetter;
		}

		
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def, null, null);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			choiceLetter.hyperlinkThingDefs = hyperlinkThingDefs;
			return choiceLetter;
		}
	}
}
