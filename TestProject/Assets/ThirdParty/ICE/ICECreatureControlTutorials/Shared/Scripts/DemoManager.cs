using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.World.Demo
{
	[ExecuteInEditMode]
	public class DemoManager : MonoBehaviour {

		public List<string> Layers = new List<string>();
		public List<string> Tags = new List<string>();

		void Awake()
		{
			if( Layers.Count == 0 )
			{
				Layers.Add("WalkableSurface");
				Layers.Add("Obstacle");
			}

			foreach( string _layer in Layers )
				AddLayer( _layer);

			foreach( string _tag in Tags )
				AddTag( _tag);
		}

		void Update () {

		}

		private static void AddTag( string _tag )
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

		private static void AddLayer( string _name )
		{
#if UNITY_EDITOR
			if( LayerMask.NameToLayer( _name ) != -1 )
				return;

			UnityEditor.SerializedObject _tag_manager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

			UnityEditor.SerializedProperty _layers = _tag_manager.FindProperty("layers");
			if( _layers == null || ! _layers.isArray )
			{
				Debug.LogWarning( "Sorry, can't set up the layers! It's possible the format of the layers and tags data has changed in this version of Unity. Please add the required layer '" + _name + "' by hand!" );
				return;
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
#endif
		}
	}
}

