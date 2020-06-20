using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000442 RID: 1090
	public static class SimpleColorExtension
	{
		// Token: 0x06002085 RID: 8325 RVA: 0x000C6B44 File Offset: 0x000C4D44
		public static Color ToUnityColor(this SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return Color.white;
			case SimpleColor.Red:
				return Color.red;
			case SimpleColor.Green:
				return Color.green;
			case SimpleColor.Blue:
				return Color.blue;
			case SimpleColor.Magenta:
				return Color.magenta;
			case SimpleColor.Yellow:
				return Color.yellow;
			case SimpleColor.Cyan:
				return Color.cyan;
			default:
				throw new ArgumentException();
			}
		}
	}
}
