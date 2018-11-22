// ##############################################################################
//
// ICECreatureComponentMenu.cs
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

using UnityEditor;
using UnityEngine;

using ICE;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.Windows;

namespace ICE.Creatures.Menus
{
	public class ICECreatureComponentsMenu : MonoBehaviour {

		// REGISTER
		[MenuItem ( "ICE/ICE Creature Control/Components/Create Creature Register", false, 1000 )]
		static void AddCreatureRegister() {
			ICECreatureRegister.Create();
		}
			
		[MenuItem ( "ICE/ICE Creature Control/Components/Add Creature Register", true)]
		static bool ValidateAddCreatureRegister() {
			return ! ICECreatureRegister.Exists();
		}

		// CRETAURES AND MORE
		[MenuItem ( "ICE/ICE Creature Control/Components/Add Creature Component", false, 1011 )]
		static void AddCreatureControl() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureControl>() == null )
				_object.AddComponent<ICECreatureControl>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Add Creature Component", true)]
		static bool ValidateAddCreatureControl() {
			return ValidateObject();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Add Player Component", false, 1011 )]
		static void AddPlayer() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			
			if( _object != null && _object.GetComponent<ICECreaturePlayer>() == null )
				_object.AddComponent<ICECreaturePlayer>();
		}
		
		[MenuItem ( "ICE/ICE Creature Control/Components/Add Player Component", true)]
		static bool ValidateAddPlayer(){
			return ValidateObject();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Add Plant Component", false, 1011 )]
		static void AddPlant() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreaturePlant>() == null )
				_object.AddComponent<ICECreaturePlant>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Add Plant Component", true)]
		static bool ValidateAddPlant(){
			return ValidateObject();
		}

		// CRETAURES - BODYPARTS
		[MenuItem ( "ICE/ICE Creature Control/Components/Add BodyPart Component", false, 1011 )]
		static void AddBodyPart() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureBodyPart>() == null )
				_object.AddComponent<ICECreatureBodyPart>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Add BodyPart Component", true)]
		static bool ValidateAddBodyPart() {

			if( ValidateObject() )
			{
				GameObject _obj = Selection.activeObject as GameObject;

				if( _obj != null && 
					_obj.GetComponentInParent<ICECreatureEntity>() != null )
					return true;
				else
					return false;
			}
			
			return ValidateObject();
		}

		// ITEMS

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Item Component", false, 1022 )]
		static void AddItem() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			
			if( _object != null && _object.GetComponent<ICECreatureItem>() == null )
				_object.AddComponent<ICECreatureItem>();
		}
		
		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Item Component", true)]
		static bool ValidateAddItem(){
			return ValidateObject();
		}

		// ITEM - TOOL

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Tool Component", false, 1022 )]
		static void AddTool() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			if( _object != null && _object.GetComponent<ICECreatureTool>() == null )
				_object.AddComponent<ICECreatureTool>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Tool Component", true)]
		static bool ValidateAddTool(){
			return ValidateObject();
		}

		// ITEM - FLASHLIGHT 

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Flashlight Component", false, 1022 )]
		static void AddFlashlight() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			if( _object != null && _object.GetComponent<ICECreatureFlashlight>() == null )
				_object.AddComponent<ICECreatureFlashlight>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Flashlight Component", true)]
		static bool ValidateAddFlashlight(){
			return ValidateObject();
		}

		// ITEM - TORCH 

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Torch Component", false, 1022 )]
		static void AddTorch() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			if( _object != null && _object.GetComponent<ICECreatureTorch>() == null )
				_object.AddComponent<ICECreatureTorch>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Items/Add Torch Component", true)]
		static bool ValidateAddTorch(){
			return ValidateObject();
		}

		// WEAPON - MELEE WEAPON

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Melee Weapon Component", false, 1022 )]
		static void AddMeleeWeapon() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureMeleeWeapon>() == null )
				_object.AddComponent<ICECreatureMeleeWeapon>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Melee Weapon Component", true)]
		static bool ValidateAddMeleeWeapon(){
			return ValidateObject();
		}

		// WEAPON - RANGE WEAPON

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Ranged Weapon Component", false, 1022 )]
		static void AddRangedWeapon() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureRangedWeapon>() == null )
				_object.AddComponent<ICECreatureRangedWeapon>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Ranged Weapon Component", true)]
		static bool ValidateAddRangedWeapon(){
			return ValidateObject();
		}

		// WEAPON - BULLET 

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Projectile Component", false, 1022 )]
		static void AddRangedProjectile() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureProjectile>() == null )
				_object.AddComponent<ICECreatureProjectile>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Projectile Component", true)]
		static bool ValidateAddProjectile(){
			return ValidateObject();
		}

		// WEAPON - TURRET

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Turret Component", false, 1022 )]
		static void AddRangedWeaponTurret() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureTurret>() == null )
				_object.AddComponent<ICECreatureTurret>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Turret Component", true)]
		static bool ValidateAddRangedWeaponTurret(){
			return ValidateObject();
		}

		// WEAPON - EXPLOSIVE

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Explosive Component", false, 1022 )]
		static void AddRangedExplosive() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureExplosive>() == null )
				_object.AddComponent<ICECreatureExplosive>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Explosive Component", true)]
		static bool ValidateAddExplosive(){
			return ValidateObject();
		}

		// WEAPON - EXPLOSIVE - MINE

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Mine Component", false, 1022 )]
		static void AddRangedExplosiveMine() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureMine>() == null )
				_object.AddComponent<ICECreatureMine>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Weapons/Add Mine Component", true)]
		static bool ValidateAddExplosiveMine(){
			return ValidateObject();
		}

		// OBJECTS

		[MenuItem ( "ICE/ICE Creature Control/Components/Objects/Add Object Component", false, 1022 )]
		static void AddObject() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureObject>() == null )
				_object.AddComponent<ICECreatureObject>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Objects/Add Object Component", true)]
		static bool ValidateAddObject(){
			return ValidateObject();
		}

		// OBJECTS - DOOR

		[MenuItem ( "ICE/ICE Creature Control/Components/Objects/Add Door Component", false, 1022 )]
		static void AddObjectDoor() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureDoor>() == null )
				_object.AddComponent<ICECreatureDoor>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Objects/Add Door Component", true)]
		static bool ValidateAddObjectDoor(){
			return ValidateObject();
		}

		// LOCATIONS

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Location Component", false, 1022 )]
		static void AddLocation() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureLocation>() == null )
				_object.AddComponent<ICECreatureLocation>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Location Component", true)]
		static bool ValidateAddLocation(){
			return ValidateObject();
		}

		// LOCATIONS - WAYPOINT

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Waypoint Component", false, 1022 )]
		static void AddWaypoint() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			
			if( _object != null && _object.GetComponent<ICECreatureWaypoint>() == null )
				_object.AddComponent<ICECreatureWaypoint>();
		}
		
		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Waypoint Component", true)]
		static bool ValidateAddWaypoint(){
			return ValidateObject();
		}

		// LOCATIONS - MARKER

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Marker Component", false, 1022 )]
		static void AddMarker() 
		{
			GameObject _object = Selection.activeObject as GameObject;
			
			if( _object != null && _object.GetComponent<ICECreatureMarker>() == null )
				_object.AddComponent<ICECreatureMarker>();
		}
		
		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Marker Component", true)]
		static bool ValidateAddMarker(){
			return ValidateObject();
		}

		// LOCATIONS - ZONE

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Zone Component", false, 1022 )]
		static void AddZone() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureZone>() == null )
				_object.AddComponent<ICECreatureZone>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Zone Component", true)]
		static bool ValidateAddZone(){
			return ValidateObject();
		}
			
		// LOCATIONS - FIREPLACE

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Fireplace Component", false, 1022 )]
		static void AddFireplace() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureFireplace>() == null )
				_object.AddComponent<ICECreatureFireplace>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Fireplace Component", true)]
		static bool ValidateAddFireplace(){
			return ValidateObject();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Breadcrumb Component", false, 1022 )]
		static void AddBreadcrumb() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureBreadcrumb>() == null )
				_object.AddComponent<ICECreatureBreadcrumb>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Locations/Add Breadcrumb Component", true)]
		static bool ValidateAddBreadcrumb(){
			return ValidateObject();
		}

		static bool ValidateObject() 
		{
			GameObject _obj = Selection.activeObject as GameObject;
			
			if( _obj != null && 
				_obj.GetComponent<ICECreatureEntity>() == null && 
				_obj.GetComponent<ICECreatureRegister>() == null )
				return true;
			else
				return false;
		}
	}
}