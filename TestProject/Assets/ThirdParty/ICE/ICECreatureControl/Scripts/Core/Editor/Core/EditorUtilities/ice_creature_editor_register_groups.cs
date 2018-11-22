// ##############################################################################
//
// ice_creature_editor_register_groups.cs | RegisterGroupsEditor
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
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.Shared;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.Attributes;


namespace ICE.Creatures.EditorUtilities
{
	public static class RegisterGroupsEditor
	{
		private static bool m_foldout = true;
		public static void Print( ICECreatureRegister _register )
		{
			// HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();
				/*if( _register.UseReferenceCategories )
					ICEEditorLayout.Label( "Reference Objects", true );
				else*/
			m_foldout = ICEEditorLayout.Foldout ( m_foldout, "Reference Objects" );

				if( _register.UseReferenceCategories )
				{
					int _res_foldout = ICEEditorLayout.ListFoldoutButtons<ReferenceGroupCategory>( _register.ReferenceGroupCategories );
					if( _res_foldout == 0 || _res_foldout == 1 )
					{
						foreach( ReferenceGroupCategory _cat in _register.ReferenceGroupCategories )
						{
							foreach( ReferenceGroupObject _group in _cat.ReferenceGroupObjects )
								_group.Foldout = ( _res_foldout == 1 ? true : _res_foldout == 0 ? false : _group.Foldout ); 
						}
					}
				}
				else
					ICEEditorLayout.ListFoldoutButtons<ReferenceGroupObject>( _register.ReferenceGroupObjects );

				GUILayout.Space( 5 );

				_register.BreakAll = ICEEditorLayout.CheckButtonMiddle( "BREAK", "Forces an interruption to all objects during runtime", _register.BreakAll );

				_register.UseReferenceCategories = ICEEditorLayout.CheckButtonMiddle( "GROUPS", "Displays all reference objects in their respective entity groups.", _register.UseReferenceCategories );

				_register.UsePoolManagement = ICEEditorLayout.CheckButtonMiddle( "POOL","Activates/deactivates the Pool Management",_register.UsePoolManagement );

				// REFRESH REFERENCE OBJECTS BEGIN
				ICECreatureEntity[] _entities = GameObject.FindObjectsOfType<ICECreatureEntity>();
				EditorGUI.BeginDisabledGroup( _entities == null );
					if( _entities != null )
						GUI.backgroundColor = ( _entities.Length != _register.ReferenceGameObjects.Count ? Color.yellow : Color.green );	
					if( ICEEditorLayout.Button( "UPDATE", "Updates the list of reference objects" ) )
						_register.UpdateReferences();
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
				EditorGUI.EndDisabledGroup();
				// REFRESH REFERENCE OBJECTS END
		
			ICEEditorLayout.EndHorizontal( ref _register.ShowReferencesInfo, ref _register.ReferencesInfo, Info.REGISTER_REFERENCE_OBJECTS );
			// HEADER END

			if( ! m_foldout )// && ! _register.UseReferenceCategories )
				return;

			EditorGUI.indentLevel++;

				if( _register.UseReferenceCategories )
				{
					foreach( ReferenceGroupCategory _cat in _register.ReferenceGroupCategories )
					{
						if( _cat.ReferenceGroupObjects.Count > 0 )
							DrawReferenceGroupListCat( _register, _cat.Type.ToString(), _cat, ref _cat.Foldout );
					}
				}
				else
				{
					DrawReferenceGroupList( _register, _register.ReferenceGroupObjects );
				}

				EditorGUILayout.Separator();
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );
				
				ICEEditorLayout.BeginHorizontal();
					GameObject _new = (GameObject)EditorGUILayout.ObjectField( "Add Reference Object", null, typeof(GameObject), true );					
					if( _new != null )
						_register.AddReference( _new );				
				ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECTS_ADD );

		
			EditorGUI.indentLevel--;		
			EditorGUILayout.Separator();
			ICEEditorStyle.SplitterByIndent(0);

			// REFRESH REFERENCE OBJECTS BEGIN
			EditorGUI.BeginDisabledGroup( _entities == null );
				if( _entities != null )
					GUI.backgroundColor = ( _entities.Length > _register.ReferenceGameObjects.Count ? Color.yellow : Color.green );					
				if( ICEEditorLayout.ButtonExtraLarge( "UPDATE REFERENCE OBJECTS", "Updates the list of reference objects") )
					_register.UpdateReferences();
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();
			// REFRESH REFERENCE OBJECTS END
		}

		private static void DrawReferenceGroupListCat( ICECreatureRegister _register, string _list_title, ReferenceGroupCategory _cat , ref bool _foldout )
		{
			if( _cat == null || _cat.ReferenceGroupObjects == null )
				return;
			
			List<ReferenceGroupObject> _list = _cat.ReferenceGroupObjects;

			EditorGUI.BeginDisabledGroup( _list.Count == 0 );

				// HEADER BEGIN
				ICEEditorLayout.BeginHorizontal();
					_list_title += " (" + _list.Count + ")";
					_foldout =  ICEEditorLayout.Foldout( _foldout, _list_title, "" );

					ICEEditorLayout.ListFoldoutButtons<ReferenceGroupObject>( _list );

				ICEEditorLayout.EndHorizontal( ref _cat.ShowInfoText, ref _cat.InfoText, Info.REGISTER_REFERENCE_CAT );
				// HEADER END

				EditorGUI.indentLevel++;
					if( _foldout )
					{
						for( int _index = 0 ; _index < _list.Count ; _index++ )
						{
							ReferenceGroupObject _group = _list[_index];
							
							UpdateStatus( _group );
							
							if( _group != null && _group.ReferenceGameObject != null )
							{
								ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );
								ICEEditorLayout.BeginHorizontal();
									EditorGUI.BeginDisabledGroup( _group.Enabled == false );
										_group.Foldout = EditorGUILayout.Foldout( _group.Foldout, GetTitleText( _group ) , ICEEditorStyle.Foldout );
										GUILayout.FlexibleSpace();					
										

										EditorGUI.BeginDisabledGroup( _group.EntityComponent == null );
										_group.Break = ICEEditorLayout.CheckButtonMiddle( "BREAK", "Deactivates/activates all entity components", _group.Break );
										EditorGUI.EndDisabledGroup();
						

										_group.UseSoftRespawn = ICEEditorLayout.CheckButtonMiddle( "RECYCLE", "Allows the reuse of suspended objects without new instantiations", _group.UseSoftRespawn );


										EditorGUI.BeginDisabledGroup( _register.UsePoolManagement == false  );
											_group.PoolManagementEnabled = ICEEditorLayout.CheckButtonMiddle( "POOL", "Activates Pool Management", _group.PoolManagementEnabled );
										EditorGUI.EndDisabledGroup();
									EditorGUI.EndDisabledGroup();

									_group.Enabled = ICEEditorLayout.EnableButton( "Enables/Disables the group", _group.Enabled );
										
								if( ICEEditorLayout.ListDeleteButtonMini<ReferenceGroupObject>( _register.ReferenceGroupObjects, _group, "Removes this reference group." ) )
										return;
							
								ICEEditorLayout.EndHorizontal(  Info.REGISTER_REFERENCE_OBJECT_GROUP );
								
						
								DrawReferenceGroup( _register, _group );
								
							}
							else
							{
								_list.RemoveAt(_index);
								--_index;
							}
						}

					if( _list.Count > 0 )
						EditorGUILayout.Separator();

				}
				EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
		}

		private static string GetTitleText( ReferenceGroupObject _group )
		{
			string _suspended = ( _group.UseSoftRespawn ? "/" + _group.SuspendedObjects.Count :"" );
			string _amount = "";
			if( _group.PoolManagementEnabled )
				_amount = " [" + _group.ActiveObjects.Count + _suspended + " of " + _group.MaxCoexistingObjects + "]";
			else
				_amount = " [" + _group.ActiveObjects.Count + _suspended + "]";


			return _group.Name + " " + ( EditorTools.IsPrefab( _group.ReferenceGameObject )?"(PREFAB)":"(SCENE)") + _amount;
		}

		private static void DrawReferenceGroupList( ICECreatureRegister _register, List<ReferenceGroupObject> _list )
		{
			for( int _index = 0 ; _index < _list.Count ; _index++ )
			{
				ReferenceGroupObject _group = _list[_index];
				
				UpdateStatus( _group );
				
				if( _group != null && _group.ReferenceGameObject != null )
				{
					ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );
					ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup( _group.Enabled == false );
							_group.Foldout = EditorGUILayout.Foldout( _group.Foldout, GetTitleText( _group ) , ICEEditorStyle.Foldout );
							GUILayout.FlexibleSpace();					
							
							DrawEntityType( _group );
							
						EditorGUI.EndDisabledGroup();


						if( ICEEditorLayout.ListUpDownButtons<ReferenceGroupObject>( _list, _index ) )
							return;

						GUILayout.Space( 5 );
						EditorGUI.BeginDisabledGroup( _group.Enabled == false );
									
							EditorGUI.BeginDisabledGroup( _group.EntityComponent == null );
								_group.Break = ICEEditorLayout.CheckButtonMiddle( "BREAK", "Deactivates all entity components", _group.Break );
							EditorGUI.EndDisabledGroup();

							_group.UseSoftRespawn = ICEEditorLayout.CheckButtonMiddle( "RECYCLE", "Allows the reuse of suspended objects without new instantiations", _group.UseSoftRespawn );

							EditorGUI.BeginDisabledGroup( _register.UsePoolManagement == false );
								_group.PoolManagementEnabled = ICEEditorLayout.CheckButtonMiddle( "POOL", "Activates Pool Management", _group.PoolManagementEnabled );
							EditorGUI.EndDisabledGroup();
						EditorGUI.EndDisabledGroup();
	
						_group.Enabled = ICEEditorLayout.EnableButton( "Enables/Disables the group", _group.Enabled );

						if( ICEEditorLayout.ListDeleteButtonMini<ReferenceGroupObject>( _list, _group, "Removes this reference group." ) )
							return;
							
					ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT_GROUP );

					DrawReferenceGroup( _register, _group );
					
				}
				else
				{
					_list.RemoveAt(_index);
					--_index;
				}
			}
		}

		private static void DrawEntityType( ReferenceGroupObject _obj )
		{
			GUI.backgroundColor = Color.cyan;
			if( _obj.ReferenceGameObject.GetComponent<ICECreatureEntity>() != null )
			{
				ICECreatureEntity _entity = _obj.ReferenceGameObject.GetComponent<ICECreatureEntity>();

				GUILayout.Label( new GUIContent( _entity.EntityType.ToString(), "" ) );
							
			}
			else
			{
				GUILayout.Label( new GUIContent( "undefined", "" ) );
			}

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			GUILayout.Space( 10 );
		}
		
		private static void DrawReferenceGroup( ICECreatureRegister _register, ReferenceGroupObject _obj )
		{
			if( _obj == null || _obj.Foldout == false )
				return;

			EditorGUI.BeginDisabledGroup( _obj.Enabled == false );

				// BEGIN OBJECT
				ICEEditorLayout.BeginHorizontal();
					_obj.ReferenceGameObject = (GameObject)EditorGUILayout.ObjectField( "Reference Object", _obj.ReferenceGameObject, typeof(GameObject), true );			
					EditorGUI.BeginDisabledGroup( _obj.ReferenceGameObject == null );	


						if( _obj.ReferenceGameObject != null )
							ICEEditorLayout.ButtonDisplayObject( _obj.ReferenceGameObject.transform.position );
						else
							ICEEditorLayout.ButtonDisplayObject( Vector3.zero );
						ICEEditorLayout.ButtonSelectObject( _obj.ReferenceGameObject, ICEEditorStyle.CMDButtonDouble );

						//_obj.GroupByTag = ICEEditorLayout.ButtonCheck( "TAG", "Allows to find a group in addition to its name also by its tag.", _obj.GroupByTag, ICEEditorStyle.CMDButtonDouble ); 
						_obj.GroupByTag = false;

						_obj.UseGroupParent = ICEEditorLayout.CheckButtonMiddle( "GROUP", "Assorts instances to the defined Hierarchy Group", _obj.UseGroupParent ); 
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT );
				// END OBJECT

				//_obj.UseHierarchyGroupObject = ICEEditorLayout.Toggle( "Use Hierarchy Group", "", _obj.UseHierarchyGroupObject, Info.REGISTER_REFERENCE_OBJECT_POOL_GROUP_USE );
				if( _obj.UseGroupParent )
				{
					EditorGUI.indentLevel++;
					ICEEditorLayout.BeginHorizontal();
						_obj.CustomGroupParent = (GameObject)EditorGUILayout.ObjectField( "Custom Hierarchy Group", _obj.CustomGroupParent, typeof(GameObject), true );
					ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT_POOL_GROUP_CUSTOM );

					if( _obj.CustomGroupParent == null )
						EditorGUILayout.HelpBox( Info.REGISTER_REFERENCE_OBJECT_POOL_GROUP_INFO, MessageType.Info );
				
					EditorGUI.indentLevel--;
				}

				// BEGIN POOL MANAGEMENT
				if( _obj.PoolManagementEnabled == true && _register.UsePoolManagement == true )
				{
					// PLAYER
					if( _obj.EntityType == EntityClassType.Player )
					{
						_obj.MaxCoexistingObjects = 1;
						_obj.UseInitialSpawn = true;
						_obj.InitialSpawnPriority = 0;
						_obj.UseRandomization = false;
						_obj.SpawnWaveMin = 1;
						_obj.SpawnWaveMax = 1;
						_obj.UseSoftRespawn = true;

						ICEEditorLayout.MinMaxRandomDefaultSlider( "Spawn Interval (min/max)", "", ref _obj.MinSpawnInterval, ref _obj.MaxSpawnInterval,0, ref _obj.RespawnIntervalMax,0,0, 0.25f, 30, Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_INTERVAL ); 

						if( _obj.MinSpawnInterval > 0 && _obj.ReferenceGameObject != null && _obj.ReferenceGameObject.GetComponentsInChildren<Camera>() != null )
						{
							//Debug.Log( "test" );
						}
					}

					// CREATURES AND OTHER OBJECTS
					else
					{
						
						ICEEditorLayout.BeginHorizontal();
							float _maximum = _obj.MaxCoexistingObjectsMaximum;
							_obj.MaxCoexistingObjects = (int)ICEEditorLayout.MaxDefaultSlider( "Max. Coexistent Objects (" + _obj.ActiveObjectsCount + ")","Specifies the limit of coexistent objects", _obj.MaxCoexistingObjects, 1, 0, ref _maximum, 25, "" );
							_obj.MaxCoexistingObjectsMaximum = (int)_maximum;
							_obj.UseMaxSpawnCycles = ICEEditorLayout.CheckButtonSmall( "MAX", "Specifies the total number of objects", _obj.UseMaxSpawnCycles );	
							_obj.UseInitialSpawn = ICEEditorLayout.CheckButtonMiddle( "INITIAL", "Spawns all instances on start according to the given priority", _obj.UseInitialSpawn );				
						ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_MAX  );

						
							if( _obj.UseMaxSpawnCycles )
							{

								if( _obj.MaxSpawnCycles < _obj.MaxCoexistingObjects )
									_obj.MaxSpawnCycles = _obj.MaxCoexistingObjects;
						
								if( _obj.MaxSpawnCyclesMaximum < _obj.MaxCoexistingObjectsMaximum )
									_obj.MaxSpawnCyclesMaximum = _obj.MaxCoexistingObjectsMaximum;
										
								float _max_spawn_cycles = _obj.MaxSpawnCyclesMaximum;								
								_obj.MaxSpawnCycles = (int)ICEEditorLayout.MaxDefaultSlider( "Max. Spawn Cycles (" + _obj.TotalSpawnCycles + ")","", _obj.MaxSpawnCycles, 1, _obj.MaxCoexistingObjects, ref _max_spawn_cycles, 25, "" );
								_obj.MaxSpawnCyclesMaximum = (int)_max_spawn_cycles;
							}

							if( _obj.UseInitialSpawn )
							{
								EditorGUI.indentLevel++;
								_obj.InitialSpawnPriority = (int)ICEEditorLayout.DefaultSlider( "Initial Spawn Priority","", _obj.InitialSpawnPriority, 1, 0, 100,0, Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_PRIORITY );
								EditorGUI.indentLevel--;
							}

						ICEEditorLayout.BeginHorizontal();
							ICEEditorLayout.MinMaxRandomDefaultSlider( "Spawn Interval (min/max)", "", ref _obj.MinSpawnInterval, ref _obj.MaxSpawnInterval,0, ref _obj.RespawnIntervalMax,0,0 ,0.25f, 30 ); 
							_obj.UseSpawnWave = ICEEditorLayout.CheckButton( "WAVE", "",_obj.UseSpawnWave, ICEEditorStyle.ButtonMiddle );
						ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_INTERVAL  );

						if( _obj.UseSpawnWave )
						{
							EditorGUI.indentLevel++;
								ICEEditorLayout.MinMaxRandomDefaultSlider( "Wave Size (min/max)", "Amount per Wave", ref _obj.SpawnWaveMin, ref _obj.SpawnWaveMax,1, ref _obj.SpawnWaveMaximum,1,5, 30, Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_WAVE ); 
							EditorGUI.indentLevel--;
						}

						EditorGUILayout.Separator();


						_obj.UseRandomization = ICEEditorLayout.Toggle( "Spawn Randomization", "", _obj.UseRandomization, Info.REGISTER_REFERENCE_OBJECT_POOL_RANDOM_SIZE );
						if( _obj.UseRandomization )
						{
							EditorGUI.indentLevel++;
								ICEEditorLayout.MinMaxRandomDefaultSlider( "Random Size Variance (min/max)", "", ref _obj.RandomSizeMin, ref _obj.RandomSizeMax,-1,1,0,0, 0.025f, 30, Info.REGISTER_REFERENCE_OBJECT_POOL_RANDOM_SIZE_VARIANCE ); 
							EditorGUI.indentLevel--;
							EditorGUILayout.Separator();
						}

						WorldObjectEditor.DrawCullingOptionsObject( _register, _obj.CullingOptions, EditorHeaderType.TOGGLE, Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_CONDITIONS, "Spawn Conditions"  );
			

					}
					
					ICEEditorLayout.BeginHorizontal();
						ICEEditorLayout.Label( "Spawn Areas", false );
						if( ICEEditorLayout.AddButton( "Adds a new spawn point entry" ) )
							_obj.SpawnPoints.Add( new SpawnPointObject() );
					ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINTS  );
									
					EditorGUI.indentLevel++;

						if( _obj.SpawnPoints.Count == 0 )
						{
							if( _obj.EntityCreature != null )
								_obj.SpawnPoints.Add( new SpawnPointObject( _obj.EntityCreature.Creature.Essentials.Target ) );						
							else if( _obj.ReferenceGameObject != null )
							{
								ICECreatureTargetAttribute _target = _obj.ReferenceGameObject.GetComponent<ICECreatureTargetAttribute>();
								if( _target != null )
									_obj.SpawnPoints.Add( new SpawnPointObject( _target.Target ) );
								else
									_obj.SpawnPoints.Add( new SpawnPointObject( _obj.ReferenceGameObject ) );
							}
						}

						foreach( SpawnPointObject _point in _obj.SpawnPoints )
							if( CreatureObjectEditor.DrawSpawnPointObject( _point, _obj.SpawnPoints, EditorHeaderType.FOLDOUT_ENABLED ) )
								return;

					EditorGUI.indentLevel--;
				}
				// END POOL MANAGEMENT

			EditorGUI.EndDisabledGroup();
			EditorGUILayout.Separator();

		}

		private static void DrawFlags( ReferenceGroupObject _object )
		{

			string[] _flags = new string[10];

			// CC controlled
			if( _object.Status.HasCreatureController )
				_flags[0] = "icons/cc_1";
			else if( _object.Status.HasCreatureAdapter )
				_flags[0] = "icons/failed";
			else
				_flags[0] = "icons/failed";

			if( _object.Status.HasHome )
				_flags[1] = "icons/home_ready";
			else
				_flags[1] = "icons/home_failed";

			if( _object.Status.HasMissionOutpost )
				_flags[2] = "icons/cc_1";
			else
				_flags[2] = "icons/failed";

			if( _object.Status.HasMissionEscort ) 
				_flags[3] = "icons/cc_1";
			else
				_flags[3] = "icons/failed";

			if( _object.Status.HasMissionPatrol ) 
				_flags[4] = "icons/cc_1";
			else
				_flags[4] = "icons/failed";

			if( _object.Status.isActiveAndEnabled ) 
				_flags[5] = "icons/cc_1";
			else
				_flags[5] = "icons/failed";

			if( _object.Status.isActiveInHierarchy ) 
				_flags[6] = "icons/cc_1";
			else
				_flags[6] = "icons/failed";

			if( _object.Status.isPrefab ) 
				_flags[7] = "icons/cc_1";
			else
				_flags[7] = "icons/failed";

			//EditorGUILayout.Separator();
			ICEEditorLayout.DrawLabelIconBar( "Status", _flags, 16, 16, 0,0,5);
		}

		/// <summary>
		/// Updates the creature status.
		/// </summary>
		/// <param name="_object">_object.</param>
		private static void UpdateStatus( ReferenceGroupObject _object )
		{
			if( _object == null )
				return;
			
			_object.Status.HasCreatureController = false;
			_object.Status.HasCreatureAdapter = false;
			_object.Status.HasHome = false;
			_object.Status.HasMissionOutpost = false;
			_object.Status.HasMissionEscort = false;
			_object.Status.HasMissionPatrol = false;
			_object.Status.isActiveAndEnabled = false;
			_object.Status.isActiveInHierarchy = false;
			_object.Status.isPrefab = false;
			
			if( _object.ReferenceGameObject != null )
			{
				if( _object.EntityCreature != null )
				{
					_object.Status.HasCreatureController = true;
					
					if( _object.EntityCreature.isActiveAndEnabled )
						_object.Status.isActiveAndEnabled = true;
					
					if( _object.EntityCreature.Creature.Essentials.TargetReady() )
						_object.Status.HasHome = true;
					
					if( _object.EntityCreature.Creature.Missions.Outpost.TargetReady() )
						_object.Status.HasMissionOutpost = true;
					
					if( _object.EntityCreature.Creature.Missions.Escort.TargetReady() )
						_object.Status.HasMissionEscort = true;
					
					if( _object.EntityCreature.Creature.Missions.Patrol.TargetReady() )
						_object.Status.HasMissionPatrol = true;
					
				}
				
				if( _object.ReferenceGameObject.activeInHierarchy )
					_object.Status.isActiveInHierarchy = true;
				else if( EditorTools.IsPrefab( _object.ReferenceGameObject ) ) // Is a prefab
					_object.Status.isPrefab = true;				
			}
		}
	}
}
