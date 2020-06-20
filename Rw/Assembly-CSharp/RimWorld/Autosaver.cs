using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	// Token: 0x02000F33 RID: 3891
	public sealed class Autosaver
	{
		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06005F4F RID: 24399 RVA: 0x0020E9B4 File Offset: 0x0020CBB4
		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x06005F50 RID: 24400 RVA: 0x0020E9E7 File Offset: 0x0020CBE7
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x0020E9FA File Offset: 0x0020CBFA
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null, true);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x0020EA38 File Offset: 0x0020CC38
		public void DoAutosave()
		{
			string fileName;
			if (Current.Game.Info.permadeathMode)
			{
				fileName = Current.Game.Info.permadeathModeUniqueName;
			}
			else
			{
				fileName = this.NewAutosaveFileName();
			}
			GameDataSaveLoader.SaveGame(fileName);
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x0008A0BA File Offset: 0x000882BA
		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06005F54 RID: 24404 RVA: 0x0020EA78 File Offset: 0x0020CC78
		private string NewAutosaveFileName()
		{
			string text = (from name in this.AutoSaveNames()
			where !SaveGameFilesUtility.SavedGameNamedExists(name)
			select name).FirstOrDefault<string>();
			if (text != null)
			{
				return text;
			}
			return this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
		}

		// Token: 0x06005F55 RID: 24405 RVA: 0x0020EAE4 File Offset: 0x0020CCE4
		private IEnumerable<string> AutoSaveNames()
		{
			int num;
			for (int i = 1; i <= 5; i = num + 1)
			{
				yield return "Autosave-" + i;
				num = i;
			}
			yield break;
		}

		// Token: 0x040033B6 RID: 13238
		private int ticksSinceSave;

		// Token: 0x040033B7 RID: 13239
		private const int NumAutosaves = 5;

		// Token: 0x040033B8 RID: 13240
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
