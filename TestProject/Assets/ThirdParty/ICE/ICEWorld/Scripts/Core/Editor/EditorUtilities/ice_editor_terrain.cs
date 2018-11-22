// ##############################################################################
//
// ice_editor_terrain.cs | ICEEditorLayout
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
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	public class ICEEditorTerrain
	{
		private static Vector3 GetRandomGroundPosition( GameObject _ground, float _base_offset = 0 )
		{
			Vector3 _size = Vector3.zero;
			Vector3 _pos = Vector3.zero;

			if( _ground != null )
			{
				if( _ground.GetComponent<Terrain>() && _ground.GetComponent<Terrain>().terrainData )
				{
					TerrainData _terrain_data = _ground.GetComponent<Terrain>().terrainData;

					_size = _terrain_data.size;
					_pos = new Vector3( UnityEngine.Random.Range( 0,_size.x ), 0, UnityEngine.Random.Range( 0,_size.z ) ) ;
				}
				else
				{
					_size = _ground.transform.localScale * 5;
					_pos = new Vector3( UnityEngine.Random.Range( -_size.x,_size.x ), 0, UnityEngine.Random.Range( -_size.z,_size.z ) ) ;
				}

				_pos += _ground.transform.position;
			}

			_pos.y = WorldManager.GetGroundLevel( _pos, _base_offset ) + _base_offset;

			return _pos;
		}

		/// <summary>
		/// Updates the level.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_ground">Ground.</param>
		public static void UpdateLevel( GameObject _object, GameObject _ground )
		{
			if( _object != null )
				_object.transform.position = GetRandomGroundPosition( _ground, 2 );
		}

		/// <summary>
		/// Draws the scene hills.
		/// </summary>
		/// <param name="_ground">Ground.</param>
		/// <param name="_player">Player.</param>
		public static void DrawSceneHills( GameObject _ground, GameObject _player )
		{
			if( _ground == null )
				return;
			
			ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();
			TerrainTools.TerrainHills = (int)ICEEditorLayout.DefaultSlider( "Hills (max.)", "", TerrainTools.TerrainHills , 1, 0, 500, TerrainTools.DefaultTerrainHills );
			EditorGUI.BeginDisabledGroup( TerrainTools.TerrainHills == 0 || _ground == null || _ground.GetComponent<Terrain>() == null  );		
			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.CreateHills( _ground.GetComponent<Terrain>().terrainData );
					TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
					UpdateLevel( _player, _ground );
				}
			}
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_HILLS );
			EditorGUI.BeginDisabledGroup( TerrainTools.TerrainHills == 0 );
			EditorGUI.indentLevel++;

			int _terrain_height = TerrainTools.GetTerrainHeight( _ground );
			int _terrain_size = TerrainTools.GetTerrainSize( _ground );

			float _height = (int)MathTools.Denormalize( TerrainTools.TerrainHillsMaxHeight, 0, _terrain_height );
			float _radius = (int)MathTools.Denormalize( TerrainTools.TerrainHillsMaxRadius * 2, 0, _terrain_size * 0.5f );
			float _radius_min = MathTools.Normalize( _height, 0, _terrain_size ) ;

			TerrainTools.TerrainHillsMaxHeight = ICEEditorLayout.DefaultSlider( "Max. Height (" + _height + ")", "", TerrainTools.TerrainHillsMaxHeight , Init.DECIMAL_PRECISION, 0, 1, TerrainTools.DefaultTerrainHillsMaxHeight * TerrainTools.GetSizeMultiplier() );
			TerrainTools.TerrainHillsMaxRadius = ICEEditorLayout.DefaultSlider( "Max. Radius (" + _radius + ")", "", TerrainTools.TerrainHillsMaxRadius , Init.DECIMAL_PRECISION, _radius_min, 1, TerrainTools.DefaultTerrainHillsMaxRadius  * TerrainTools.GetSizeMultiplier() );
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
		}

		private static bool _update_required = false;

		/// <summary>
		/// Draws the scene terrain.
		/// </summary>
		/// <param name="_ground">Ground.</param>
		/// <param name="_player">Player.</param>
		public static void DrawSceneTerrain( GameObject _ground, GameObject _player )
		{
			ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();

			TerrainTools.DesiredTerrainSize = (int)ICEEditorLayout.Slider( "Terrain Size", "", TerrainTools.DesiredTerrainSize , 100, 100, 2000 );

			TerrainTools.DesiredTerrainResolutionMultiplier = (int)ICEEditorLayout.ButtonOptionMini( "1", TerrainTools.DesiredTerrainResolutionMultiplier, 1 );
			TerrainTools.DesiredTerrainResolutionMultiplier = (int)ICEEditorLayout.ButtonOptionMini( "2", TerrainTools.DesiredTerrainResolutionMultiplier, 2 );
			TerrainTools.DesiredTerrainResolutionMultiplier = (int)ICEEditorLayout.ButtonOptionMini( "4", TerrainTools.DesiredTerrainResolutionMultiplier, 4 );
			TerrainTools.DesiredTerrainResolutionMultiplier = (int)ICEEditorLayout.ButtonOptionMini( "8", TerrainTools.DesiredTerrainResolutionMultiplier, 8 );

			int _desired_value = TerrainTools.DesiredTerrainSize + TerrainTools.DesiredTerrainResolutionMultiplier;
			int _current_value = TerrainTools.TerrainSize + TerrainTools.TerrainResolutionMultiplier;
			int _default_value = TerrainTools.DefaultTerrainSize + TerrainTools.DefaultTerrainResolutionMultiplier;

			if( _desired_value != _default_value )
				_update_required = true;

			if( ICEEditorLayout.ButtonDefault( _desired_value, _default_value ) == _default_value )
			{
				TerrainTools.DesiredTerrainSize = TerrainTools.DefaultTerrainSize;
				TerrainTools.DesiredTerrainResolutionMultiplier = TerrainTools.DefaultTerrainResolutionMultiplier;
			}

			if( ICEEditorLayout.UpdateButton( _desired_value != _current_value ) )
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");

					if( _update_required )
					{
						_update_required = false;
						GameObject.DestroyImmediate( _ground );

						_ground = TerrainTools.CreateTerrain();
						if( _ground != null && _ground.GetComponent<Terrain>() != null )
						{
							Vector3 _s = _ground.GetComponent<Terrain>().terrainData.size;
							_ground.transform.position = new Vector3( _s.x / -2, 0, _s.z / -2 );
						}
					}
					else
					{
						TerrainTools.UpdateTerrainSize( _ground.GetComponent<Terrain>().terrainData );
					}

					UpdateLevel( _player, _ground );
				}

			}

			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_SIZE );

			EditorGUI.indentLevel++;

			ICEEditorLayout.BeginHorizontal();
			TerrainTools.DesiredTerrainHeight = (int)ICEEditorLayout.DefaultSlider( "Height", "", TerrainTools.DesiredTerrainHeight , 1, 0, 600, 600 );
			EditorGUI.BeginDisabledGroup( _ground == null || _ground.GetComponent<Terrain>() == null );
			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.UpdateTerrainSize( _ground.GetComponent<Terrain>().terrainData );
					UpdateLevel( _player, _ground );
				}
			}
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_HEIGHT );

			ICEEditorLayout.BeginHorizontal();
			TerrainTools.TerrainBaseLevel = ICEEditorLayout.DefaultSlider( "Base", "", TerrainTools.TerrainBaseLevel , Init.DECIMAL_PRECISION_DISTANCES, -1, 1, 0 );
			EditorGUI.BeginDisabledGroup( _ground == null || _ground.GetComponent<Terrain>() == null );
			if( ICEEditorLayout.ButtonMiddle( "RESET", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.Reset( _ground.GetComponent<Terrain>().terrainData );
				}
			}

			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.UpdateTerrainBaseLevel( _ground.GetComponent<Terrain>().terrainData );
					TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
					UpdateLevel( _player, _ground );
				}
			}	
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_BASE );

			ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();
			TerrainTools.TerrainBumpsMax = ICEEditorLayout.DefaultSlider( "Bumps", "", TerrainTools.TerrainBumpsMax, Init.DECIMAL_PRECISION, 0, 1, TerrainTools.DefaultTerrainBumpsMax * TerrainTools.GetSizeMultiplier() );
			TerrainTools.TerrainBumpsMin = - TerrainTools.TerrainBumpsMax;
			EditorGUI.BeginDisabledGroup( _ground == null || _ground.GetComponent<Terrain>() == null  );		
			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.UpdateTerrainBumps( _ground.GetComponent<Terrain>().terrainData );
					TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
					UpdateLevel( _player, _ground );
				}					
			}
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_BUMPS );

			ICEEditorLayout.BeginHorizontal();
			TerrainTools.TerrainSmoothingLoops = (int)ICEEditorLayout.DefaultSlider( "Smoothing", "", TerrainTools.TerrainSmoothingLoops, 1, 0, 8, TerrainTools.DefaultTerrainSmoothingLoops );
			EditorGUI.BeginDisabledGroup( _ground == null || _ground.GetComponent<Terrain>() == null  );
			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.SmoothTerrain( _ground.GetComponent<Terrain>().terrainData );
					TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
					UpdateLevel( _player, _ground );
				}
			}
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_SMOOTHING );

			EditorGUI.indentLevel--;
		}

		/// <summary>
		/// Draws the scene valleys.
		/// </summary>
		/// <param name="_ground">Ground.</param>
		/// <param name="_player">Player.</param>
		public static void DrawSceneValleys( GameObject _ground, GameObject _player )
		{
			ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();
			TerrainTools.TerrainValleys = (int)ICEEditorLayout.DefaultSlider( "Valleys (max.)", "", TerrainTools.TerrainValleys , 1, 0, 500, TerrainTools.DefaultTerrainValleys );
			EditorGUI.BeginDisabledGroup( TerrainTools.TerrainHills == 0 || _ground == null || _ground.GetComponent<Terrain>() == null );
			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.CreateValleys( _ground.GetComponent<Terrain>().terrainData );
					TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
					UpdateLevel( _player, _ground );
				}
			}
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_VALLEYS  );
			EditorGUI.BeginDisabledGroup( TerrainTools.TerrainHills == 0 );
			EditorGUI.indentLevel++;

			int _terrain_height = TerrainTools.GetTerrainHeight( _ground );
			int _terrain_size = TerrainTools.GetTerrainSize( _ground );

			float _deep = (int)MathTools.Denormalize( TerrainTools.TerrainValleysMaxDeep, 0, _terrain_height );
			float _radius = (int)MathTools.Denormalize( TerrainTools.TerrainValleysMaxRadius * 2, 0, _terrain_size * 0.5f );

			TerrainTools.TerrainValleysMaxDeep = ICEEditorLayout.DefaultSlider( "Max. Deep (" + _deep + ")", "", TerrainTools.TerrainValleysMaxDeep , Init.DECIMAL_PRECISION_INDICATOR, 0, 1, TerrainTools.DefaultTerrainValleysMaxDeep * TerrainTools.GetSizeMultiplier() );
			TerrainTools.TerrainValleysMaxRadius = ICEEditorLayout.DefaultSlider( "Max. Radius (" + _radius + ")", "", TerrainTools.TerrainValleysMaxRadius, Init.DECIMAL_PRECISION_INDICATOR, 0, 1, TerrainTools.DefaultTerrainValleysMaxRadius * TerrainTools.GetSizeMultiplier() );

			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
		}

		/// <summary>
		/// Draws the scene trees.
		/// </summary>
		/// <param name="_ground">Ground.</param>
		/// <param name="_player">Player.</param>
		public static void DrawSceneTrees( GameObject _ground, GameObject _player )
		{
			ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();
			TerrainTools.TerrainTreesMax = (int)ICEEditorLayout.DefaultSlider( "Trees", "", TerrainTools.TerrainTreesMax , 1, 0, 10000, TerrainTools.DefaultTerrainTreesMax );
			TerrainTools.TerrainTreesMaxAngle = EditorGUILayout.FloatField( TerrainTools.TerrainTreesMaxAngle, GUILayout.MaxWidth( 45 ) );

			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.UpdateTrees( _ground.GetComponent<Terrain>().terrainData );
				}
			}

			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_TREES );
		}

		/// <summary>
		/// Draws the scene grass.
		/// </summary>
		/// <param name="_ground">Ground.</param>
		/// <param name="_player">Player.</param>
		public static void DrawSceneGrass( GameObject _ground, GameObject _player )
		{
			//ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();

			TerrainTools.TerrainGrassDensity = ICEEditorLayout.DefaultSlider( "Grass", "", TerrainTools.TerrainGrassDensity , 0.5f, 0, 50, TerrainTools.DefaultTerrainGrassDensity );
			TerrainTools.TerrainGrassAngle = EditorGUILayout.FloatField( TerrainTools.TerrainGrassAngle, GUILayout.MaxWidth( 45 ) );

			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.UpdateDetailLayer( _ground.GetComponent<Terrain>().terrainData );
				}
			}

			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_GRAS );

			//ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();

			TerrainTools.TerrainMeshDensity = ICEEditorLayout.DefaultSlider( "Bushes", "", TerrainTools.TerrainMeshDensity , 0.5f, 0, 50, TerrainTools.DefaultTerrainMeshDensity );
			TerrainTools.TerrainMeshAngle = EditorGUILayout.FloatField( TerrainTools.TerrainMeshAngle, GUILayout.MaxWidth( 45 ) );

			if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
			{
				if( _ground != null && _ground.GetComponent<Terrain>() != null )
				{
					Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
					TerrainTools.UpdateDetailLayer( _ground.GetComponent<Terrain>().terrainData );
				}
			}

			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_BUSHES );
		}

		/// <summary>
		/// Draws the scene plateaus.
		/// </summary>
		/// <param name="_ground">Ground.</param>
		/// <param name="_player">Player.</param>
		public static void DrawScenePlateaus( GameObject _ground, GameObject _player )
		{
			ICEEditorStyle.Splitter();
			ICEEditorLayout.BeginHorizontal();
				TerrainTools.TerrainPlateaus = (int)ICEEditorLayout.DefaultSlider( "Plateaus (max.)", "", TerrainTools.TerrainPlateaus , 1, 0, 500, 50);
				EditorGUI.BeginDisabledGroup( TerrainTools.TerrainPlateaus == 0 || _ground == null || _ground.GetComponent<Terrain>() == null  );		
					if( ICEEditorLayout.ButtonMiddle( "UPDATE", "" ))
					{
						if( _ground != null && _ground.GetComponent<Terrain>() != null )
						{
							Undo.RegisterCompleteObjectUndo( _ground.GetComponent<Terrain>().terrainData, "Wizard Terrain Data");
							TerrainTools.CreatePlateaus( _ground.GetComponent<Terrain>().terrainData );
							UpdateLevel( _player, _ground );
						}
					}
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_PLATEAUS );

			EditorGUI.BeginDisabledGroup( TerrainTools.TerrainPlateaus == 0 );
				EditorGUI.indentLevel++;
					TerrainTools.TerrainPlateausMaxHeight = ICEEditorLayout.DefaultSlider( "Max. Height", "", TerrainTools.TerrainPlateausMaxHeight , Init.DECIMAL_PRECISION_INDICATOR, 0, 1, 0.3f );
					TerrainTools.TerrainPlateausMaxRadius = (int)ICEEditorLayout.DefaultSlider( "Max. Radius", "", TerrainTools.TerrainPlateausMaxRadius , Init.DECIMAL_PRECISION_INDICATOR, 0, TerrainTools.TerrainSize / 2, 100 );
				EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
		}

	}
}