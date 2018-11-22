// ##############################################################################
//
// ICECreatureEntityEditor.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
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
	[CustomEditor(typeof(ICECreatureEntity))]
	public class ICECreatureEntityEditor : ICEWorldBehaviourEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICECreatureEntity _target = DrawEntityHeader<ICECreatureEntity>();

			DrawFooter( _target );
		}

		/// <summary>
		/// Draws the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T DrawEntityHeader<T>() where T : ICECreatureEntity
		{
			m_HeaderType = EditorHeaderType.FOLDOUT_ENABLED_BOLD;
			ICEEditorLayout.SetDefaults();
			T _target = (T)target;

			GUI.changed = false;
			Info.Reset( _target );

				// TARGET DEFAULTS
			DrawEntitySettings( _target );
			HandleDebug<ICECreatureEntityDebug>( _target );
			EditorGUILayout.Separator();

			// indentLevel will be decreased in the footer, so we don't need to do it here
			EditorGUI.indentLevel++;

			DrawEntityContent( _target );
			return _target;
		}

		/// <summary>
		/// Draws the footer.
		/// </summary>
		/// <param name="_target">Target.</param>
		public virtual void DrawFooter( ICECreatureEntity _entity )
		{
			if( _entity == null )
				return;

			EditorGUILayout.Separator();
			CreatureObjectEditor.DrawEntityRuntimeBehaviourObject( _entity, _entity.RuntimeBehaviour, m_HeaderType );

			if( _entity as ICECreatureItem != null )
			{
				if( _entity.ObjectRigidbody == null || _entity.ObjectColliders == null  )
					EditorGUILayout.HelpBox( Info.RIGIDBODY_AND_COLLIDER, MessageType.Info );

				ICEEditorLayout.DrawAddRigidbody( _entity.gameObject );
				ICEEditorLayout.DrawAddCollider( _entity.gameObject );
				EditorGUILayout.Separator();
			}
			else if( _entity as  ICECreatureBodyPart != null ||
				_entity as  ICECreatureZone != null )
			{
				if( _entity.ObjectColliders == null )
					EditorGUILayout.HelpBox( Info.TRIGGER_COLLIDER, MessageType.Info );

				ICEEditorLayout.DrawAddTrigger( _entity.gameObject );
			}

			ICECreatureEntityEditor.DrawTargetAttribute( _entity.gameObject );
			EditorGUILayout.Separator();

			// indentLevel was increased in the header, so we have to decrease the level here
			EditorGUI.indentLevel--;

			// Version Info
			EditorGUILayout.LabelField( " - " + _entity.GetType().ToString() + " v" + Info.Version + " - ", EditorStyles.centeredGreyMiniLabel );

			MarkSceneDirty( _entity );
		}

		/// <summary>
		/// Handles the debug.
		/// </summary>
		/// <returns>The debug.</returns>
		/// <param name="_entity">Entity.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T HandleDebug<T>( ICECreatureEntity _entity ) where T : Behaviour
		{
			if( _entity == null )
				return null;

			T _debug = _entity.GetComponent<T>();
			
			if( _entity.UseDebug )
			{
				if( _debug == null )
					_debug = _entity.gameObject.AddComponent<T>();
				else if( _debug.enabled == false )
					_debug.enabled = true;

			}
			else if( _debug != null )
			{
				_debug.enabled = false;
			}

			return _debug;
		}

		/// <summary>
		/// Draws the entity settings.
		/// </summary>
		/// <param name="_entity">Entity.</param>
		public static void DrawEntitySettings( ICECreatureEntity _entity )
		{
			if( _entity == null )
				return;

			if( _entity.EntityType == ICE.World.EnumTypes.EntityClassType.Creature && Application.isPlaying )
				EditorGUILayout.HelpBox( Info.DISPLAY_OPTIONS_RUNTIME_INFO , MessageType.Info );
			
			EditorGUILayout.Separator();

			if( ICECreatureRegister.Instance == null )
			{
				GUI.backgroundColor = Color.yellow;
				if( ICEEditorLayout.ButtonExtraLarge( "ADD CREATURE REGISTER", Info.REGISTER_MISSING ) )
					ICECreatureRegister.Create();

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			}
			else if( ! ICECreatureRegister.Instance.isActiveAndEnabled )
			{
				GUI.backgroundColor = Color.yellow;
				if( ICEEditorLayout.ButtonExtraLarge( "ACTIVATE CREATURE REGISTER", Info.REGISTER_DISABLED  ) )
				{
					ICECreatureRegister.Instance.gameObject.SetActive( true);
				}
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			}
			else
			{
				Popups.QuickSelectionPopup( "Quick Selection", _entity );
			}

			ICEEditorLayout.BeginHorizontal();
		
				ICEEditorLayout.PrefixLabel( "Debug Options" );
							
			GUILayout.FlexibleSpace();

				_entity.UseDebugLogs = ICEEditorLayout.DebugButtonSmall( "LOG", "Print debug information for this GameObject", _entity.UseDebugLogs );
				EditorGUI.BeginDisabledGroup( _entity.UseDebugLogs == false );
					if( _entity.UseDebugLogs )
						_entity.UseDebugLogsSelectedOnly = ICEEditorLayout.DebugButtonMini( "S", "Shows debug information for selected GameObjects only.", _entity.UseDebugLogsSelectedOnly );
					else
						ICEEditorLayout.DebugButtonMini( "S", "Shows debug information for selected GameObjects only.", false );
				EditorGUI.EndDisabledGroup();
				GUILayout.Space( 3 );

				_entity.UseDebugRays = ICEEditorLayout.DebugButtonSmall( "RAY", "Shows debug rays for this GameObject", _entity.UseDebugRays  );
				EditorGUI.BeginDisabledGroup( _entity.UseDebugRays == false );
					if( _entity.UseDebugRays )
						_entity.UseDebugRaysSelectedOnly = ICEEditorLayout.DebugButtonMini( "S", "Shows debug rays for selected GameObjects only.", _entity.UseDebugRaysSelectedOnly );
					else
						ICEEditorLayout.DebugButtonMini( "S", "Shows debug rays for selected GameObjects only.", false );
				EditorGUI.EndDisabledGroup();
				GUILayout.Space( 3 );

				_entity.UseDebug = ICEEditorLayout.DebugButton( "DEBUG", "Enables enhanced debug options", _entity.UseDebug );		
				GUILayout.Space( 3 );

				EditorGUI.BeginDisabledGroup( _entity as ICECreatureControl == null );
					_entity.ShowInfo = ICEEditorLayout.DebugButton( "INFO", "Displays runtime information.", _entity.ShowInfo );
				EditorGUI.EndDisabledGroup();
	
				_entity.ShowHelp = ICEEditorLayout.DebugButton( "HELP", "Displays all help informations", _entity.ShowHelp );
				_entity.ShowNotes = ICEEditorLayout.DebugButton( "NOTES", "Displays all note fields", _entity.ShowNotes );
			ICEEditorLayout.EndHorizontal( Info.ENTITY_DEBUG_OPTIONS );

		}

		protected virtual void DrawEntityContent( ICECreatureEntity _entity )
		{
			if( _entity == null )
				return;
			
			CreatureObjectEditor.DrawEntityStatusObject( _entity, _entity.Status, m_HeaderType );
		}

		private static AttributeType _attribute;
		/// <summary>
		/// Draws the target attribute.
		/// </summary>
		/// <param name="_object">Object.</param>
		public static void DrawTargetAttribute( GameObject _object ){

			if( _object == null )
				return;

			ICEEditorLayout.BeginHorizontal();
			_attribute = (AttributeType)ICEEditorLayout.EnumPopup( "Add Attribute", "", _attribute ); 
			if( ICEEditorLayout.AddButton( "Adds the selected attribute" ) )
			{
				if( _attribute == AttributeType.TARGET )
					_object.AddComponent<ICECreatureTargetAttribute>();
				else if( _attribute == AttributeType.ODOUR )
					_object.AddComponent<ICECreatureOdourAttribute>();
			}
			ICEEditorLayout.EndHorizontal( Info.ATTRIBUTE_ADD );
		}
	}
}
	
