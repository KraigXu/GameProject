// ##############################################################################
//
// ice_CreatureIO.cs
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

using UnityEditor;

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.Utilities;

namespace ICE.Creatures.EditorUtilities
{
	[System.Serializable]
	public class CreatureEditorIO : ICEEditorWorldIO
	{
		/// <summary>
		/// Saves the creature to file.
		/// </summary>
		/// <param name="status">Status.</param>
		public static void SaveCreatureToFile( CreatureObject _object, string owner  )
		{
			m_Path = Save( owner, "cc_preset" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<CreatureObject>( _object );
		}

		/// <summary>
		/// Loads the creature from file.
		/// </summary>
		/// <returns>The status from file.</returns>
		/// <param name="status">Status.</param>
		public static CreatureObject LoadCreatureFromFile( CreatureObject _object )
		{
			m_Path = Load( "cc_preset" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<CreatureObject>( _object );
		}

		/// <summary>
		/// Saves the environment to file.
		/// </summary>
		/// <param name="environment">Environment.</param>
		public static void SaveEnvironmentToFile( EnvironmentObject _object, string owner  )
		{
			m_Path = Save( owner, "cc_environment" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<EnvironmentObject>( _object );
		}

		/// <summary>
		/// Loads the environment from file.
		/// </summary>
		/// <returns>The status from file.</returns>
		/// <param name="environment">Environment.</param>
		public static EnvironmentObject LoadEnvironmentFromFile( EnvironmentObject _object )
		{
			m_Path = Load( "cc_environment" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<EnvironmentObject>( _object );
		}

		/// <summary>
		/// Saves the environment to file.
		/// </summary>
		/// <param name="environment">Environment.</param>
		public static void SaveEnvironmentCollisionToFile( CollisionObject _object, string owner  )
		{
			m_Path = Save( owner, "cc_collision" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<CollisionObject>( _object );
		}

		/// <summary>
		/// Loads the environment from file.
		/// </summary>
		/// <returns>The status from file.</returns>
		/// <param name="environment">Environment.</param>
		public static CollisionObject LoadEnvironmentCollisionFromFile( CollisionObject _object )
		{
			m_Path = Load( "cc_collision" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<CollisionObject>( _object );
		}

		/// <summary>
		/// Saves the environment to file.
		/// </summary>
		/// <param name="environment">Environment.</param>
		public static void SaveEnvironmentSurfaceToFile( SurfaceObject _object, string owner  )
		{
			m_Path = Save( owner, "cc_surface" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<SurfaceObject>( _object );
		}

		/// <summary>
		/// Loads the environment from file.
		/// </summary>
		/// <returns>The status from file.</returns>
		/// <param name="environment">Environment.</param>
		public static SurfaceObject LoadEnvironmentSurfaceFromFile( SurfaceObject _object )
		{
			m_Path = Load( "cc_surface" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<SurfaceObject>( _object );
		}

		/// <summary>
		/// Saves the status to file.
		/// </summary>
		/// <param name="status">Status.</param>
		public static void SaveStatusToFile( StatusObject _object, string owner  )
		{
			m_Path = Save( owner, "cc_status" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<StatusObject>( _object );
		}

		/// <summary>
		/// Loads the status from file.
		/// </summary>
		/// <returns>The status from file.</returns>
		/// <param name="status">Status.</param>
		public static StatusObject LoadStatusFromFile( StatusObject _object )
		{
			m_Path = Load( "cc_status" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<StatusObject>( _object );
		}

		/// <summary>
		/// Saves the memory to file.
		/// </summary>
		/// <param name="memory">Memory.</param>
		public static void SaveMemoryToFile( MemoryObject _object, string owner  )
		{
			m_Path = Save( owner, "cc_memory" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<MemoryObject>( _object );
		}

		/// <summary>
		/// Loads the memory from file.
		/// </summary>
		/// <returns>The memory from file.</returns>
		/// <param name="status">Memory.</param>
		public static MemoryObject LoadMemoryFromFile( MemoryObject _object )
		{
			m_Path = Load( "cc_memory" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<MemoryObject>( _object );			
		}

		/// <summary>
		/// Saves the behaviour to file.
		/// </summary>
		/// <param name="status">Status.</param>
		public static void SaveBehaviourToFile( BehaviourObject _object, string owner )
		{
			m_Path = Save( owner, "cc_behaviours" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<BehaviourObject>( _object );
		}

		/// <summary>
		/// Loads the behaviour from file.
		/// </summary>
		/// <returns>The behaviour from file.</returns>
		/// <param name="behaviour">Behaviour.</param>
		public static BehaviourObject LoadBehaviourFromFile( BehaviourObject _object )
		{
			m_Path = Load( "cc_behaviours" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<BehaviourObject>( _object );	
		}





		public static void SaveEssentialsToFile( EssentialsDataObject _object, string owner )
		{
			m_Path = Save( owner, "cc_essentials" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<EssentialsDataObject>( _object );
		}

		/// <summary>
		/// Loads the behaviour from file.
		/// </summary>
		/// <returns>The behaviour from file.</returns>
		/// <param name="behaviour">Behaviour.</param>
		public static EssentialsDataObject LoadEssentialsFromFile( EssentialsDataObject _object )
		{
			m_Path = Load( "cc_essentials" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<EssentialsDataObject>( _object );	
		}




		/// <summary>
		/// Saves an escort mission to file.
		/// </summary>
		/// <param name="escort">Escort.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveMissionEscortToFile( EscortObject _object, string owner )
		{
			m_Path = Save( owner, "cc_escort" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<EscortObject>( _object );
		}

		/// <summary>
		/// Loads an escort mission from file.
		/// </summary>
		/// <returns>The mission escort from file.</returns>
		/// <param name="escort">Escort.</param>
		public static EscortObject LoadMissionEscortFromFile( EscortObject _object )
		{
			m_Path = Load( "cc_escort" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<EscortObject>( _object );				
		}

		/// <summary>
		/// Saves an outpost mission to file.
		/// </summary>
		/// <param name="outpost">Outpost.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveMissionOutpostToFile( OutpostObject _object, string owner )
		{
			m_Path = Save( owner, "cc_outpost" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<OutpostObject>( _object );
		}

		/// <summary>
		/// Loads an outpost mission from file.
		/// </summary>
		/// <returns>The mission outpost from file.</returns>
		/// <param name="outpost">Outpost.</param>
		public static OutpostObject LoadMissionOutpostFromFile( OutpostObject _object )
		{
			m_Path = Load( "cc_outpost" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<OutpostObject>( _object );	
		}

		/// <summary>
		/// Saves an patrol mission to file.
		/// </summary>
		/// <param name="patrol">Patrol.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveMissionPatrolToFile( PatrolObject _object, string owner )
		{
			m_Path = Save( owner, "cc_patrol" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<PatrolObject>( _object );
		}

		/// <summary>
		/// Loads an patrol mission from file.
		/// </summary>
		/// <returns>The mission patrol from file.</returns>
		/// <param name="patrol">Patrol.</param>
		public static PatrolObject LoadMissionPatrolFromFile( PatrolObject _object )
		{
			m_Path = Load( "cc_patrol" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<PatrolObject>( _object );		
		}

		/// <summary>
		/// Saves the interaction settings to file.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveInteractionToFile( InteractionObject _object, string owner )
		{
			m_Path = Save( owner, "cc_interactions" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<InteractionObject>( _object );
		}

		/// <summary>
		/// Loads the interaction settings from file.
		/// </summary>
		/// <returns>The interaction from file.</returns>
		/// <param name="_object">Object.</param>
		public static InteractionObject LoadInteractionFromFile( InteractionObject _object )
		{
			m_Path = Load( "cc_interactions" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<InteractionObject>( _object );	
		}

		/// <summary>
		/// Saves an interactor object to file.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveInteractorToFile( InteractorObject _object, string owner )
		{
			m_Path = Save( owner, "cc_interactor" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<InteractorObject>( _object );
		}

		/// <summary>
		/// Loads an interactor object from file.
		/// </summary>
		/// <returns>The interactor from file.</returns>
		/// <param name="_object">Object.</param>
		public static InteractorObject LoadInteractorFromFile( InteractorObject _object )
		{
			m_Path = Load( "cc_interactor" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<InteractorObject>( _object );
		}


		public static void SaveBehaviourModeToFile( BehaviourModeObject _object, string owner )
		{
			m_Path = Save( owner, "cc_behaviour_mode" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<BehaviourModeObject>( _object );
		}


		public static BehaviourModeObject LoadBehaviourModeFromFile( BehaviourModeObject _object )
		{
			m_Path = Load( "cc_behaviour_mode" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<BehaviourModeObject>( _object );
		}



		/// <summary>
		/// Saves the audio container to file.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveAudioContainerToFile( AudioDataObject _object, string owner )
		{
			m_Path = Save( owner, "audio" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<AudioDataObject>( _object );
		}

		/// <summary>
		/// Loads the audio containern from file.
		/// </summary>
		/// <returns>The audio containern from file.</returns>
		/// <param name="_object">Object.</param>
		public static AudioDataObject LoadAudioContainernFromFile( AudioDataObject _object )
		{
			m_Path = Load( "audio" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<AudioDataObject>( _object );
		}
	}
}
