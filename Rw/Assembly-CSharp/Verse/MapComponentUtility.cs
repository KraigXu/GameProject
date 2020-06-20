using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000182 RID: 386
	public static class MapComponentUtility
	{
		// Token: 0x06000B38 RID: 2872 RVA: 0x0003C0F4 File Offset: 0x0003A2F4
		public static void MapComponentUpdate(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0003C148 File Offset: 0x0003A348
		public static void MapComponentTick(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003C19C File Offset: 0x0003A39C
		public static void MapComponentOnGUI(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0003C1F0 File Offset: 0x0003A3F0
		public static void FinalizeInit(Map map)
		{
			List<MapComponent> components = map.components;
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

		// Token: 0x06000B3C RID: 2876 RVA: 0x0003C244 File Offset: 0x0003A444
		public static void MapGenerated(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapGenerated();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003C298 File Offset: 0x0003A498
		public static void MapRemoved(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapRemoved();
				}
				catch (Exception arg)
				{
					Log.Error("Could not notify map component: " + arg, false);
				}
			}
		}
	}
}
