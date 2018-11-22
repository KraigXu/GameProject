// ##############################################################################
//
// ice_creature_editor_interaction.cs | InteractionEditor
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
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EditorUtilities;
using ICE.Shared;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures.EditorUtilities
{
	public static class InteractionEditor
	{	
		private static ICECreatureRegister m_creature_register = null;

		public static void Print( ICECreatureControl _control )
		{
			if( m_creature_register == null )
				m_creature_register = ICECreatureRegister.Instance;

			if( m_creature_register == null )
				return;
			
			if( ! _control.Display.ShowInteraction )
				return;
			
			ICEEditorStyle.SplitterByIndent( 0 );			
			ICEEditorLayout.BeginHorizontal();
				_control.Display.FoldoutInteraction = ICEEditorLayout.Foldout( _control.Display.FoldoutInteraction, "Interaction" );	

				if( ICEEditorLayout.SaveButton( "Saves the complete interaction settings to file" ) )
					CreatureEditorIO.SaveInteractionToFile( _control.Creature.Interaction, _control.gameObject.name );			
				if( ICEEditorLayout.LoadButton( "Loads existing interaction settings form file" ) )
					_control.Creature.Interaction = CreatureEditorIO.LoadInteractionFromFile( _control.Creature.Interaction );			
				if( ICEEditorLayout.ResetButton( "Removes all interaction settings" ) )
					_control.Creature.Interaction.Reset();		

			//	GUILayout.Space( 5 );
				//if( ICEEditorLayout.ListDeleteButton<InteractorObject>( _control.Creature.Interaction.Interactors, _waypoint ) )
				//	return true;

			GUILayout.Space( 5 );
			InteractorFoldoutButtons( _control.Creature.Interaction.Interactors );
			
			ICEEditorLayout.EndHorizontal( Info.INTERACTION );
			
			if ( ! _control.Display.FoldoutInteraction ) 
				return;
				
			EditorGUILayout.Separator();
			EditorGUI.indentLevel++;				
				for (int _interactor_index = 0; _interactor_index < _control.Creature.Interaction.Interactors.Count; ++_interactor_index )
					if( DrawInteractor( _control, _control.Creature.Interaction, _interactor_index ) )	
						return;
			EditorGUI.indentLevel--;

			string _tmp_title = "Add Interactor";
			ICEEditorLayout.DrawListAddLine<InteractorObject>( _control.Creature.Interaction.Interactors , new InteractorObject(), _tmp_title );
		}

		public static int InteractorFoldoutButtons<T>( List<T> _list )
		{
			int _result = -1;
			EditorGUI.BeginDisabledGroup( _list.Count == 0 );

			if( ICEEditorLayout.ListButton( "_", "Folds all foldouts in the list" , ICEEditorStyle.CMDButtonDouble ) )
			{
				foreach( T _item in _list )
				{
					InteractorObject _obj = _item as InteractorObject;
					if( _obj != null )_obj.InteractorFoldout = false;
				}

				_result = 0;
			}

			if( ICEEditorLayout.ListButton( "\u25A1", "Unfolds all foldouts in the list" , ICEEditorStyle.CMDButtonDouble ) )
			{
				foreach( T _item in _list )
				{
					InteractorObject _obj = _item as InteractorObject;
					if( _obj != null )_obj.InteractorFoldout = true;
				}

				_result = 1;
			}

			EditorGUI.EndDisabledGroup();

			return _result;
		}

		private static bool DrawInteractor( ICECreatureControl _control, InteractionObject _interaction_object, int _index )
		{
			ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );
			
			InteractorObject _interactor = _interaction_object.Interactors[_index];

			// INTERACTOR TITLE BEGIN
			string _title = "Interactor '" + _interactor.TargetTitle + "'";
			if( _interactor.TargetGameObject == null )
				_title += "*";

			if( _interactor.Rules.Count == 0  )
				_title += " (1 act)";
			else if( _interactor.Rules.Count > 1  )
				_title += " (" + (_interactor.Rules.Count + 1) + " acts)";
			// INTERACTOR TITLE END

			// HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();			
				EditorGUI.BeginDisabledGroup ( _interactor.Enabled == false);
				_interactor.InteractorFoldout = EditorGUILayout.Foldout(_interactor.InteractorFoldout, _title , ICEEditorStyle.Foldout);
				
				if( ICEEditorLayout.SaveButtonSmall( "Saves selected interactor to file" ) )
					CreatureEditorIO.SaveInteractorToFile( _interactor, _interactor.TargetName );
				
				if( ICEEditorLayout.LoadButtonSmall( "Replaces selected interactor settings" ) )
					_interactor.Copy( CreatureEditorIO.LoadInteractorFromFile( new InteractorObject() ) );

				if( ICEEditorLayout.CopyButtonSmall( "Creates a copy of the selected interactor" ) )
					_control.Creature.Interaction.Interactors.Add( new InteractorObject( _interactor ) );
			
				if( ICEEditorLayout.ResetButtonSmall( "Resets the selected interactor settings" ) )
					_interactor.Copy( new InteractorObject() );

				EditorGUI.EndDisabledGroup();

				GUILayout.Space( 5 );

				if( ICEEditorLayout.ListDeleteButton<InteractorObject>( _interaction_object.Interactors, _interactor, "Removes selected interactor" ) )
					return true;

				GUILayout.Space( 5 );

				if( ICEEditorLayout.ListUpDownButtons<InteractorObject>( _interaction_object.Interactors, _index ) )
					return true;

				int _res_foldout = ICEEditorLayout.ListFoldoutButtons( _interactor.Rules );
				if( _res_foldout == 0 || _res_foldout == 1 )
					_interactor.Foldout = ( _res_foldout == 1 ? true : _res_foldout == 0 ? false : _interactor.Foldout );
		
				GUILayout.Space( 5 );
				_interactor.Enabled = ICEEditorLayout.EnableButton( _interactor.Enabled );

				ICEEditorLayout.PriorityButton( _interactor.AveragePriority, "Average Priority" );

			ICEEditorLayout.EndHorizontal(  ref _interactor.ShowInteractorInfoText, ref _interactor.InteractorInfoText, Info.INTERACTION_INTERACTOR );
			// HEADER END
			
			if( ! _interactor.InteractorFoldout )
				return false;

			EditorGUI.BeginDisabledGroup ( _interactor.Enabled == false);


				ICEEditorLayout.BeginHorizontal();	
					_interactor.Foldout = ICEEditorLayout.Foldout( _interactor.Foldout, " Act #1 - " + _interactor.Behaviour.BehaviourTitle() );
					ICEEditorLayout.PriorityButton( _interactor.SelectionPriority );
				ICEEditorLayout.EndHorizontal(  ref _interactor.ShowInfoText, ref _interactor.InfoText, Info.INTERACTION_INTERACTOR_TARGET );
				if( _interactor.Foldout )
				{
					TargetEditor.DrawTargetObject( _control, _interactor, "", "" );
					TargetEditor.DrawTargetContent( _control, _interactor, true, "IACT_" + _index + "_0" );
					EditorGUILayout.Separator();	
				}

				for( int _behaviour_index = 0 ; _behaviour_index < _interactor.Rules.Count ; _behaviour_index++ )
					if( DrawInteractorRule( _control, _interactor, _behaviour_index ) )
						return true;
				
			string _tmp_title = "Add Interaction Rule" + ( ! string.IsNullOrEmpty( _interactor.TargetTitle ) ? " for '" + _interactor.TargetTitle + "'" : "" );
			ICEEditorLayout.DrawListAddLine<InteractorRuleObject>( _interactor.Rules , new InteractorRuleObject() , _tmp_title );
				
			EditorGUI.EndDisabledGroup();

			return false;
		}
		/*
		private static InteractorRuleObject DrawInteractorRuleOffset( ICECreatureControl _control,InteractorObject _interactor, InteractorRuleObject _rule )
		{
			TargetObject _target = _rule as TargetObject;//new TargetObject(  TargetType.INTERACTOR );
			_target.TargetGameObject = _interactor.TargetGameObject;// m_creature_register.GetTargetByName( _interactor.TargetName );
			_target.TargetTag = _interactor.TargetTag;
			_target.TargetName = _interactor.TargetName;

			_rule.OverrideTargetMovePosition = EditorSharedTools.DrawTargetObjectBlind( _target, _rule.OverrideTargetMovePosition );
			if( _rule.OverrideTargetMovePosition )
				EditorSharedTools.DrawTargetMoveSettings( _control.gameObject, _target);
		
			return _rule;
		}	*/
		
		private static bool DrawInteractorRule( ICECreatureControl _control, InteractorObject _interactor, int _index )
		{
	
			InteractorRuleObject _rule = _interactor.Rules[_index];

			ICEEditorLayout.BeginHorizontal();	
				EditorGUI.BeginDisabledGroup ( _rule.Enabled == false);
					_rule.Foldout = ICEEditorLayout.Foldout( _rule.Foldout, " Act #" + ( _index + 2 ) + " - " + _rule.Behaviour.BehaviourTitle() );
					
					if( ICEEditorLayout.CopyButtonSmall( "Creates a copy of the selected interactor rule" ))
						_interactor.Rules.Insert( _index, new InteractorRuleObject( _rule, true ) );

					if( ICEEditorLayout.ResetButtonSmall( "Resets the selected interactor rule" ) )
						_rule.Copy( new InteractorRuleObject() );
				EditorGUI.EndDisabledGroup();

				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListDeleteButton<InteractorRuleObject>( _interactor.Rules, _rule ) )
					return true;

				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListUpDownButtons<InteractorRuleObject>( _interactor.Rules, _interactor.Rules.IndexOf( _rule ) ) )
					return true;

				
				GUILayout.Space( 5 );
				_rule.Enabled = ICEEditorLayout.EnableButton( "Enables/disables the selected rule" , _rule.Enabled );
				
				ICEEditorLayout.PriorityButton( _rule.SelectionPriority );


			ICEEditorLayout.EndHorizontal(  ref _rule.ShowInfoText, ref _rule.InfoText, Info.INTERACTION_INTERACTOR_RULE );

			if( _rule.Foldout )
			{
				EditorGUI.BeginDisabledGroup ( _rule.Enabled == false);

					if( ! Application.isPlaying )
						_rule.OverrideTargetGameObject( _interactor.TargetGameObject );
				
					_rule.Selectors.CanUseDefaultPriority = true;

					if( _rule.Selectors.UseDefaultPriority )
						_rule.Selectors.Priority = _interactor.Selectors.Priority + _index + 1;

					TargetEditor.DrawTargetObjectBlind( _rule as TargetObject, "", "" );
					TargetEditor.DrawTargetContent( _control, _rule as TargetObject, true, "IACT_" + _interactor.TargetName.ToUpper() + "_R" + ( _index + 2 ) );

			
					//TargetEditor.DrawTargetBehaviour( _control, _rule as TargetObject );
				/*
					// BEHAVIOUR
					string _auto_key = _interactor.TargetName + "_action_" + _index;
					_rule.BehaviourModeKey = BehaviourEditor.BehaviourSelect( _control, "Behaviour", "Action behaviour for this interaction rule", _rule.BehaviourModeKey, _auto_key ); 
				*/	
				EditorGUILayout.Separator();					

				EditorGUI.EndDisabledGroup ();
			}
			return false;
		}

	}


}
