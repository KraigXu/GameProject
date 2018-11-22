// ##############################################################################
//
// ICECreatureLadderEditor.cs
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
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures
{
	[CustomEditor(typeof(ICECreatureLadder)), CanEditMultipleObjects]
	public class ICECreatureLadderEditor : ICECreatureObjectEditor  
	{
		public override void OnInspectorGUI()
		{
			ICECreatureLadder _target = DrawEntityHeader<ICECreatureLadder>();
			DrawObjectFunctionsContent( _target );
			DrawFooter( _target );
		}

		public virtual void DrawObjectFunctionsContent( ICECreatureLadder _target )
		{
			//_target.AttachedCollider = (Collider)EditorGUILayout.ObjectField( "Collider", _target.AttachedCollider, typeof(Collider), true );
			_target.ClimbingOffset = ICEEditorLayout.Vector3Field( "", "", _target.ClimbingOffset );

			AddComponentButton<BoxCollider>( _target.gameObject, "Box Collider", "" );

		}

		public T AddComponentButton<T>( GameObject _object, string _title, string _hint ) where T : Component
		{
			T _component = _object.GetComponent<T>(); 

			EditorGUI.BeginDisabledGroup( _component != null );
				GUI.backgroundColor = ( _component == null ? Color.yellow : Color.green );			
				if( ICEEditorLayout.Button( _title, _hint, ICEEditorStyle.ButtonExtraLarge ) )
				_component = _object.AddComponent<T>() as T;
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();

			return _component;
		}
	}


}

