using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	
	public sealed class Autosaver
	{
		
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

		
		// (get) Token: 0x06005F50 RID: 24400 RVA: 0x0020E9E7 File Offset: 0x0020CBE7
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null, true);
				this.ticksSinceSave = 0;
			}
		}

		
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

		
		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		
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

		
		private int ticksSinceSave;

		
		private const int NumAutosaves = 5;

		
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
