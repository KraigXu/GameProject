using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D5 RID: 1237
	public abstract class SoundFilter
	{
		// Token: 0x06002440 RID: 9280
		public abstract void SetupOn(AudioSource source);

		// Token: 0x06002441 RID: 9281 RVA: 0x000D8B60 File Offset: 0x000D6D60
		protected static T GetOrMakeFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T t = source.gameObject.GetComponent<T>();
			if (t != null)
			{
				t.enabled = true;
			}
			else
			{
				t = source.gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}
