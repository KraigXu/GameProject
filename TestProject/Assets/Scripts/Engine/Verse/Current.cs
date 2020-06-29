using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	
	public static class Current
	{
		
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00025677 File Offset: 0x00023877
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0002567E File Offset: 0x0002387E
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00025685 File Offset: 0x00023885
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0002568C File Offset: 0x0002388C
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00025693 File Offset: 0x00023893
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002569A File Offset: 0x0002389A
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x000256A1 File Offset: 0x000238A1
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x000256A8 File Offset: 0x000238A8
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x000256AF File Offset: 0x000238AF
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x000256B7 File Offset: 0x000238B7
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x000256BE File Offset: 0x000238BE
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x000256C6 File Offset: 0x000238C6
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x000256CD File Offset: 0x000238CD
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
				return;
			}
			if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}

		
		private static ProgramState programStateInt;

		
		private static Root rootInt;

		
		private static Root_Entry rootEntryInt;

		
		private static Root_Play rootPlayInt;

		
		private static Camera cameraInt;

		
		private static CameraDriver cameraDriverInt;

		
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		
		private static SubcameraDriver subcameraDriverInt;

		
		private static Game gameInt;

		
		private static World creatingWorldInt;
	}
}
