using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public class GameConditionDef : Def
	{
		
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
			yield break;
			yield break;
		}

		
		public Type conditionClass = typeof(GameCondition);

		
		private List<GameConditionDef> exclusiveConditions;

		
		[MustTranslate]
		public string endMessage;

		
		[MustTranslate]
		public string letterText;

		
		public List<ThingDef> letterHyperlinks;

		
		public LetterDef letterDef;

		
		public bool canBePermanent;

		
		[MustTranslate]
		public string descriptionFuture;

		
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		
		public bool preventRain;

		
		public WeatherDef weatherDef;

		
		public float temperatureOffset = -10f;
	}
}
