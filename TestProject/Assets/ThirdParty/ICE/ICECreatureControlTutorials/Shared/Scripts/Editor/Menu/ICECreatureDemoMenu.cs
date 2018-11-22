// ##############################################################################
//
// ICECreatureDemoMenu.cs
// Version 1.3.6
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
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif


using ICE;
using ICE.World;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.Windows;

namespace ICE.Creatures.Menus
{
	public class ICECreatureDemoMenu : MonoBehaviour {

		public ICECreatureDemoMenu(){

			#if UNITY_EDITOR

			if( ! Application.isPlaying )
			{
				if( LayerMask.NameToLayer( "WalkableSurface" ) == -1 )
					ICE.World.EditorUtilities.EditorTools.AddLayer( "WalkableSurface" );

				if( LayerMask.NameToLayer( "Obstacle" ) == -1 )
					ICE.World.EditorUtilities.EditorTools.AddLayer( "Obstacle" );
			}

			#endif
		}

		[MenuItem ( "ICE/ICE Creature Control/Demos/Assign Demo Layer", false, 1966 )]
		static void AssignDemoLayer() 
		{
			if( ! Application.isPlaying )
			{
				if( LayerMask.NameToLayer( "WalkableSurface" ) == -1 )
					ICE.World.EditorUtilities.EditorTools.AddLayer( "WalkableSurface" );

				if( LayerMask.NameToLayer( "Obstacle" ) == -1 )
					ICE.World.EditorUtilities.EditorTools.AddLayer( "Obstacle" );
			}
		}

		// BASICS

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Static Targets' Tutorial", false, 19771 )]
		static void TutorialStaticTarget(){
			LoadDemo( "TutorialStaticTarget" );
		}

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Moveable Targets' Tutorial", false, 19772 )]
		static void TutorialMoveableTarget(){
			LoadDemo( "TutorialMoveableTarget" );
		}

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Move Examples' Tutorial", false, 19773 )]
		static void TutorialMoveExamples(){
			LoadDemo( "TutorialMoveExamples" );
		}

		// BASICS PLUS

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Avoid Fire' Tutorial", false, 19781 )]
		static void TutorialAvoidFire(){
			LoadDemo( "TutorialAvoidFire" );
		}

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Gatherers' Tutorial", false, 19782 )]
		static void TutorialGatherers(){
			LoadDemo( "TutorialGatherers" );
		}

		// MECHANIM

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Mecanim Locomotion' Tutorial", false, 19790 )]
		static void TutorialMecanimLocomotion(){
			LoadDemo( "TutorialMecanimLocomotion" );
		}

		// PATHFINDING


		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Avoidance' Tutorial", false, 19791 )]
		static void TutorialAvoidance(){
			LoadDemo( "TutorialAvoidance" );
		}

		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Navigation Mesh' Tutorial", false, 19792 )]
		static void TutorialNavMesh(){
			LoadDemo( "TutorialNavMesh" );
		}


		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'NavMesh House' Tutorial", false, 19793 )]
		static void TutorialNavMeshHouse(){
			LoadDemo( "TutorialNavMeshHouse" );
		}


		[MenuItem ( "ICE/ICE Creature Control/Demos/Open 'Deathmatch' Tutorial", false, 19794 )]
		static void TutorialDeathmatch(){
			LoadDemo( "TutorialDeathmatch" );
		}

		private static void LoadDemo( string _scene ) 
		{
			string _path = "Assets/ICE/ICECreatureControlTutorials/Tutorials/" + _scene + "/" + _scene + ".unity";

			ICEDebug.LogInfo( "Open Tutorial Scene : " + _path );

			#if UNITY_5_3 || UNITY_5_3_OR_NEWER
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				EditorSceneManager.OpenScene( _path );
			#else
				EditorApplication.OpenScene( _path );
			#endif
		


		}
	}
}