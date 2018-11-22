// ##############################################################################
//
// ice_CreatureDisplayOptions.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{
	public static class GlobalDisplayData
	{
		public static bool UseGlobalAll = false;

		public static bool ShowOptions = false;
		public static bool ShowHelp = false;
		public static bool ShowHelpDescription = false;
		public static bool ShowHelpTarget = false;
		public static bool ShowHelpBehaviour = false;
		public static bool ShowDebug = false;
		public static bool ShowInfo = false;

		public static bool ShowEssentials = false;
		public static bool ShowStatus = false;
		public static bool ShowStatusAdvanced = false;

		public static bool ShowMissions = false;
		public static bool ShowMissionsOutpost = false;
		public static bool ShowMissionsEscort = false;
		public static bool ShowMissionsPatrol = false;
		
		public static bool ShowEnvironment = false;
		public static bool ShowInteraction = false;

		public static bool ShowBehaviour = false;
		public static bool ShowBehaviourMove = false;
		public static bool ShowBehaviourAudio = false;
		public static bool ShowBehaviourInluences = false;
		public static bool ShowBehaviourEffect = false;
		public static bool ShowBehaviourLink = false;
		
		public static bool ShowBeta = false;

		public static bool FoldoutWizard = true;
		public static bool FoldoutEssentials = true;
		public static bool FoldoutEssentialsHome = true;
		public static bool FoldoutEssentialsBehaviours = true;
		public static bool FoldoutEssentialsMotion = true;
		public static bool FoldoutEssentialsBodyParts = true;
		public static bool FoldoutEssentialsSystem = true;
		public static bool FoldoutBehaviours = true;
		public static bool FoldoutMissions = true;
		public static bool FoldoutMissionOutpost = true;
		public static bool FoldoutMissionEscort = true;
		public static bool FoldoutMissionPatrol = true;
		public static bool FoldoutMemory = true;
		public static bool FoldoutStatus = true;
		public static bool FoldoutStatusVital = true;
		public static bool FoldoutStatusCharacter = true;
		public static bool FoldoutStatusSensory = true;
		public static bool FoldoutStatusInfluence = true;
		public static bool FoldoutStatusBasics = true;
		public static bool FoldoutStatusAdvanced = true;
		public static bool FoldoutStatusMemory = true;
		public static bool FoldoutStatusInfos = true;
		public static bool FoldoutStatusDynamicInfluences = true;

		public static bool FoldoutInteraction = true;
		public static bool FoldoutEnvironment = true;
		public static bool FoldoutStatusInventory = true;

	}


	[System.Serializable]
	public class DisplayData
	{
		public bool UseGlobal{
			get{ return ( GlobalDisplayData.UseGlobalAll ? true : false ); }
		}

		[SerializeField]
		//private bool m_UseGlobalAll = false;
		public bool UseGlobalAll{
			set{ 
				if( GlobalDisplayData.UseGlobalAll =! value )
				{
					if( value )
						SetGlobalToLocal();
					else
						SetLocalToGlobal();
				}

				GlobalDisplayData.UseGlobalAll = value;
			}
			get{ return GlobalDisplayData.UseGlobalAll; }
		}

		public void SetLocalToGlobal()
		{
			GlobalDisplayData.ShowOptions = m_ShowOptions;
			GlobalDisplayData.ShowHelp = m_ShowHelp;
			GlobalDisplayData.ShowInfo = m_ShowInfo;
			GlobalDisplayData.ShowDebug = m_ShowDebug;

			GlobalDisplayData.ShowEssentials = m_ShowEssentials;

			GlobalDisplayData.ShowStatus = m_ShowStatus;
			GlobalDisplayData.ShowMissions = m_ShowMissions;
			GlobalDisplayData.ShowBehaviour = m_ShowBehaviour;

			GlobalDisplayData.ShowEnvironment = m_ShowEnvironment;
			
		

			GlobalDisplayData.ShowInteraction = m_ShowInteraction;



			GlobalDisplayData.FoldoutEssentials = m_FoldoutEssentials;
			GlobalDisplayData.FoldoutEssentialsHome = m_FoldoutEssentialsHome;
			GlobalDisplayData.FoldoutEssentialsBehaviours = m_FoldoutEssentialsBehaviours;
			GlobalDisplayData.FoldoutEssentialsMotion = m_FoldoutEssentialsMotion;
			GlobalDisplayData.FoldoutEssentialsBodyParts = m_FoldoutEssentialsBodyParts;
			GlobalDisplayData.FoldoutEssentialsSystem = m_FoldoutEssentialsSystem;

			GlobalDisplayData.FoldoutBehaviours = m_FoldoutBehaviours;

			GlobalDisplayData.FoldoutStatus = m_FoldoutStatus;
			GlobalDisplayData.FoldoutStatusVital = m_FoldoutStatusVital;
			GlobalDisplayData.FoldoutStatusCharacter = m_FoldoutStatusCharacter;
			GlobalDisplayData.FoldoutStatusBasics = m_FoldoutStatusBasics;
			GlobalDisplayData.FoldoutStatusAdvanced = m_FoldoutStatusAdvanced;
			GlobalDisplayData.FoldoutStatusSensory = m_FoldoutStatusSensory;
			GlobalDisplayData.FoldoutStatusInfluence = m_FoldoutStatusInfluence;
			GlobalDisplayData.FoldoutStatusInfos = m_FoldoutStatusInfos;
			GlobalDisplayData.FoldoutStatusDynamicInfluences = m_FoldoutStatusDynamicInfluences;
			GlobalDisplayData.FoldoutStatusMemory = m_FoldoutStatusMemory;

			GlobalDisplayData.FoldoutInteraction = m_FoldoutInteraction;

			GlobalDisplayData.FoldoutMissions = m_FoldoutMissions;
	
			GlobalDisplayData.FoldoutEnvironment = m_FoldoutEnvironment;

			GlobalDisplayData.FoldoutStatusInventory = m_FoldoutStatusInventory;
		}

		public void SetGlobalToLocal()
		{
			m_ShowOptions = GlobalDisplayData.ShowOptions;
			m_ShowHelp = GlobalDisplayData.ShowHelp;
			m_ShowInfo = GlobalDisplayData.ShowInfo;
			m_ShowDebug = GlobalDisplayData.ShowDebug;

			m_ShowEssentials = GlobalDisplayData.ShowEssentials;
			m_ShowStatus = GlobalDisplayData.ShowStatus;
			m_ShowEnvironment = GlobalDisplayData.ShowEnvironment;			
			m_ShowMissions = GlobalDisplayData.ShowMissions;
			m_ShowInteraction = GlobalDisplayData.ShowInteraction;
			m_ShowBehaviour = GlobalDisplayData.ShowBehaviour;

			m_FoldoutEssentials = GlobalDisplayData.FoldoutEssentials;
			m_FoldoutEssentialsHome = GlobalDisplayData.FoldoutEssentialsHome;
			m_FoldoutEssentialsBehaviours = GlobalDisplayData.FoldoutEssentialsBehaviours;
			m_FoldoutEssentialsMotion = GlobalDisplayData.FoldoutEssentialsMotion;
			m_FoldoutEssentialsBodyParts = GlobalDisplayData.FoldoutEssentialsBodyParts;
			m_FoldoutEssentialsSystem = GlobalDisplayData.FoldoutEssentialsSystem;
			m_FoldoutBehaviours = GlobalDisplayData.FoldoutBehaviours;
			m_FoldoutStatus = GlobalDisplayData.FoldoutStatus;
			m_FoldoutStatusVital = GlobalDisplayData.FoldoutStatusVital;
			m_FoldoutStatusCharacter = GlobalDisplayData.FoldoutStatusCharacter;
			m_FoldoutStatusSensory = GlobalDisplayData.FoldoutStatusSensory;
			m_FoldoutStatusBasics = GlobalDisplayData.FoldoutStatusBasics;
			m_FoldoutStatusAdvanced = GlobalDisplayData.FoldoutStatusAdvanced;
			m_FoldoutStatusInfos = GlobalDisplayData.FoldoutStatusInfos;
			m_FoldoutStatusInfluence = GlobalDisplayData.FoldoutStatusInfluence;
			m_FoldoutStatusMemory = GlobalDisplayData.FoldoutStatusMemory;
			m_FoldoutInteraction = GlobalDisplayData.FoldoutInteraction;
			m_FoldoutEnvironment = GlobalDisplayData.FoldoutEnvironment;
			m_FoldoutMissions = GlobalDisplayData.FoldoutMissions;
			m_FoldoutStatusDynamicInfluences = GlobalDisplayData.FoldoutStatusDynamicInfluences;
			m_FoldoutStatusInventory = GlobalDisplayData.FoldoutStatusInventory;
		}

		[SerializeField]
		private bool m_ShowOptions = false;
		public bool ShowOptions{
			set{ if( UseGlobal ) GlobalDisplayData.ShowOptions = value; else m_ShowOptions = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowOptions; else return m_ShowOptions; }
		}

		[SerializeField]
		private bool m_ShowHelp = false;
		public bool ShowHelp{
			set{ if( UseGlobal ) GlobalDisplayData.ShowHelp = value; else m_ShowHelp = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowHelp; else return m_ShowHelp; }
		}

		[SerializeField]
		private bool m_ShowDebug = false;
		public bool ShowDebug {
			set{ if( UseGlobal ) GlobalDisplayData.ShowDebug = value; else m_ShowDebug = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowDebug; else return m_ShowDebug; }
		}

		[SerializeField]
		private bool m_ShowInfo = false;
		public bool ShowInfo {
			set{ if( UseGlobal ) GlobalDisplayData.ShowInfo = value; else m_ShowInfo = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowInfo; else return m_ShowInfo; }
		}


		[SerializeField]
		private bool m_ShowEssentials = false;
		public bool ShowEssentials{
			set{ if( UseGlobal ) GlobalDisplayData.ShowEssentials = value; else m_ShowEssentials = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowEssentials; else return m_ShowEssentials; }
		}
			
		[SerializeField]
		private bool m_ShowEnvironment = false;
		public bool ShowEnvironment {
			set{ if( UseGlobal ) GlobalDisplayData.ShowEnvironment  = value; else m_ShowEnvironment  = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowEnvironment ; else return m_ShowEnvironment ; }
		}
			
		[SerializeField]
		private bool m_ShowStatus = false;
		public bool ShowStatus{
			set{ if( UseGlobal ) GlobalDisplayData.ShowStatus = value; else m_ShowStatus = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowStatus; else return m_ShowStatus; }
		}

		[SerializeField]
		private bool m_ShowMissions = false;
		public bool ShowMissions{
			set{ if( UseGlobal ) GlobalDisplayData.ShowMissions = value; else m_ShowMissions = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowMissions; else return m_ShowMissions; }
		}
			
		[SerializeField]
		private bool m_ShowInteraction = false;
		public bool ShowInteraction{
			set{ if( UseGlobal ) GlobalDisplayData.ShowInteraction = value; else m_ShowInteraction = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowInteraction; else return m_ShowInteraction; }
		}

		[SerializeField]
		private bool m_ShowBehaviour = false;
		public bool ShowBehaviour{
			set{ if( UseGlobal ) GlobalDisplayData.ShowBehaviour = value; else m_ShowBehaviour = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.ShowBehaviour; else return m_ShowBehaviour; }
		}

		/// <summary>
		/// FOLDOUTS
		/// </summary>

		[SerializeField]
		private bool m_FoldoutEssentials = true;
		public bool FoldoutEssentials{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEssentials = value; else m_FoldoutEssentials = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEssentials; else return m_FoldoutEssentials; }
		}

		[SerializeField]
		private bool m_FoldoutEssentialsHome = false;
		public bool FoldoutEssentialsHome{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEssentialsHome = value; else m_FoldoutEssentialsHome = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEssentialsHome; else return m_FoldoutEssentialsHome; }
		}

		[SerializeField]
		private bool m_FoldoutEssentialsBehaviours = false;
		public bool FoldoutEssentialsBehaviours{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEssentialsBehaviours = value; else m_FoldoutEssentialsBehaviours = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEssentialsBehaviours; else return m_FoldoutEssentialsBehaviours; }
		}

		[SerializeField]
		private bool m_FoldoutEssentialsMotion = false;
		public bool FoldoutEssentialsMotion{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEssentialsMotion = value; else m_FoldoutEssentialsMotion = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEssentialsMotion; else return m_FoldoutEssentialsMotion; }
		}

		[SerializeField]
		private bool m_FoldoutEssentialsBodyParts = false;
		public bool FoldoutEssentialsBodyParts{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEssentialsBodyParts = value; else m_FoldoutEssentialsBodyParts = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEssentialsBodyParts; else return m_FoldoutEssentialsBodyParts; }
		}

		[SerializeField]
		private bool m_FoldoutEssentialsSystem = false;
		public bool FoldoutEssentialsSystem{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEssentialsSystem = value; else m_FoldoutEssentialsSystem = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEssentialsSystem; else return m_FoldoutEssentialsSystem; }
		}

		[SerializeField]
		private bool m_FoldoutBehaviours = true;
		public bool FoldoutBehaviours{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutBehaviours = value; else m_FoldoutBehaviours = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutBehaviours; else return m_FoldoutBehaviours; }
		}

		[SerializeField]
		private bool m_FoldoutMissions = true;
		public bool FoldoutMissions{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutMissions = value; else m_FoldoutMissions = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutMissions; else return m_FoldoutMissions; }
		}

		[SerializeField]
		private bool m_FoldoutStatus = true;
		public bool FoldoutStatus{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatus = value; else m_FoldoutStatus = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatus; else return m_FoldoutStatus; }
		}

		[SerializeField]
		private bool m_FoldoutStatusVital = true;
		public bool FoldoutStatusVital{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusVital = value; else m_FoldoutStatusVital = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusVital; else return m_FoldoutStatusVital; }
		}

		[SerializeField]
		private bool m_FoldoutStatusCharacter = true;
		public bool FoldoutStatusCharacter{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusCharacter = value; else m_FoldoutStatusCharacter = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusCharacter; else return m_FoldoutStatusCharacter; }
		}

		[SerializeField]
		private bool m_FoldoutStatusSensory = true;
		public bool FoldoutStatusSensory{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusSensory = value; else m_FoldoutStatusSensory = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusSensory; else return m_FoldoutStatusSensory; }
		}

		[SerializeField]
		private bool m_FoldoutStatusInfos = true;
		public bool FoldoutStatusInfos{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusInfos = value; else m_FoldoutStatusInfos = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusInfos; else return m_FoldoutStatusInfos; }
		}

		[SerializeField]
		private bool m_FoldoutStatusBasics = true;
		public bool FoldoutStatusBasics{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusBasics = value; else m_FoldoutStatusBasics = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusBasics; else return m_FoldoutStatusBasics; }
		}

		[SerializeField]
		private bool m_FoldoutStatusAdvanced = true;
		public bool FoldoutStatusAdvanced {
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusAdvanced  = value; else m_FoldoutStatusAdvanced  = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusAdvanced ; else return m_FoldoutStatusAdvanced; }
		}

		[SerializeField]
		private bool m_FoldoutStatusInfluence = true;
		public bool FoldoutStatusInfluence{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusInfluence = value; else m_FoldoutStatusInfluence = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusInfluence; else return m_FoldoutStatusInfluence; }
		}

		[SerializeField]
		private bool m_FoldoutStatusMemory = true;
		public bool FoldoutStatusMemory{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusMemory = value; else m_FoldoutStatusMemory = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusMemory; else return m_FoldoutStatusMemory; }
		}

		[SerializeField]
		private bool m_FoldoutStatusDynamicInfluences = true;
		public bool FoldoutStatusDynamicInfluences{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusDynamicInfluences = value; else m_FoldoutStatusDynamicInfluences = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusDynamicInfluences; else return m_FoldoutStatusDynamicInfluences; }
		}

		[SerializeField]
		private bool m_FoldoutStatusInventory = true;
		public bool FoldoutStatusInventory{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutStatusInventory = value; else m_FoldoutStatusInventory = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutStatusInventory; else return m_FoldoutStatusInventory; }
		}

		[SerializeField]
		private bool m_FoldoutInteraction = true;
		public bool FoldoutInteraction{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutInteraction = value; else m_FoldoutInteraction = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutInteraction; else return m_FoldoutInteraction; }
		}

		[SerializeField]
		private bool m_FoldoutEnvironment = true;
		public bool FoldoutEnvironment{
			set{ if( UseGlobal ) GlobalDisplayData.FoldoutEnvironment = value; else m_FoldoutEnvironment = value; }
			get{ if( UseGlobal ) return GlobalDisplayData.FoldoutEnvironment; else return m_FoldoutEnvironment; }
		}



	}
}
