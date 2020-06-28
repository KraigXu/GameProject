using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000111 RID: 273
	public static class GameComponentUtility
	{
		// Token: 0x060007A2 RID: 1954 RVA: 0x00023904 File Offset: 0x00021B04
		public static void GameComponentUpdate()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002395C File Offset: 0x00021B5C
		public static void GameComponentTick()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000239B4 File Offset: 0x00021BB4
		public static void GameComponentOnGUI()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00023A0C File Offset: 0x00021C0C
		public static void FinalizeInit()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00023A64 File Offset: 0x00021C64
		public static void StartedNewGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].StartedNewGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00023ABC File Offset: 0x00021CBC
		public static void LoadedGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].LoadedGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}
	}
}
