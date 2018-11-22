// ##############################################################################
//
// ice_creature_editor_environment.cs | EnvironmentEditor
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
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures.EditorUtilities
{
	
	public static class EnvironmentEditor
	{	
		public static void Print( ICECreatureControl _control )
		{
			if( ! _control.Display.ShowEnvironment )
				return;
			
			string _surfaces = _control.Creature.Environment.SurfaceHandler.Surfaces.Count.ToString();
			string _impacts = _control.Creature.Environment.CollisionHandler.Collisions.Count.ToString();
			
			ICEEditorStyle.SplitterByIndent( 0 );

			ICEEditorLayout.BeginHorizontal();
			_control.Display.FoldoutEnvironment = ICEEditorLayout.Foldout( _control.Display.FoldoutEnvironment, "Environment (" + _surfaces + "/" + _impacts + ")"  );

				if( ICEEditorLayout.SaveButton( "" ) )
					CreatureEditorIO.SaveEnvironmentToFile( _control.Creature.Environment, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton( "" ) )
					_control.Creature.Environment = CreatureEditorIO.LoadEnvironmentFromFile( _control.Creature.Environment );				
				if( ICEEditorLayout.ResetButton( "" ) )
					_control.Creature.Environment.Reset();

			ICEEditorLayout.EndHorizontal( Info.ENVIROMENT );
			
			if( ! _control.Display.FoldoutEnvironment ) 
				return;

			EditorGUILayout.Separator();
			EditorGUI.indentLevel++;
				DrawEnvironmentSurfaceSettings( _control );				
				DrawEnvironmentCollisionSettings( _control );	
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
				
		}

		
		private static void DrawEnvironmentSurfaceSettings( ICECreatureControl _control )
		{

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _control.Creature.Environment.SurfaceHandler.Enabled == false );
					WorldObjectEditor.DrawObjectHeaderLine( ref _control.Creature.Environment.SurfaceHandler.Enabled, ref _control.Creature.Environment.SurfaceHandler.Foldout, EditorHeaderType.FOLDOUT_BOLD, "Surfaces", "" );
							
				EditorGUI.EndDisabledGroup();

				if( ICEEditorLayout.AddButton( "Adds a new surface rule" ) )
				{
					_control.Creature.Environment.SurfaceHandler.Surfaces.Add( new SurfaceDataObject() );	
					_control.Creature.Environment.SurfaceHandler.Enabled = true;
				}
				

				if( ICEEditorLayout.SaveButton( "Saves surface data to file" ) )
					CreatureEditorIO.SaveEnvironmentSurfaceToFile( _control.Creature.Environment.SurfaceHandler, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton( "Loads surface data  to file" ) )
					_control.Creature.Environment.SurfaceHandler = CreatureEditorIO.LoadEnvironmentSurfaceFromFile( _control.Creature.Environment.SurfaceHandler );				
				if( ICEEditorLayout.ResetButton( "Resets the surface data" ) )
					_control.Creature.Environment.SurfaceHandler.Reset();
			
				_control.Creature.Environment.SurfaceHandler.Enabled = ICEEditorLayout.EnableButton( _control.Creature.Environment.SurfaceHandler.Enabled );
			ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_SURFACE );

			if( _control.Creature.Environment.SurfaceHandler.Enabled == true && _control.Creature.Environment.SurfaceHandler.Surfaces.Count == 0 )
			{
				_control.Creature.Environment.SurfaceHandler.Surfaces.Add( new SurfaceDataObject() );
				_control.Creature.Environment.SurfaceHandler.Foldout = true;
			}

			// CONTENT BEGIN
			if( WorldObjectEditor.BeginObjectContentOrReturn( EditorHeaderType.FOLDOUT_BOLD, _control.Creature.Environment.SurfaceHandler ) )
				return;

				_control.Creature.Environment.SurfaceHandler.GroundScanInterval = ICEEditorLayout.MaxDefaultSlider( "Ground Scan Interval (secs.)", "Defines the interval for the ground check", _control.Creature.Environment.SurfaceHandler.GroundScanInterval, 0.25f, 0, ref _control.Creature.Environment.SurfaceHandler.GroundScanIntervalMaximum, 1, Info.ENVIROMENT_SURFACE_SCAN_INTERVAL );
			
				for (int i = 0; i < _control.Creature.Environment.SurfaceHandler.Surfaces.Count; ++i)
				{
					// HEADER BEGIN
					SurfaceDataObject _surface = _control.Creature.Environment.SurfaceHandler.Surfaces[i];
					
					if(_surface.Name == "")
						_surface.Name = "Surface Rule #"+(i+1);
					
					ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup( _surface.Enabled == false );
							_surface.Foldout = ICEEditorLayout.Foldout( _surface.Foldout, _surface.Name );	
						EditorGUI.EndDisabledGroup();


						if( ICEEditorLayout.ListDeleteButton<SurfaceDataObject>( _control.Creature.Environment.SurfaceHandler.Surfaces, _surface, "Removes the selected surface rule" ) )
						{
							if( _control.Creature.Environment.SurfaceHandler.Surfaces.Count == 0 )
								_control.Creature.Environment.SurfaceHandler.Enabled = false;

							return;
						}

						GUILayout.Space( 5 );
						if( ICEEditorLayout.ListUpDownButtons<SurfaceDataObject>( _control.Creature.Environment.SurfaceHandler.Surfaces, i ) )
							return;

						_surface.Enabled = ICEEditorLayout.EnableButton( "Activates/deactivates the  the selected surfaces rule", _surface.Enabled );

					ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_SURFACE_RULE );
					// HEADER END

					if( _surface.Foldout ) 
					{
						EditorGUI.BeginDisabledGroup( _surface.Enabled == false );							
							ICEEditorLayout.BeginHorizontal();
								_surface.Name = ICEEditorLayout.Text("Name", "", _surface.Name );	
								if( ICEEditorLayout.ResetButtonSmall() )
									_surface.Name = "";
							ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_SURFACE_RULE_NAME );											
							//_surface.Interval = ICEEditorLayout.DefaultSlider( "Interval", "", _surface.Interval, 0.005f, 0, 30, 1, Info.ENVIROMENT_SURFACE_RULE_INTERVAL );
							
							//ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );
							
							DrawEnvironmentTextures( _surface );
							
							ICEEditorLayout.Label( "Procedures" , true, Info.ENVIROMENT_SURFACE_RULE_PROCEDURES );
							EditorGUI.indentLevel++;
								EditorGUI.BeginDisabledGroup( _surface.Textures.Count == 0 );
									_surface.UseBehaviourModeKey = ICEEditorLayout.Toggle( "Behaviour", "", _surface.UseBehaviourModeKey, Info.ENVIROMENT_SURFACE_BEHAVIOUR  );
									if( _surface.UseBehaviourModeKey )
									{
										EditorGUI.indentLevel++;
											_surface.BehaviourModeKey = BehaviourEditor.BehaviourSelect( _control, "Behaviour","Reaction to this impact", _surface.BehaviourModeKey, "SURFACE_" + _surface.Name.ToUpper(), Info.ENVIROMENT_SURFACE_BEHAVIOUR );
										EditorGUI.indentLevel--;			
									}
									EditorHeaderType _header = EditorHeaderType.TOGGLE;
									CreatureObjectEditor.DrawInfluenceObject( _surface.Influences, _header, _control.Creature.Status.UseAdvanced, _control.Creature.Status.InitialDurabilityMax, Info.ENVIROMENT_SURFACE_INFLUENCES );
									CreatureObjectEditor.DrawFootstepAudioObject( _surface.Footsteps, _header, Info.ENVIROMENT_SURFACE_AUDIO );
									CreatureObjectEditor.DrawAudioObject( _surface.Audio, _header, Info.ENVIROMENT_SURFACE_AUDIO );
									CreatureObjectEditor.DrawEffectObject( _control, _surface.Effect, _header, Info.ENVIROMENT_SURFACE_EFFECT );								
								EditorGUI.EndDisabledGroup();
							EditorGUI.indentLevel--;
							EditorGUILayout.Separator();							
						EditorGUI.EndDisabledGroup();
						ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );
					}
				}
				
				ICEEditorLayout.DrawListAddLine<SurfaceDataObject>( _control.Creature.Environment.SurfaceHandler.Surfaces , new SurfaceDataObject(), false, "Add Surface Rule" );

			WorldObjectEditor.EndObjectContent();
			// CONTENT END

		}
		
		private static void DrawEnvironmentCollisionSettings( ICECreatureControl _control )
		{
			// IMPACT HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();

				EditorGUI.BeginDisabledGroup( _control.Creature.Environment.CollisionHandler.Enabled == false );
					WorldObjectEditor.DrawObjectHeaderLine( ref _control.Creature.Environment.CollisionHandler.Enabled, ref _control.Creature.Environment.CollisionHandler.Foldout, EditorHeaderType.FOLDOUT_BOLD, "Collisions", "" );

					_control.Creature.Environment.CollisionHandler.AllowChildCollisions = ICEEditorLayout.CheckButtonSmall( "ACC", "Allow Child Collisions", _control.Creature.Environment.CollisionHandler.AllowChildCollisions );
					_control.Creature.Environment.CollisionHandler.UseCollider = ICEEditorLayout.CheckButtonSmall( "COL", "Use Collider events", _control.Creature.Environment.CollisionHandler.UseCollider );
					_control.Creature.Environment.CollisionHandler.UseTrigger = ICEEditorLayout.CheckButtonSmall( "TRI", "Use Trigger events", _control.Creature.Environment.CollisionHandler.UseTrigger );
					EditorGUI.BeginDisabledGroup( _control.GetComponent<CharacterController>() == null );		
						_control.Creature.Environment.CollisionHandler.UseCharacterController = ICEEditorLayout.CheckButtonSmall( "CON", "Use CharacterController events", _control.Creature.Environment.CollisionHandler.UseCharacterController );
					EditorGUI.EndDisabledGroup();
					GUILayout.Space( 5 );
				EditorGUI.EndDisabledGroup();

				if( ICEEditorLayout.AddButton( "Adds a new collision rule" ) )
				{
					_control.Creature.Environment.CollisionHandler.Collisions.Add( new CollisionDataObject() ); 
					_control.Creature.Environment.CollisionHandler.Enabled = true;
				}
				
				if( ICEEditorLayout.SaveButton( "Saves collision data to file" ) )
					CreatureEditorIO.SaveEnvironmentCollisionToFile( _control.Creature.Environment.CollisionHandler, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton( "Loads collision data  to file" ) )
					_control.Creature.Environment.CollisionHandler = CreatureEditorIO.LoadEnvironmentCollisionFromFile( _control.Creature.Environment.CollisionHandler );				
				if( ICEEditorLayout.ResetButton( "Resets the collision data" ) )
					_control.Creature.Environment.CollisionHandler.Reset();
			
				_control.Creature.Environment.CollisionHandler.Enabled = ICEEditorLayout.EnableButton( _control.Creature.Environment.CollisionHandler.Enabled );
			ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_COLLISION );
			// IMPACT HEADER END

			if( _control.Creature.Environment.CollisionHandler.Enabled == true && _control.Creature.Environment.CollisionHandler.Collisions.Count == 0 )
			{
				_control.Creature.Environment.CollisionHandler.Collisions.Add( new CollisionDataObject() );
				_control.Creature.Environment.CollisionHandler.Foldout = true;
			}

			// CONTENT BEGIN
			if( WorldObjectEditor.BeginObjectContentOrReturn( EditorHeaderType.FOLDOUT_BOLD, _control.Creature.Environment.CollisionHandler ) )
				return;

				for( int i = 0; i < _control.Creature.Environment.CollisionHandler.Collisions.Count ; i++ )
				{
					CollisionDataObject _collision = _control.Creature.Environment.CollisionHandler.Collisions[i];
					
					if( _collision != null )
					{
			
						if( _collision.Name.Trim() == "" )
							_collision.Name = "Collision Rule #"+(i+1);
								
						// IMPACT RULE HEADER BEGIN
						ICEEditorLayout.BeginHorizontal();	
							EditorGUI.BeginDisabledGroup( _collision.Enabled == false );
								_collision.Foldout = ICEEditorLayout.Foldout( _collision.Foldout, _collision.Name );
							EditorGUI.EndDisabledGroup();

							if( ICEEditorLayout.ListDeleteButton<CollisionDataObject>( _control.Creature.Environment.CollisionHandler.Collisions, _collision, "Removes the selected collision rule" ) )
							{
								if( _control.Creature.Environment.CollisionHandler.Collisions.Count == 0 )
									_control.Creature.Environment.CollisionHandler.Enabled = false;
								return;
							}

							GUILayout.Space( 5 );
							if( ICEEditorLayout.ListUpDownButtons<CollisionDataObject>( _control.Creature.Environment.CollisionHandler.Collisions, i ) )
								return;
					
							_collision.Enabled = ICEEditorLayout.EnableButton( "Activates/deactivates the selected collision rule", _collision.Enabled );

						ICEEditorLayout.EndHorizontal(  Info.ENVIROMENT_COLLISION_RULE  );
						// IMPACT RULE HEADER END

						// IMPACT RULE CONTENT BEGIN
						if( _collision.Foldout ) 
						{
							EditorGUI.BeginDisabledGroup( _collision.Enabled == false );		
								ICEEditorLayout.BeginHorizontal();
									_collision.Name = ICEEditorLayout.Text("Name", "", _collision.Name );	
									if( ICEEditorLayout.ResetButtonSmall() )
										_collision.Name = "";
								ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_COLLISION_RULE_NAME );

								EditorGUILayout.Separator();
							ICEEditorLayout.BeginHorizontal();
								ICEEditorLayout.Label( "Conditions" , true );

								_collision.UseTag = ICEEditorLayout.CheckButtonMiddle( "TAG", "", _collision.UseTag );
								_collision.UseLayer = ICEEditorLayout.CheckButtonMiddle( "LAYER", "", _collision.UseLayer );
								_collision.UseBodyPart = ICEEditorLayout.CheckButtonMiddle( "COLLIDER", "", _collision.UseBodyPart );
							ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_COLLISION_RULE_CONDITIONS );
								EditorGUI.indentLevel++;

									if( _collision.UseLayer )
										_collision.Layer = ICEEditorLayout.Layer( "Layer","Desired collision layer", _collision.Layer, Info.ENVIROMENT_COLLISION_RULE_LAYER );
					
									if( _collision.UseTag )
										_collision.Tag = ICEEditorLayout.Tag( "Tag","Desired collision tag", _collision.Tag, Info.ENVIROMENT_COLLISION_RULE_TAG );
								
									if( _collision.UseBodyPart )
										_collision.BodyPart = ICEEditorLayout.ColliderPopup( _control.gameObject, "Body Part","Desired body part", _collision.BodyPart, Info.ENVIROMENT_COLLISION_RULE_BODYPART );
						
								EditorGUI.indentLevel--;	
									
								EditorGUILayout.Separator();
								ICEEditorLayout.Label( "Procedures" , true, Info.ENVIROMENT_COLLISION_RULE_PROCEDURES );
								EditorGUI.indentLevel++;
									_collision.UseBehaviourModeKey = ICEEditorLayout.Toggle("Behaviour", "", _collision.UseBehaviourModeKey, Info.ENVIROMENT_COLLISION_BEHAVIOUR  );								
									if( _collision.UseBehaviourModeKey )
									{
										EditorGUI.indentLevel++;
											_collision.BehaviourModeKey = BehaviourEditor.BehaviourSelect( _control, "Behaviour","Reaction to this impact", _collision.BehaviourModeKey, "COLLISION_" + _collision.Name.ToUpper(), Info.ENVIROMENT_COLLISION_BEHAVIOUR );
										EditorGUI.indentLevel--;			
									}

									EditorHeaderType _header = EditorHeaderType.TOGGLE;
									CreatureObjectEditor.DrawInfluenceObject( _collision.Influences, _header, _control.Creature.Status.UseAdvanced, _control.Creature.Status.InitialDurabilityMax, Info.ENVIROMENT_COLLISION_INFLUENCES );
									//CreatureObjectEditor.DrawAudioObject( _collision.Audio, _header, Info.ENVIROMENT_COLLISION_AUDIO );
									//CreatureObjectEditor.DrawEffectObject( _control, _collision.Effect, _header, Info.ENVIROMENT_COLLISION_EFFECT );

			
									//_impact.ForceInteraction = EditorGUILayout.Toggle("Force Interaction", _impact.ForceInteraction );
									EditorGUI.indentLevel--;
								EditorGUILayout.Separator();
							
														
							EditorGUI.EndDisabledGroup();
							ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );
						}
						// IMPACT RULE CONTENT END
					}
				}
				
				ICEEditorLayout.DrawListAddLine<CollisionDataObject>( _control.Creature.Environment.CollisionHandler.Collisions , new CollisionDataObject(), false, "Add Collision Rule" );

			WorldObjectEditor.EndObjectContent();
			// CONTENT END
		}
		
		
		private static void DrawEnvironmentTextures( SurfaceDataObject _environment )
		{
			if( _environment == null )
				return;

			ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.Label( "Trigger Textures", true );
				if( ICEEditorLayout.AddButton( "Add a texture" ) )
					_environment.Textures.Add( new TextureDataObject() );		
			ICEEditorLayout.EndHorizontal( Info.ENVIROMENT_SURFACE_RULE_TEXTURES );

			if( _environment.Textures.Count > 0 )
			{
				int _width = 90;
				int _tolerance_space = 50 + (EditorGUI.indentLevel * 15) + _width;
				int _inspector_width = Screen.width - _tolerance_space;
				int _textures_width = 0;		
				int _max_count = 0;
				int _counter = 0;

				if( _inspector_width < 120 )
					_max_count = 3;

				int _i = 0;
				//for( int i = 0; i < _environment.Textures.Count; i++ )
				foreach( TextureDataObject _data in _environment.Textures )
				{	
					//TextureDataObject _data = _environment.Textures[i];

					if( _data == null )
						continue;
					/*
					if( _data.Image == null && ! string.IsNullOrEmpty( _data.FilePath ) )
						_data.Image = (Texture)AssetDatabase.LoadAssetAtPath<Texture>( _data.FilePath );
						*/

					if(_counter == 0)
					{
						ICEEditorLayout.BeginHorizontal();
						GUILayout.Space( EditorGUI.indentLevel * 15 );
					}

					int _indent = EditorGUI.indentLevel;
					
					EditorGUI.indentLevel = 0;
					GUILayout.BeginVertical("BOX", GUILayout.MinWidth(_width), GUILayout.MaxWidth(_width), GUILayout.MinHeight(90));
						_data.Image = (Texture)EditorGUILayout.ObjectField( _data.Image, typeof(Texture), false, GUILayout.Height(75) );

					/*
						if( _data.Image != null )
							_data.FilePath = AssetDatabase.GetAssetPath( _data.Image );
							*/
		
						if( GUILayout.Button( "DELETE" ) )
						{
							_environment.Textures.RemoveAt(_i);
							return;
						}

					GUILayout.EndVertical();
					EditorGUI.indentLevel = _indent;					

					_counter++;					
					_textures_width = _counter * _width;
					if( _textures_width > _inspector_width || _counter == _max_count || _i == _environment.Textures.Count - 1 )
					{
						ICEEditorLayout.EndHorizontal();
						EditorGUILayout.Separator();
						_counter = 0;
					}

					_i++;
				}
			}

			if(_environment.Textures.Count == 0)
				EditorGUILayout.HelpBox("No textures assigned. Press ADD to assign a texture!", MessageType.Info);
			
			EditorGUILayout.Separator();
			
		}
		

		

	}
}
