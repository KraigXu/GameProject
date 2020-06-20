using System;

namespace Verse
{
	// Token: 0x0200046C RID: 1132
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x000CCE30 File Offset: 0x000CB030
		public static string ToStringHuman(this TemperatureDisplayMode mode)
		{
			switch (mode)
			{
			case TemperatureDisplayMode.Celsius:
				return "Celsius".Translate();
			case TemperatureDisplayMode.Fahrenheit:
				return "Fahrenheit".Translate();
			case TemperatureDisplayMode.Kelvin:
				return "Kelvin".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
