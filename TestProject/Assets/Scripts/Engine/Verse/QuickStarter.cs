﻿using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	
	public static class QuickStarter
	{
		
		public static bool CheckQuickStart()
		{
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				return true;
			}
			return false;
		}

		
		private static bool quickStarted;
	}
}
