// ##############################################################################
//
// ice_utilities_terrain.cs | ICE.World.Utilities.TerrainTools
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
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ICE.World.Utilities
{
	public class TerrainTools
	{
		public static int GetTerrainHeight( GameObject _ground )
		{
			if( _ground != null && _ground.GetComponent<Terrain>() != null && _ground.GetComponent<Terrain>().terrainData != null  )
				return (int)_ground.GetComponent<Terrain>().terrainData.size.y;
			else
				return DesiredTerrainHeight;
		}

		public static int GetTerrainResolution( GameObject _ground )
		{
			if( _ground != null && _ground.GetComponent<Terrain>() != null && _ground.GetComponent<Terrain>().terrainData != null  )
				return (int)_ground.GetComponent<Terrain>().terrainData.heightmapResolution;
			else
				return Mathf.ClosestPowerOfTwo( 128 * DesiredTerrainResolutionMultiplier ) + 1;
		}

		public static int GetTerrainSize( GameObject _ground )
		{
			if( _ground != null && _ground.GetComponent<Terrain>() != null && _ground.GetComponent<Terrain>().terrainData != null  )
				return (int)_ground.GetComponent<Terrain>().terrainData.size.z;
			else
				return DesiredTerrainSize;
		}

		public static float GetSizeMultiplier(){
			return TerrainSize / 500;
		}

		public static int DesiredTerrainSize = 500;
		public static int DesiredTerrainHeight = 600;
		public static int DesiredTerrainResolutionMultiplier = 2;

		public static int DefaultTerrainSize = 500;
		public static int DefaultTerrainHeight = 600;
		public static int DefaultTerrainResolutionMultiplier = 2;

		public static int TerrainSize = 500;
		public static int TerrainHeight = 600;
		public static int TerrainResolutionMultiplier = 2;
		public static float TerrainBaseLevel = 0f;
		public static int TerrainResolution = 0;

		public static int TerrainHills = 50;
		public static float TerrainHillsMaxHeight = 0.1f;
		public static float TerrainHillsMaxRadius = 0.4f;

		public static int DefaultTerrainHills = 50;
		public static float DefaultTerrainHillsMaxHeight = 0.1f;
		public static float DefaultTerrainHillsMaxRadius = 0.4f;

		public static int TerrainValleys = 50;
		public static float TerrainValleysMaxDeep = 0.025f;
		public static float TerrainValleysMaxRadius = 0.25f;

		public static int DefaultTerrainValleys = 50;
		public static float DefaultTerrainValleysMaxDeep = 0.025f;
		public static float DefaultTerrainValleysMaxRadius = 0.25f;

		public static int TerrainPlateaus = 5;
		public static float TerrainPlateausMaxHeight = 0.15f;
		public static int TerrainPlateausMaxRadius = 100;

		public static float TerrainBumpsMin = -0.05f;
		public static float TerrainBumpsMax = 0.3f;
		public static int TerrainSmoothingLoops = 3;

		public static float DefaultTerrainBumpsMax = 0.3f;
		public static int DefaultTerrainSmoothingLoops = 3;

		public static int TerrainTreesMax = 2000;
		public static float TerrainTreesMaxAngle = 25;
		public static float TerrainGrassDensity = 6;
		public static float TerrainGrassAngle = 35;
		public static float TerrainMeshDensity = 3;
		public static float TerrainMeshAngle = 25;

		public static int DefaultTerrainTreesMax = 2000;
		public static float DefaultTerrainTreesMaxAngle = 25;
		public static float DefaultTerrainGrassDensity = 6;
		public static float DefaultTerrainGrassAngle = 35;
		public static float DefaultTerrainMeshDensity = 3;
		public static float DefaultTerrainMeshAngle = 25;


		public static GameObject CreateTerrain()
		{
			TerrainData _terrain_data = new TerrainData();

			int _default_res = 128;
			int _res = Mathf.ClosestPowerOfTwo( _default_res * DesiredTerrainResolutionMultiplier );
			int _hm_res = _res + 1;

			_terrain_data.heightmapResolution = _hm_res;
		//	_terrain_data.size = new Vector3 ( TerrainSize, TerrainHeight, TerrainSize );
			_terrain_data.baseMapResolution = _res;
			//_terrain_data.alphamapResolution = _res;
			_terrain_data.SetDetailResolution( _res * 2, 8 );
	
			TerrainResolutionMultiplier = (int)_terrain_data.heightmapResolution / _default_res;

			UpdateTerrainSize( _terrain_data, DesiredTerrainSize, DesiredTerrainHeight );

			//Debug.Log( TerrainSize + " - " +  _terrain_data.heightmapScale + " - " +  _terrain_data.heightmapResolution + " - " + _terrain_data.heightmapWidth + " - " + _terrain_data.heightmapHeight );

			UpdateTerrainBaseLevel( _terrain_data, _hm_res, TerrainBaseLevel );
			CreateHills( _terrain_data, _hm_res, TerrainHills, TerrainHillsMaxHeight, TerrainHillsMaxRadius );
			CreateValleys( _terrain_data, _hm_res, TerrainValleys, TerrainValleysMaxDeep, TerrainValleysMaxRadius );
			//CreatePlateaus( _terrain_data, _hm_res, TerrainPlateaus, TerrainPlateausMaxHeight, TerrainPlateausMaxRadius );
			UpdateTerrainBumps( _terrain_data, _hm_res, TerrainBumpsMin, TerrainBumpsMax );
			SmoothTerrain( _terrain_data, _hm_res );

			return CreateTerrain( _terrain_data );
		}
			
		public static void CreateHills( TerrainData _terrain_data )
		{
			CreateHills( _terrain_data, _terrain_data.heightmapResolution, TerrainHills, TerrainHillsMaxHeight, TerrainHillsMaxRadius );
		}

		public static void CreateHills( TerrainData _terrain_data, int _res, int _max_count, float _max_height_norm, float _max_radius_norm )
		{
			for( int _count = 0; _count < _max_count; _count++ )
			{
				int _pos_x = UnityEngine.Random.Range( 0, _res );
				int _pos_y = UnityEngine.Random.Range( 0, _res );
				float _height = UnityEngine.Random.Range( 0.025f, _max_height_norm );

				float _m = _terrain_data.size.y / _terrain_data.size.z;
				int _max_radius = (int)MathTools.Denormalize( _max_radius_norm, 0, _res * 0.5f );
				int _min_radius = (int)MathTools.Denormalize( _height * _m * 2, 0, _res * 0.5f );

				int _radius = UnityEngine.Random.Range( _min_radius, Mathf.Max( _max_radius, _min_radius ) );

				CreateHill( _terrain_data, _res, _pos_x, _pos_y, _height, _radius );
			}
		}
			
		public static void CreateHill( TerrainData _terrain_data, int _res, int _x, int _y, float _height, int _radius)
		{
			_radius = ( _radius * 2 >= _res ? ( _res / 2 ) - 1 : _radius );

			int _diameter = _radius * 2;
			int _heights_center_x = _radius;
			int _heights_center_y = _radius;
			int _base_x = _x - _heights_center_x;
			int _base_y = _y - _heights_center_y;

			_base_x = ( _base_x < 0 ? 0 : _base_x );
			_base_x = ( _base_x >=  _res - _diameter ? _res - _diameter - 1: _base_x );

			_base_y = ( _base_y < 0 ? 0 : _base_y );
			_base_y = ( _base_y >= _res - _diameter ?  _res - _diameter - 1: _base_y );

			float[,] _heights = _terrain_data.GetHeights( _base_x, _base_y, _diameter, _diameter );

			var _control_point_1 = new Vector2( 0.52f, 0.06f );
			var _control_point_2 = new Vector2( 0.42f, 0.95f );

			for( int a = 0; a < _diameter; a++ )
			{    
				for( int b = 0; b < _diameter; b++ )
				{
					float _distance_from_center = Mathf.Sqrt( Mathf.Pow( ( _heights_center_y - b ), 2 ) + Mathf.Pow( ( _heights_center_x - a ), 2 ) );
					float _time = Math.Max( 1 - ( _distance_from_center / _radius ), 0 ); // TODO : _distance_from_center - _radius / 10
					Vector2 _bezier_point = MathTools.BezierCurve( _time, new Vector2( 0, 0 ), _control_point_1, _control_point_2, new Vector2( 1, 1 ) );

					float _point_height = _bezier_point.y * _height;
					_point_height = ( _point_height < 0 ? 0 : _point_height );

					_heights[a, b] = Mathf.Lerp( _heights[a, b], _heights[a, b] + _point_height , 0.5f );
				}
			}

			_terrain_data.SetHeights( _base_x, _base_y, _heights );
		}

		public static void CreateValleys( TerrainData _terrain_data )
		{
			CreateValleys( _terrain_data, _terrain_data.heightmapResolution, TerrainValleys, TerrainValleysMaxDeep, TerrainValleysMaxRadius );
		}

		public static void CreateValleys( TerrainData _terrain_data, int _res, int _max_count, float _max_height_norm, float _max_radius_norm )
		{
			for( int _count = 0; _count < _max_count; _count++ )
			{
				int _pos_x = UnityEngine.Random.Range( 0, _res );
				int _pos_y = UnityEngine.Random.Range( 0, _res );
				float _height = UnityEngine.Random.Range( 0.025f, _max_height_norm );

				float _m = _terrain_data.size.y / _terrain_data.size.z;
				int _max_radius = (int)MathTools.Denormalize( _max_radius_norm, 0, _res * 0.5f );
				int _min_radius = (int)MathTools.Denormalize( _height * _m * 2, 0, _res * 0.5f );

				int _radius = UnityEngine.Random.Range( _min_radius, Mathf.Max( _max_radius, _min_radius ) );

				CreateValley( _terrain_data, _res, _pos_x, _pos_y, _height, _radius );
			}
		}

		public static void CreateValley( TerrainData _terrain_data, int _res, int _x, int _y, float _height, int _radius)
		{
			_radius = ( _radius * 2 >= _res ?  _res / 2 - 1 : _radius );

			int _diameter = _radius * 2;
			int _heights_center_x = _radius;
			int _heights_center_y = _radius;
			int _base_x = _x - _heights_center_x;
			int _base_y = _y - _heights_center_y;

			_base_x = ( _base_x < 0 ? 0 : _base_x );
			_base_x = ( _base_x >=  _res - _diameter ? _res - _diameter - 1: _base_x );

			_base_y = ( _base_y < 0 ? 0 : _base_y );
			_base_y = ( _base_y >= _res - _diameter ?  _res - _diameter - 1: _base_y );

			float[,] _heights = _terrain_data.GetHeights( _base_x, _base_y, _diameter, _diameter );

			var controlPoint1 = new Vector2( 0.52f, 0.06f );
			var controlPoint2 = new Vector2( 0.42f, 0.95f );

			for( int a = 0; a < _diameter; a++ )
			{    
				for( int b = 0; b < _diameter; b++ )
				{
					float _distance_from_center = Mathf.Sqrt( Mathf.Pow( ( _heights_center_y - b ), 2 ) + Mathf.Pow( ( _heights_center_x - a ), 2 ) );
					float _time = Math.Max( 1 - ( _distance_from_center / _radius ), 0 );
					Vector2 _bezier_point = MathTools.BezierCurve( _time, new Vector2( 0, 0 ), controlPoint1, controlPoint2, new Vector2( 1, 1 ) );

					float _point_height = ( _bezier_point.y * _height );

					_point_height = Mathf.Lerp( _heights[a, b], _heights[a, b] - _point_height , 0.5f );
					_point_height = ( _point_height < 0 ? 0 : _point_height );

					_heights[a, b] = _point_height;
				}
			}

			_terrain_data.SetHeights( _base_x, _base_y, _heights );
		}

		public static void CreatePlateaus( TerrainData _terrain_data )
		{
			//CreatePlateaus( _terrain_data, _terrain_data.heightmapResolution, TerrainValleys, TerrainValleysMaxDeep, TerrainValleysMaxRadius );
		}


		public static void CreatePlateaus( TerrainData _terrain_data, int _res, int _max_count, float _max_height, int _max_radius)
		{
			for( int _count = 0; _count < _max_count; _count++ )
			{
				int _pos_x = UnityEngine.Random.Range( 0, _res );
				int _pos_y = UnityEngine.Random.Range( 0, _res );
				float _height = UnityEngine.Random.Range( 0.025f, _max_height );
				int _radius = UnityEngine.Random.Range( 0, _max_radius );

				CreatePlateau( _terrain_data, _res, _pos_x, _pos_y, _height, _radius );
			}
		}

		public static void CreatePlateau( Terrain _terrain, Vector3 _position, float _radius )
		{
			if( _terrain == null )
				return;

			_position = _terrain.gameObject.transform.InverseTransformPoint( _position ); 
			
			TerrainData _terrain_data = _terrain.terrainData;

			float _height = MathTools.Normalize( _position.y, 0, _terrain_data.size.y );

			float _xm = _terrain_data.heightmapWidth / _terrain_data.size.x;
			float _ym = _terrain_data.heightmapHeight / _terrain_data.size.z;

			int _xr = Mathf.RoundToInt(_radius * _xm );
			int _yr = Mathf.RoundToInt(_radius * _ym );
			int _xd = _xr * 2;
			int _yd = _yr * 2;

			int _base_x = Mathf.RoundToInt( _position.x * _xm ) - _xr;
			int _base_y = Mathf.RoundToInt( _position.z * _ym ) - _yr;

	
			_base_x = Mathf.Clamp( _base_x, 0, _terrain_data.heightmapWidth - _xd );
			_base_y = Mathf.Clamp( _base_y, 0, _terrain_data.heightmapHeight - _yd );

			float _max_distance = Mathf.Sqrt( Mathf.Pow( _yr, 2 ) + Mathf.Pow( _xr, 2 ) );

			float[,] _heights = _terrain_data.GetHeights( _base_x, _base_y, _xd, _yd );

			for( int a = 0; a < _xd; a++ )
			{    
				for( int b = 0; b < _yd; b++ )
				{
					float _dist = Mathf.Sqrt( Mathf.Pow( ( _yr - b ), 2 ) + Mathf.Pow( ( _xr - a ), 2 ) );
					float _variation = ( _dist < _max_distance * 0.5f ? 1f : Mathf.Clamp01( 1 - MathTools.Normalize( _dist, 0, _max_distance ) ) );

					_heights[a, b] = Mathf.Lerp( _heights[a, b], _height, _variation );
				}
			}

			_terrain_data.SetHeights( _base_x, _base_y, _heights );
		}

		public static void CreatePlateau( TerrainData _terrain_data, int _res, int _x, int _y, float _height, int _radius)
		{
			_radius = ( _radius * 2 >= _res ?  _res / 2 - 1 : _radius );

			int _diameter = _radius * 2;
			int _heights_center_x = _radius;
			int _heights_center_y = _radius;
			int _base_x = _x - _heights_center_x;
			int _base_y = _y - _heights_center_y;

			_base_x = ( _base_x < 0 ? 0 : _base_x );
			_base_x = ( _base_x >=  _res - _diameter ? _res - _diameter - 1: _base_x );

			_base_y = ( _base_y < 0 ? 0 : _base_y );
			_base_y = ( _base_y >= _res - _diameter ?  _res - _diameter - 1: _base_y );

			float[,] _heights = _terrain_data.GetHeights( _base_x, _base_y, _diameter, _diameter );

			var controlPoint1 = new Vector2( 0.52f, 0.06f );
			var controlPoint2 = new Vector2( 0.42f, 0.95f );

			for( int a = 0; a < _diameter; a++ )
			{    
				for( int b = 0; b < _diameter; b++ )
				{
					float _distance_from_center = Mathf.Sqrt( Mathf.Pow( ( _heights_center_y - b ), 2 ) + Mathf.Pow( ( _heights_center_x - a ), 2 ) );
					float _time = Math.Max( 1 - ( _distance_from_center / ( _radius * 0.5f ) ), 0 );
					Vector2 _bezier_point = MathTools.BezierCurve( _time, new Vector2( 0, 0 ), controlPoint1, controlPoint2, new Vector2( 1, 1 ) );

					float _point_height = _bezier_point.y * _height;
					_point_height = ( _point_height < 0 ? 0 : _point_height );

					_heights[a, b] = Mathf.Lerp( _heights[a, b], _heights[a, b] + _point_height , 0.5f );
				}
			}

			_terrain_data.SetHeights( _base_x, _base_y, _heights );
		}


		public static void UpdateTerrainBumps( TerrainData _terrain_data ){
			UpdateTerrainBumps( _terrain_data, _terrain_data.heightmapResolution, TerrainBumpsMin, TerrainBumpsMax );
		}

		public static void UpdateTerrainBumps( TerrainData _terrain_data, int _res, float _variance_min, float _variance_max )
		{
			_variance_max = _variance_max / Mathf.Max( DesiredTerrainResolutionMultiplier, 1 );
			_variance_min = _variance_min / Mathf.Max( DesiredTerrainResolutionMultiplier, 1 );

			if( _variance_max == 0 )
				return;

			float[,] _heights = _terrain_data.GetHeights( 0, 0, _res, _res );

			for( int _i = 0; _i < _res * _res; _i++) 
			{
				int _x = _i%_res;
				int _y = _i/_res;

				float _height = _heights[ _x, _y ];

				float _xn = MathTools.Normalize( _x, 0, _res );
				float _yn = MathTools.Normalize( _y, 0, _res );
				Vector3 _ground = _terrain_data.GetInterpolatedNormal( _yn, _xn );
				float _angle =  Mathf.Abs( Vector3.Angle( _ground, Vector3.up ) );
				float _angle_multiplier = (_angle / 90 );

				if( _angle_multiplier > 0 )
					_angle_multiplier += _height;


				float _variance = UnityEngine.Random.Range( _variance_min, _variance_max ) * _angle_multiplier;

				float _point_height = Mathf.Lerp( _height, _height + _variance , 0.5f );

				_heights[ _x, _y ] = _point_height;
			}

				

			_terrain_data.SetHeights( 0, 0, _heights );

		}

		public static void Reset( TerrainData _terrain_data ){
			Reset( _terrain_data, _terrain_data.heightmapResolution, TerrainBaseLevel);
		}

		public static void Reset( TerrainData _terrain_data, int _res, float _height )
		{			
			_height = ( _height < 0 ? 0 : _height );

			float[,] _heights = _terrain_data.GetHeights( 0, 0, _res, _res );

			for( int a = 0; a < _res; a++ )
			{    
				for( int b = 0; b < _res; b++ )
				{
					_heights[a,b] = _height;
				}
			}

			_terrain_data.SetHeights( 0, 0, _heights );

		}

		public static void UpdateTerrainSize( TerrainData _terrain_data ){
			UpdateTerrainSize( _terrain_data, DesiredTerrainSize, DesiredTerrainHeight );
		}

		public static void UpdateTerrainSize( TerrainData _terrain_data, int _size, int _height ){
			if( _terrain_data.size != new Vector3( _size, _height, _size ) )
				_terrain_data.size = new Vector3( _size, _height, _size );

			TerrainSize = (int)_terrain_data.size.z;
			TerrainHeight = (int)_terrain_data.size.y;
		}

		public static void UpdateTerrainBaseLevel( TerrainData _terrain_data ){
			UpdateTerrainBaseLevel( _terrain_data, _terrain_data.heightmapResolution, TerrainBaseLevel );
		}
		public static void UpdateTerrainBaseLevel( TerrainData _terrain_data, int _res, float _base )
		{
			if( _base == 0 )
				return;

			float[,] _heights = _terrain_data.GetHeights( 0, 0, _res, _res );

			for( int a = 0; a < _res; a++ )
			{    
				for( int b = 0; b < _res; b++ )
				{
					float _height = _heights[a,b] + _base;
					_height = ( _height < 0 ? 0 : _height );

					_heights[a,b] = _height;
				}
			}

			_terrain_data.SetHeights( 0, 0, _heights );

		}

		/*
		public static void BumpyTerrain( TerrainData _terrain_data, int _res )
		{
			float[,] _heights = new float[ _res, _res ];

			for( int i = 0; i < _res * _res; i++) 
				_heights[ i%_res, i/_res ] = UnityEngine.Random.Range( 0.49f, 0.51f );

			_terrain_data.SetHeights( 0, 0, _heights );

			var _flat_areas = new float[50, 50];
			for( int i = 0; i < 50 * 50; i++) 
				_flat_areas[i%50, i/50] = 0.5f;

			for( int i = 0; i < 10; i++) 
			{
				_terrain_data.SetHeights( UnityEngine.Random.Range(0, _res-50), UnityEngine.Random.Range(0, _res-50), _flat_areas);
			}
		}*/

		public static void SmoothTerrain( TerrainData _terrain_data ){
			SmoothTerrain( _terrain_data, _terrain_data.heightmapResolution );
		}
		public static void SmoothTerrain( TerrainData _terrain_data, int _res )
		{

			float[,] _heights = _terrain_data.GetHeights( 0, 0, _res, _res );

			float _k = 0.5f;

			for( int i = 0 ; i < TerrainSmoothingLoops ; i++ )
			{
				/* Rows, left to right */
				for (int x = 1; x < _terrain_data.heightmapWidth; x++)
					for (int z = 0; z < _terrain_data.heightmapHeight; z++)
						_heights[x, z] = _heights[x - 1, z] * (1 - _k) +
							_heights[x, z] * _k;

				/* Rows, right to left*/
				for (int x = _terrain_data.heightmapWidth - 2; x < -1; x--)
					for (int z = 0; z < _terrain_data.heightmapHeight; z++)
						_heights[x, z] = _heights[x + 1, z] * (1 - _k) +
							_heights[x, z] * _k;

				/* Columns, bottom to top */
				for (int x = 0; x < _terrain_data.heightmapWidth; x++)
					for (int z = 1; z < _terrain_data.heightmapHeight; z++)
						_heights[x, z] = _heights[x, z - 1] * (1 - _k) +
							_heights[x, z] * _k;

				/* Columns, top to bottom */
				for (int x = 0; x < _terrain_data.heightmapWidth; x++)
					for (int z = _terrain_data.heightmapHeight; z < -1; z--)
						_heights[x, z] = _heights[x, z + 1] * (1 - _k) +
							_heights[x, z] * _k;
			}

			_terrain_data.SetHeights(0, 0, _heights);
		}

		public static GameObject CreateTerrain( Texture2D _map, int _map_divider, float _level_multiplier )
		{
			TerrainData _terrain_data = new TerrainData();

			ModifyHeightmap( _terrain_data, _map, _map_divider, _level_multiplier );


			return CreateTerrain( _terrain_data );
		}

		public static GameObject CreateTerrain( TerrainData _terrain_data )
		{
			if( _terrain_data == null )
				return null;
#if UNITY_EDITOR


			UpdateTrees( _terrain_data );
			UpdateDetailLayer( _terrain_data );
	
	
			UnityEditor.AssetDatabase.CreateAsset( _terrain_data, "Assets/DemoTerrain.asset" );

			GameObject _terrain = Terrain.CreateTerrainGameObject( _terrain_data );


			UpdateSplatmap( _terrain.GetComponent<Terrain>().terrainData );

		
			//_terrain.GetComponent<Terrain>().treeBillboardDistance = 500;

			return _terrain;
#else
			return null;
#endif
		}

		public static void UpdateSplatmap( TerrainData _terrain_data )
		{
			SplatPrototype[] _terrain_texture = new SplatPrototype[6];
			_terrain_texture[0] = new SplatPrototype();
			_terrain_texture[0].texture = (Texture2D)Resources.Load("Textures/Grass (Meadows)" );
			_terrain_texture[0].tileSize = new Vector2( 5, 5 );

			_terrain_texture[1] = new SplatPrototype();
			_terrain_texture[1].texture = (Texture2D)Resources.Load("Textures/Grass (Forest)" );
			_terrain_texture[1].tileSize = new Vector2( 5, 5 );

			_terrain_texture[2] = new SplatPrototype();
			_terrain_texture[2].texture = (Texture2D)Resources.Load("Textures/Grass (Rock)");
			_terrain_texture[2].tileSize = new Vector2( 15, 15 );

			_terrain_texture[3] = new SplatPrototype();
			_terrain_texture[3].texture = (Texture2D)Resources.Load("Textures/Cliff (Grassy)");
			_terrain_texture[3].tileSize = new Vector2( 15, 15 );

			_terrain_texture[4] = new SplatPrototype();
			_terrain_texture[4].texture = (Texture2D)Resources.Load("Textures/Cliff (Layered Rock Grass)");
			_terrain_texture[4].tileSize = new Vector2( 25, 25 );

			_terrain_texture[5] = new SplatPrototype();
			_terrain_texture[5].texture = (Texture2D)Resources.Load("Textures/Sand (Beach)");
			_terrain_texture[5].tileSize = new Vector2( 15, 15 );

			_terrain_data.splatPrototypes = _terrain_texture;

			// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
			float[,,] splatmapData = new float[_terrain_data.alphamapWidth, _terrain_data.alphamapHeight, _terrain_data.alphamapLayers];

			//float _max_debug_value =0;
			for (int y = 0; y < _terrain_data.alphamapHeight; y++)
			{
				for (int x = 0; x < _terrain_data.alphamapWidth; x++)
				{
					// Normalise x/y coordinates to range 0-1 
					float y_01 = (float)y/(float)_terrain_data.alphamapHeight;
					float x_01 = (float)x/(float)_terrain_data.alphamapWidth;

					// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
					//float _height = _terrain_data.GetInterpolatedHeight(y_01,x_01);

					// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
					Vector3 _normal = _terrain_data.GetInterpolatedNormal(y_01,x_01);

					// Calculate the angle deviation of the terrain (normal 0)
					float _angle =  Mathf.Abs( Vector3.Angle( _normal, Vector3.up ) );
					//float _angle_multiplier = Mathf.Clamp01( _angle / 90 );
						
					// Calculate the steepness of the terrain
					float _steepness = _terrain_data.GetSteepness(y_01,x_01);

					//float _waterlevel = 10;

					// Setup an array to record the mix of texture weights at this point
					float[] splatWeights = new float[_terrain_data.alphamapLayers];

					// CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

					// BASIC FOREST - Texture[0] has constant influence
					splatWeights[0] = 0.5f;
						
					// GRASS MEADOWS - Texture[1] stronger on flatter terrain
					splatWeights[1] = 1f - Mathf.Clamp01(_steepness*_steepness/(_terrain_data.heightmapHeight/5.0f));

					// BASIC CORRECTION - reduce the Texture[0] according to the MEADOW value
					//splatWeights[0] = Mathf.Clamp01( splatWeights[0] - ( splatWeights[1] / 2 ) );

					// MEADOW CORRECTION - splatWeights[1] will be also required on flatt hills
					//splatWeights[1] = ( splatWeights[1] > Mathf.Clamp01( ( _steepness * Mathf.Clamp01(-_normal.z) ) - _height ) ? splatWeights[1] : Mathf.Clamp01( ( _steepness * Mathf.Clamp01(-_normal.z) ) - _height ) );

				
					// BOULDERS - Texture[2] will be required only on flat hillslopes 
					splatWeights[2] = MathTools.NormalizeRange( _angle, 4, 18, 6 );//* ( _normal.z > 0 ? 1f : 0f );

					splatWeights[3] = MathTools.NormalizeRange( _angle, 10, 45, 6 ) * Mathf.Clamp01( _normal.z * ((90 - _angle )* 0.1f) );// ( _normal.z > 0 ? 1f : 0f );// Mathf.Clamp01( ( _steepness * Mathf.Clamp01(_normal.z) ) - ( _height * _height) );//Mathf.Clamp01( Mathf.Clamp01( _normal.z *  _normal.z ) );//1.0f - Mathf.Clamp01(steepness*steepness/(_terrain_data.heightmapHeight/5.0f));

					splatWeights[1] = Mathf.Clamp01( splatWeights[1] - (splatWeights[3]*splatWeights[3]) );

					// ROCK - Texture[4] will be required on cliffy surfaces over 35 degrees
					splatWeights[4] = MathTools.NormalizeRange( _angle, 35, 90, 10 );// ( _normal.z > 0 ? 1f : 0f );// Mathf.Clamp01( ( _steepness * Mathf.Clamp01(_normal.z) ) - ( _height * _height) );//Mathf.Clamp01( Mathf.Clamp01( _normal.z *  _normal.z ) );//1.0f - Mathf.Clamp01(steepness*steepness/(_terrain_data.heightmapHeight/5.0f));

					splatWeights[2] = ( splatWeights[2] == 0 ? MathTools.NormalizeRange( _angle, 30, 45, 5 ) : splatWeights[2] );

					//if( _height > _max_debug_value )
					//	_max_debug_value = _height;

					float _sum = splatWeights.Sum();

					// Loop through each terrain texture
					for( int _i = 0; _i<_terrain_data.alphamapLayers; _i++ )
					{
						// Normalize so that sum of all texture weights = 1
						splatWeights[_i] /= _sum;

						// Assign this point to the splatmap array
						splatmapData[x, y, _i] = splatWeights[_i];
					}
				}
			}

			//Debug.Log( _max_debug_value );

			// Finally assign the new splatmap to the terrainData:
			_terrain_data.SetAlphamaps(0, 0, splatmapData);
		}

		public static int TerrainDetailResolution = 1024;
		public static int TerrainPatchDetail = 8;

		public static void UpdateTrees( TerrainData _terrain_data )
		{
			GameObject[] _tree_objects = new GameObject[3];
			_tree_objects[0]= (GameObject)Resources.Load("Trees/tree1");
			_tree_objects[1]= (GameObject)Resources.Load("Trees/tree2");
			_tree_objects[2]= (GameObject)Resources.Load("Trees/tree3");

			TreePrototype[] _trees = new TreePrototype[3];
			_trees[0] = new TreePrototype();
			_trees[0].prefab = _tree_objects[0]; 

			_trees[1] = new TreePrototype();
			_trees[1].prefab = _tree_objects[1]; 

			_trees[2] = new TreePrototype();
			_trees[2].prefab = _tree_objects[2]; 
			//_trees[0].bendFactor = 

			_terrain_data.treePrototypes = _trees;

			List<TreeInstance> _tree_instances = new List<TreeInstance>();
			for( int i = 0 ; i < TerrainTreesMax ; i++ )
			{
				Vector3 _pos = PositionTools.GetRandomRectPosition( 1,1);		
				_pos.y = MathTools.Normalize( _terrain_data.GetInterpolatedHeight( _pos.x, _pos.z ) , 0, _terrain_data.size.y );

				Vector3 _ground = _terrain_data.GetInterpolatedNormal( _pos.x, _pos.z );
				float _angle = Mathf.Abs( Vector3.Angle( _ground, Vector3.up ) );

				//Debug.Log( _pos + " - " + _ground + " - " + Vector3.Angle( _ground, Vector3.up ) );

				if( _angle > 0 && _angle < TerrainTreesMaxAngle )
				{
					TreeInstance _tree = new TreeInstance();
					_tree.prototypeIndex = UnityEngine.Random.Range( 0, 3 );			
					_tree.position = _pos;
					_tree.rotation = UnityEngine.Random.Range( 0, 360 );
					_tree.heightScale = UnityEngine.Random.Range( 0.75f, 1.5f );
					_tree.widthScale = _tree.heightScale + UnityEngine.Random.Range( -0.15f, 0.15f );
					_tree.color = new Color (1, 1, 1);
					_tree.lightmapColor = new Color (1, 1, 1); 
	

					_tree_instances.Add( _tree );
				}


			}

			_terrain_data.treeInstances = _tree_instances.ToArray();
		}

		public static void UpdateDetailLayer( TerrainData _terrain_data )
		{
			//_terrain_data.SetDetailResolution( TerrainDetailResolution, TerrainPatchDetail );

			_terrain_data.wavingGrassStrength = 0.25f;
			_terrain_data.wavingGrassAmount = 0.25f;

			DetailPrototype[] _details = new DetailPrototype[10];

			int _index = 0;
			_details[_index] = new DetailPrototype();
			_details[_index].prototypeTexture = (Texture2D)Resources.Load("Grass/Grass01");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1.5f;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 1;
			_details[_index] = new DetailPrototype();
			_details[_index].prototypeTexture = (Texture2D)Resources.Load("Grass/Grass02");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1.5f;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 2;
			_details[_index] = new DetailPrototype();
			_details[_index].prototypeTexture = (Texture2D)Resources.Load("Grass/Grass03");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1.5f;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 3;
			_details[_index] = new DetailPrototype();
			_details[_index].prototypeTexture = (Texture2D)Resources.Load("Grass/Flower01");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.white;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 4;
			_details[_index] = new DetailPrototype();
			_details[_index].prototypeTexture = (Texture2D)Resources.Load("Grass/Flower02");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.yellow;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 5;
			_details[_index] = new DetailPrototype();
			_details[_index].usePrototypeMesh = true;
			_details[_index].prototype = (GameObject)Resources.Load("Bushes/Fern");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.white;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 6;
			_details[_index] = new DetailPrototype();
			_details[_index].usePrototypeMesh = true;
			_details[_index].prototype = (GameObject)Resources.Load("Bushes/Bush1");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.white;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 7;
			_details[_index] = new DetailPrototype();
			_details[_index].usePrototypeMesh = true;
			_details[_index].prototype = (GameObject)Resources.Load("Bushes/Bush2");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.white;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 8;
			_details[_index] = new DetailPrototype();
			_details[_index].usePrototypeMesh = true;
			_details[_index].prototype = (GameObject)Resources.Load("Bushes/Bush3");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.white;
			_details[_index].renderMode = DetailRenderMode.Grass;

			_index = 9;
			_details[_index] = new DetailPrototype();
			_details[_index].usePrototypeMesh = true;
			_details[_index].prototype = (GameObject)Resources.Load("Bushes/Bush4");
			_details[_index].minWidth = 0.25f;
			_details[_index].maxWidth = 1f;
			_details[_index].minHeight = 0.25f;
			_details[_index].maxHeight = 1f;
			_details[_index].healthyColor = Color.white;
			_details[_index].renderMode = DetailRenderMode.Grass;

		
			_terrain_data.detailPrototypes = _details;

			TerrainDetailResolution = _terrain_data.detailResolution;

			int[,] _density_map_00 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_01 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_02 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_03 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_04 = new int[ TerrainDetailResolution, TerrainDetailResolution ];

			int[,] _density_map_bush_00 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_bush_01 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_bush_02 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_bush_03 = new int[ TerrainDetailResolution, TerrainDetailResolution ];
			int[,] _density_map_bush_04 = new int[ TerrainDetailResolution, TerrainDetailResolution ];

			float _x_pos_m = (float)_terrain_data.size.x / (float)TerrainDetailResolution;
			float _y_pos_m = (float)_terrain_data.size.z / (float)TerrainDetailResolution;

			if( TerrainGrassDensity > 0 || TerrainMeshDensity > 0 )
			{
				for( int a = 0; a < TerrainDetailResolution; a++ )
				{
					for( int b = 0; b < TerrainDetailResolution; b++ )
					{
						float _x = MathTools.Normalize( a * _x_pos_m, 0, _terrain_data.size.x );
						float _y = MathTools.Normalize( b * _y_pos_m, 0, _terrain_data.size.z );
						Vector3 _ground = _terrain_data.GetInterpolatedNormal( _y, _x );
						float _angle =  Mathf.Abs( Vector3.Angle( _ground, Vector3.up ) );

						float _f = 5;
						//float _m = Mathf.Clamp01( 1 - ( _angle / TerrainGrassAngle ) );

						if( TerrainGrassDensity > 0 )
						{
							_density_map_00[a, b] = Mathf.RoundToInt( UnityEngine.Random.Range( TerrainGrassDensity * 0.5f, TerrainGrassDensity ) * MathTools.NormalizeRange( _angle, -_f, TerrainGrassAngle + _f, _f ) );

							_f = 5;
							_density_map_01[a, b] = Mathf.RoundToInt( UnityEngine.Random.Range( TerrainGrassDensity * 0.25f, TerrainGrassDensity * 0.3f ) * MathTools.NormalizeRange( _angle, 0, TerrainGrassAngle + _f, _f ) );						

							_f = 3;
							_density_map_02[a, b] = Mathf.RoundToInt( UnityEngine.Random.Range( TerrainGrassDensity * 0.15f, TerrainGrassDensity * 0.25f ) * MathTools.NormalizeRange( _angle, -_f * 0.5f, TerrainGrassAngle + _f, _f ) );						

							_f = 2;
							_density_map_03[a, b] = Mathf.RoundToInt( UnityEngine.Random.Range( TerrainGrassDensity * 0.1f, TerrainGrassDensity * 0.5f ) * MathTools.NormalizeRange( _angle, -_f, ( TerrainGrassAngle * 0.25f ) + _f, _f ) );						

							_f = 2;
							_density_map_04[a, b] = Mathf.RoundToInt( UnityEngine.Random.Range( TerrainGrassDensity * 0.1f, TerrainGrassDensity * 0.5f ) * MathTools.NormalizeRange( _angle, -_f, ( TerrainGrassAngle * 0.25f ) + _f, _f ) );					

						}

						if( TerrainMeshDensity > 0 )
						{
							_f = 5;
							_density_map_bush_00[a, b] = Mathf.RoundToInt( ( UnityEngine.Random.Range( 0, Mathf.RoundToInt( 100 / TerrainMeshDensity ) ) == 0 ? 1:0 ) * MathTools.NormalizeRange( _angle, ( TerrainGrassAngle * 0.5f ) - _f, ( TerrainGrassAngle * 2f ) + _f, _f ) );					

							_f = 2;
							_density_map_bush_01[a, b] = Mathf.RoundToInt( ( UnityEngine.Random.Range( 0, Mathf.RoundToInt( 500 / TerrainMeshDensity ) ) == 0 ? 1:0 ) * MathTools.NormalizeRange( _angle, _f, ( TerrainMeshAngle * 1.5f ) + _f, _f ) );					
						
							_f = 2;
							_density_map_bush_03[a, b] = Mathf.RoundToInt( ( UnityEngine.Random.Range( 0, Mathf.RoundToInt( 500 / TerrainMeshDensity ) ) == 0 ? 1:0 ) * MathTools.NormalizeRange( _angle, _f, ( TerrainMeshAngle * 1.5f ) + _f, _f ) );					

							_f = 2;
							_density_map_bush_03[a, b] = Mathf.RoundToInt( ( UnityEngine.Random.Range( 0, Mathf.RoundToInt( 500 / TerrainMeshDensity ) ) == 0 ? 1:0 ) * MathTools.NormalizeRange( _angle, _f, ( TerrainMeshAngle * 1.5f ) + _f, _f ) );					

							_f = 2;
							_density_map_bush_04[a, b] = Mathf.RoundToInt( ( UnityEngine.Random.Range( 0, Mathf.RoundToInt( 500 / TerrainMeshDensity ) ) == 0 ? 1:0 ) * MathTools.NormalizeRange( _angle, _f, ( TerrainMeshAngle * 1.5f ) + _f, _f ) );					


						}
					}
				}
			}

			_terrain_data.SetDetailLayer(0, 0, 0, _density_map_00 );
			_terrain_data.SetDetailLayer(0, 0, 1, _density_map_01 );
			_terrain_data.SetDetailLayer(0, 0, 2, _density_map_02 );
			_terrain_data.SetDetailLayer(0, 0, 3, _density_map_03 );
			_terrain_data.SetDetailLayer(0, 0, 4, _density_map_04 );

			_terrain_data.SetDetailLayer(0, 0, 5, _density_map_bush_00 );
			_terrain_data.SetDetailLayer(0, 0, 6, _density_map_bush_01 );
			_terrain_data.SetDetailLayer(0, 0, 7, _density_map_bush_02 );
			_terrain_data.SetDetailLayer(0, 0, 8, _density_map_bush_03 );
			_terrain_data.SetDetailLayer(0, 0, 9, _density_map_bush_04 );
							
		}

		public static void ModifyHeightmap( GameObject _object, int _map_divider, float _level_multiplier )
		{
			if( _object == null || _object.GetComponent<Terrain>() == null )
				return;

			ModifyHeightmap( _object.GetComponent<Terrain>().terrainData, _map_divider, _level_multiplier );
		}

		public static void ModifyHeightmap( TerrainData _terrain_data, string _filename, int _map_divider, float _level_multiplier ){
			ModifyHeightmap( _terrain_data, (Texture2D)Resources.Load( _filename ), _map_divider, _level_multiplier );
		}

		public static void ModifyHeightmap( TerrainData _terrain_data, int _map_divider, float _level_multiplier ){
			ModifyHeightmap( _terrain_data, (Texture2D)Resources.Load( "heightmap" ), _map_divider, _level_multiplier );
		}

		public static void ModifyHeightmap( TerrainData _terrain_data, Texture2D _height_map, int _map_divider, float _level_multiplier )
		{
			if( _terrain_data == null || _height_map == null )
				return;

			_map_divider = ( _map_divider < 1 ? 1 : _map_divider );
			_level_multiplier = ( _level_multiplier <= 0 ? 0.25f : _level_multiplier );

			Color[] _pixels = _height_map.GetPixels();

			int _level_divider = 4;

			int _size = Mathf.CeilToInt( Mathf.Sqrt( _pixels.Length ) );
			int _res = Mathf.ClosestPowerOfTwo( _size  / _map_divider );

			Debug.Log( _size + " - " + _res );

			_terrain_data.heightmapResolution = _res + 1;
			_terrain_data.size = new Vector3 ( _size, _size / _level_divider, _size );		
			_terrain_data.baseMapResolution = _res;
			_terrain_data.SetDetailResolution( _res, 8 );


			float[,] _height_values = new float[ _res, _res ];

			int _index = 0;
			for( int z = 0; z < _size; z++ )
			{
				for ( int x = 0; x < _size; x++ )
				{
					_height_values[ z / _map_divider, x / _map_divider ] = _pixels[ Mathf.CeilToInt( _index ) ].r * _level_multiplier;

					_index++;
				}
			}

			// Now set terrain heights.
			_terrain_data.SetHeights( 0, 0, _height_values );
		}
	}
}
