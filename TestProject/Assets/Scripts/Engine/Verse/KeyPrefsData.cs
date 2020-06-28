using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000459 RID: 1113
	public class KeyPrefsData
	{
		// Token: 0x06002128 RID: 8488 RVA: 0x000CB55C File Offset: 0x000C975C
		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000CB570 File Offset: 0x000C9770
		public void AddMissingDefaultBindings()
		{
			foreach (KeyBindingDef keyBindingDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (!this.keyPrefs.ContainsKey(keyBindingDef))
				{
					this.keyPrefs.Add(keyBindingDef, new KeyBindingData(keyBindingDef.defaultKeyCodeA, keyBindingDef.defaultKeyCodeB));
				}
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000CB5E0 File Offset: 0x000C97E0
		public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
		{
			KeyBindingData keyBindingData;
			if (this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				if (slot != KeyPrefs.BindingSlot.A)
				{
					if (slot != KeyPrefs.BindingSlot.B)
					{
						Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + slot.ToString(), false);
						return false;
					}
					keyBindingData.keyBindingB = keyCode;
				}
				else
				{
					keyBindingData.keyBindingA = keyCode;
				}
				return true;
			}
			Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"", false);
			return false;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000CB67C File Offset: 0x000C987C
		public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyBindingData keyBindingData;
			if (!this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"", false);
				return KeyCode.None;
			}
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return keyBindingData.keyBindingA;
			}
			if (slot != KeyPrefs.BindingSlot.B)
			{
				throw new InvalidOperationException();
			}
			return keyBindingData.keyBindingB;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000CB6DC File Offset: 0x000C98DC
		private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
		{
			using (IEnumerator<KeyBindingDef> enumerator = DefDatabase<KeyBindingDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyBindingDef def = enumerator.Current;
					KeyBindingData keyBindingData;
					if (def != keyDef && ((def.category == keyDef.category && def.category.selfConflicting) || keyDef.category.checkForConflicts.Contains(def.category) || (keyDef.extraConflictTags != null && def.extraConflictTags != null && keyDef.extraConflictTags.Any((string tag) => def.extraConflictTags.Contains(tag)))) && this.keyPrefs.TryGetValue(def, out keyBindingData) && (keyBindingData.keyBindingA == code || keyBindingData.keyBindingB == code))
					{
						yield return def;
					}
				}
			}
			IEnumerator<KeyBindingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000CB6FC File Offset: 0x000C98FC
		public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
		{
			foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, keyCode))
			{
				KeyBindingData keyBindingData = this.keyPrefs[keyBindingDef];
				if (keyBindingData.keyBindingA == keyCode)
				{
					keyBindingData.keyBindingA = KeyCode.None;
				}
				if (keyBindingData.keyBindingB == keyCode)
				{
					keyBindingData.keyBindingB = KeyCode.None;
				}
				if (callBackOnErase != null)
				{
					callBackOnErase(keyBindingDef);
				}
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000CB77C File Offset: 0x000C997C
		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000CB7A8 File Offset: 0x000C99A8
		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in this.keyPrefs)
			{
				keyPrefsData.keyPrefs[keyValuePair.Key] = new KeyBindingData(keyValuePair.Value.keyBindingA, keyValuePair.Value.keyBindingB);
			}
			return keyPrefsData;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000CB82C File Offset: 0x000C9A2C
		public void ErrorCheck()
		{
			foreach (KeyBindingDef keyDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.B);
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000CB884 File Offset: 0x000C9A84
		private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, boundKeyCode))
				{
					bool flag = boundKeyCode != keyDef.GetDefaultKeyCode(slot);
					Log.Warning(string.Concat(new object[]
					{
						"Key binding conflict: ",
						keyBindingDef,
						" and ",
						keyDef,
						" are both bound to ",
						boundKeyCode,
						".",
						flag ? " Fixed automatically." : ""
					}), false);
					if (flag)
					{
						if (slot == KeyPrefs.BindingSlot.A)
						{
							this.keyPrefs[keyDef].keyBindingA = keyDef.defaultKeyCodeA;
						}
						else
						{
							this.keyPrefs[keyDef].keyBindingB = keyDef.defaultKeyCodeB;
						}
						KeyPrefs.Save();
					}
				}
			}
		}

		// Token: 0x04001433 RID: 5171
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();
	}
}
