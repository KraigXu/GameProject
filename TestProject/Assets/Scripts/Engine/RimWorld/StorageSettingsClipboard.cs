using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public static class StorageSettingsClipboard
	{
		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x00199256 File Offset: 0x00197456
		public static bool HasCopiedSettings
		{
			get
			{
				return StorageSettingsClipboard.copied;
			}
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x0019925D File Offset: 0x0019745D
		public static void Copy(StorageSettings s)
		{
			StorageSettingsClipboard.clipboard.CopyFrom(s);
			StorageSettingsClipboard.copied = true;
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x00199270 File Offset: 0x00197470
		public static void PasteInto(StorageSettings s)
		{
			s.CopyFrom(StorageSettingsClipboard.clipboard);
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x0019927D File Offset: 0x0019747D
		public static IEnumerable<Gizmo> CopyPasteGizmosFor(StorageSettings s)
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings", true),
				defaultLabel = "CommandCopyZoneSettingsLabel".Translate(),
				defaultDesc = "CommandCopyZoneSettingsDesc".Translate(),
				action = delegate
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					StorageSettingsClipboard.Copy(s);
				},
				hotKey = KeyBindingDefOf.Misc4
			};
			Command_Action command_Action = new Command_Action();
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings", true);
			command_Action.defaultLabel = "CommandPasteZoneSettingsLabel".Translate();
			command_Action.defaultDesc = "CommandPasteZoneSettingsDesc".Translate();
			command_Action.action = delegate
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				StorageSettingsClipboard.PasteInto(s);
			};
			command_Action.hotKey = KeyBindingDefOf.Misc5;
			if (!StorageSettingsClipboard.HasCopiedSettings)
			{
				command_Action.Disable(null);
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x04002AE4 RID: 10980
		private static StorageSettings clipboard = new StorageSettings();

		// Token: 0x04002AE5 RID: 10981
		private static bool copied = false;
	}
}
