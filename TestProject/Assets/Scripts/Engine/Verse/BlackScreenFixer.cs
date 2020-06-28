using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F8 RID: 1016
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x06001E3B RID: 7739 RVA: 0x000BC7CC File Offset: 0x000BA9CC
		private void Start()
		{
			if (Screen.width != 0 && Screen.height != 0)
			{
				Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
			}
		}
	}
}
