using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public static class GraphicDatabaseUtility
	{
		
		public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
		{
			HashSet<string> loadedAssetNames = new HashSet<string>();
			Texture2D[] array = Resources.LoadAll<Texture2D>("Textures/" + folderPath);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].name.Split(new char[]
				{
					'_'
				});
				string text = "";
				if (array2.Length <= 2)
				{
					text = array2[0];
				}
				else if (array2.Length == 3)
				{
					text = array2[0] + "_" + array2[1];
				}
				else if (array2.Length == 4)
				{
					text = string.Concat(new string[]
					{
						array2[0],
						"_",
						array2[1],
						"_",
						array2[2]
					});
				}
				else
				{
					Log.Error("Cannot load assets with >3 pieces.", false);
				}
				if (!loadedAssetNames.Contains(text))
				{
					loadedAssetNames.Add(text);
					
				}
			}
			array = null;
			yield break;
		}
	}
}
