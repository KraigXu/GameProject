// ##############################################################################
//
// ICECreaturePlayerUIEditor.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.Attributes;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures.UI
{
	[CustomEditor(typeof(ICECreatureControlUI))]
	public class ICECreatureControlUIEditor : ICEWorldEntityStatusUIEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICECreatureControlUI _target = DrawEntityStatusUIHeader<ICECreatureControlUI>();
			DrawCreatureUIContent( _target );
			DrawFooter( _target );
		}
			

		protected virtual void DrawCreatureUIContent( ICECreatureControlUI _ui )
		{
			if( _ui == null )
				return;
			
			_ui.StatusCanvas = _ui.gameObject.GetComponentInChildren<Canvas>();

			EditorGUI.BeginDisabledGroup( _ui.StatusCanvas != null );
			GUI.backgroundColor = ( _ui.StatusCanvas == null ? Color.yellow : Color.green );			
				if( ICEEditorLayout.Button( "User Interface", "", ICEEditorStyle.ButtonExtraLarge ) )
					CreateInterface( _ui );
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			ICEEditorLayout.BeginHorizontal();
				_ui.StatusCanvas = (Canvas)EditorGUILayout.ObjectField( "Canvas", _ui.StatusCanvas, typeof(Canvas), true );
				EditorGUI.BeginDisabledGroup( _ui.StatusCanvas != null );
					if( ICEEditorLayout.AddButton( "Add Canvas" ) )
						CreateCanvas( _ui );
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal();

			EditorGUI.BeginDisabledGroup( _ui.StatusCanvas == null );
			EditorGUI.indentLevel++;

				ICEEditorLayout.BeginHorizontal();
					_ui.HealthSlider = (Slider)EditorGUILayout.ObjectField( "Health Slider", _ui.HealthSlider, typeof(Slider), true );
					EditorGUI.BeginDisabledGroup( _ui.HealthSlider != null );
						if( ICEEditorLayout.AddButton( "Add Health Slider" ) )
							CreateSliderHealth( _ui,0 );
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal();

				ICEEditorLayout.BeginHorizontal();
					_ui.DamageText = (Text)EditorGUILayout.ObjectField( "Damage Text", _ui.DamageText, typeof(Text), true );
					EditorGUI.BeginDisabledGroup( _ui.DamageText != null );
					if( ICEEditorLayout.AddButton( "Add Damage Popup Text" ) )
						CreateDamageText( _ui, "0" );
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal();

				EditorGUI.indentLevel++;

				_ui.UseDamagePopup = ICEEditorLayout.Toggle( "Use Damage Popup", "", _ui.UseDamagePopup, "" );
				_ui.DamagePopupTime = ICEEditorLayout.MaxDefaultSlider( "Damage Popup Time (secs.)", "", _ui.DamagePopupTime, Init.DECIMAL_PRECISION_TIMER, 0, ref _ui.DamagePopupTimeMaximum, 5, "" ); 
				
				ICEEditorLayout.MinMaxDefaultSlider( "Damage Popup Speed", "", ref _ui.DamagePopupSpeedMin, ref _ui.DamagePopupSpeedMax, 0, ref _ui.DamagePopupSpeedMaximum, 0.5f, 0.5f, Init.DECIMAL_PRECISION_TIMER, 40, "" ); 

				EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();


		}

		private void CreateInterface( ICECreatureControlUI _ui )
		{
			if( _ui == null )
				return;

			CreateCanvas( _ui );
			CreateSliderHealth( _ui );
			CreateDamageText( _ui, "0" );
		}

		private void CreateCanvas( ICECreatureControlUI _ui )
		{
			if( _ui == null )
				return;

			_ui.StatusCanvas = InterfaceTools.AddCanvas( "StatusCanvas", _ui.transform, new Vector2( 0.8f, 0.25f ), AnchorPresets.StretchAll, PivotPresets.MiddleCenter, new Rect(0,0,0,0) );
		}

		private void CreateSliderHealth( ICECreatureControlUI _ui, int _pos = 0 )
		{
			if( _ui == null || _ui.StatusCanvas == null )
				return;

			float _height = 0.05f;
			Vector2 _offset = new Vector2( 0, _height * _pos );

			_ui.HealthSlider = InterfaceTools.AddSlider( "HealthSlider", _ui.StatusCanvas.transform, new Vector2( 0, _height ), Color.green, Color.red, _offset, 0.005f );
		}

		private void CreateDamageText( ICECreatureControlUI _ui, string _text, int _pos = 0 )
		{
			if( _ui == null || _ui.StatusCanvas == null )
				return;

			float _width = 40f;
			float _height = 30f;
			Vector2 _offset = new Vector2( 0, _height * _pos );

			_ui.DamageText = InterfaceTools.AddText( "DamageText", _ui.StatusCanvas.transform, new Vector2( _width, _height ), Color.green, _text, _offset, 0.005f );
		}
	}
}
	
