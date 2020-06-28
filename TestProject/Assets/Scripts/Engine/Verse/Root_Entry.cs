using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000123 RID: 291
	public class Root_Entry : Root
	{
		// Token: 0x0600082E RID: 2094 RVA: 0x00025B34 File Offset: 0x00023D34
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00025BA0 File Offset: 0x00023DA0
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				this.musicManagerEntry.MusicManagerEntryUpdate();
				if (Find.World != null)
				{
					Find.World.WorldUpdate();
				}
				if (Current.Game != null)
				{
					Current.Game.UpdateEntry();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg, false);
			}
		}

		// Token: 0x04000741 RID: 1857
		public MusicManagerEntry musicManagerEntry;
	}
}
