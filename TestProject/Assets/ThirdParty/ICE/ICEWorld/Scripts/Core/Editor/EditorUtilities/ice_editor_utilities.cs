// ##############################################################################
//
// ice_utilities_editor.cs | ICE.World.Utilities.EditorTools
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
using System.IO;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;
using ICE.World.EditorUtilities;

namespace ICE.World.EditorUtilities
{
	public class EditorTools 
	{
		public static string GetFilePath( string _file )
		{
			string[] _file_codes = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension( _file ) );
			foreach( string _code in _file_codes )
			{
				string _path = AssetDatabase.GUIDToAssetPath( _code );
				string _filename = Path.GetFileName( _path );
				if( _filename == _file )
					return _path;
			}

			return "";
		}

		public static AxisInputData[] ReadAxes()
		{
			var _input_manager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
			SerializedObject _input_object = new SerializedObject(_input_manager);
			SerializedProperty _axes_array = _input_object.FindProperty("m_Axes");

			if( _axes_array.arraySize == 0 )
				return new AxisInputData[0];

			AxisInputData[] _axes = new AxisInputData[_axes_array.arraySize];

			for( int i = 0; i < _axes_array.arraySize; ++i )
			{
				_axes[i] = new AxisInputData();

				SerializedProperty axis = _axes_array.GetArrayElementAtIndex(i);

				_axes[i].Name = axis.FindPropertyRelative("m_Name").stringValue;
				_axes[i].Value = axis.FindPropertyRelative("axis").intValue;
				_axes[i].Type = (AxisInputType)axis.FindPropertyRelative("type").intValue;

				//Debug.Log(_axes[i].Name);
				//Debug.Log(_axes[i].Value);
				//Debug.Log(_axes[i].Type);
			}

			return _axes;
		}

		public static int StringToIndex( string _text, string[] _data )
		{
			int _i = 0;
			if( _data.Length > 0 )
			{
				foreach( string _msg in _data )
				{
					if( _text == _msg )
						return _i;

					_i++;
				}
			}

			return 0;
		}


		public static void AddTag( string _tag )
		{
#if UNITY_EDITOR
			UnityEngine.Object[] _asset = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
			if( ( _asset != null ) && ( _asset.Length > 0 ) )
			{
				UnityEditor.SerializedObject _object = new UnityEditor.SerializedObject(_asset[0]);
				UnityEditor.SerializedProperty _tags = _object.FindProperty("tags");

				for( int i = 0; i < _tags.arraySize; ++i )
				{
					if( _tags.GetArrayElementAtIndex(i).stringValue == _tag )
						return;    
				}

				_tags.InsertArrayElementAtIndex(0);
				_tags.GetArrayElementAtIndex(0).stringValue = _tag;
				_object.ApplyModifiedProperties();
				_object.Update();
			}
#endif
		}

		public static bool AddLayer( string _name )
		{
#if UNITY_EDITOR
			if( LayerMask.NameToLayer( _name ) != -1 )
				return true;

			UnityEditor.SerializedObject _tag_manager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

			UnityEditor.SerializedProperty _layers = _tag_manager.FindProperty("layers");
			if( _layers == null || ! _layers.isArray )
			{
				Debug.LogWarning( "Sorry, can't set up the layers! It's possible the format of the layers and tags data has changed in this version of Unity. Please add the required layer '" + _name + "' by hand!" );
				return false;
			}

			int _layer_index = -1;
			for ( int _i = 8 ; _i < 32 ; _i++ )
			{
				_layer_index = _i;
				UnityEditor.SerializedProperty _layer = _layers.GetArrayElementAtIndex(_i);

				//Debug.Log( _layer_index + " - " + _layer.stringValue );

				if( _layer.stringValue == "" )
				{
					Debug.Log( "Setting up layers.  Layer " + _layer_index + " is now called " + _name );
					_layer.stringValue = _name;
					break;
				}
			}

			_tag_manager.ApplyModifiedProperties();

			if( LayerMask.NameToLayer( _name ) != -1 )
				return true;
			else
				return false;
#else
			return true;
#endif
		}



		public static string ObjectTitleSuffix( GameObject _object ){
			return (_object != null?(IsPrefab( _object )?"(prefab)":"(scene)"):"(null)");
		}

		public static bool IsPrefab( GameObject _object )
		{
			#if UNITY_EDITOR
			if( _object != null && UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource( _object ) == null && UnityEditor.PrefabUtility.GetPrefabObject( _object ) != null ) // Is a prefab
				return true;
			else
				return false;
			#else
				return ( _object != null && _object.activeInHierarchy == true ? false : true );
			#endif
		}
	}

}