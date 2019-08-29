﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;

/// <summary>
/// 单元元素
/// </summary>
public class HexUnit : MonoBehaviour {

	const float rotationSpeed = 180f;
	const float travelSpeed = 4f;

	public HexGrid Grid { get; set; }


    public Entity Entity
    {
        get
        {
            if (_gameObjectEntity == null)
            {
                _gameObjectEntity = GetComponent<GameObjectEntity>();
                return _gameObjectEntity.Entity;
            }
            else
            {
                return _gameObjectEntity.Entity;
            }

        }
    }

    private GameObjectEntity _gameObjectEntity;

	public HexCell Location {
		get {
			return location;
		}
		set {
			if (location) {
				Grid.DecreaseVisibility(location, VisionRange);
				location.Unit = null;
			}
			location = value;
			value.Unit = this;
			Grid.IncreaseVisibility(value, VisionRange);
			transform.localPosition = value.Position;
            Grid.MakeChildOfColumn(transform,value.ColumnIndex);
		}
	}

	HexCell location, currentTravelLocation;

	public float Orientation {
		get {
			return orientation;
		}
		set {
			orientation = value;
			transform.localRotation = Quaternion.Euler(0f, value, 0f);
		}
	}

	public int Speed {
		get {
			return speed;
		}
	    set
	    {
	        speed = value;
	    }
	}

	public int VisionRange {
		get {
			return 3;
		}
	    set { visionRange = value; }
	}

	float orientation;

	List<HexCell> pathToTravel;

    private int speed=4;
    private int visionRange=3;
    
	public void ValidateLocation () {
		transform.localPosition = location.Position;
	}

	public bool IsValidDestination (HexCell cell) {
		//return cell.IsExplored && !cell.IsUnderwater && !cell.Unit;
	    return cell.IsExplored && !cell.IsUnderwater;
	}

	public void Travel (List<HexCell> path) {
		location.Unit = null;
		location = path[path.Count - 1];
		location.Unit = this;
		pathToTravel = path;
        Debug.Log(pathToTravel.Count);
		StopAllCoroutines();
	    if (pathToTravel.Count > 1)
	    {
	        StartCoroutine(TravelPath());
        }
		
	}

	IEnumerator TravelPath () {
		Vector3 a, b, c = pathToTravel[0].Position;
		yield return LookAt(pathToTravel[1].Position);
		//Grid.DecreaseVisibility(
		//	currentTravelLocation ? currentTravelLocation : pathToTravel[0],
		//	VisionRange
		//);

	    if (!currentTravelLocation)
	    {
	        currentTravelLocation = pathToTravel[0];
	    }
	    Grid.DecreaseVisibility(currentTravelLocation, VisionRange);
	    int currentColumn = currentTravelLocation.ColumnIndex;



        float t = WorldTimeSystem.TimeClip * travelSpeed;
		for (int i = 1; i < pathToTravel.Count; i++) {
			currentTravelLocation = pathToTravel[i];
			a = c;
			b = pathToTravel[i - 1].Position;
			//c = (b + currentTravelLocation.Position) * 0.5f;
			//Grid.IncreaseVisibility(pathToTravel[i], VisionRange);

		    int nextColumn = currentTravelLocation.ColumnIndex;
		    if (currentColumn != nextColumn)
		    {
		        if (nextColumn < currentColumn - 1)
		        {
		            a.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
		            b.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
		        }
		        else if (nextColumn > currentColumn + 1)
		        {
		            a.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
		            b.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
		        }

                Grid.MakeChildOfColumn(transform, nextColumn);
		        currentColumn = nextColumn;
		    }


		    c = (b + currentTravelLocation.Position) * 0.5f;
		    Grid.IncreaseVisibility(pathToTravel[i], VisionRange);

            for (; t < 1f; t += WorldTimeSystem.TimeClip * travelSpeed) {
				transform.localPosition = Bezier.GetPoint(a, b, c, t);
				Vector3 d = Bezier.GetDerivative(a, b, c, t);
				d.y = 0f;
				transform.localRotation = Quaternion.LookRotation(d);
				yield return null;
			}
			Grid.DecreaseVisibility(pathToTravel[i], VisionRange);
			t -= 1f;
		}
		currentTravelLocation = null;

		a = c;
		b = location.Position;
		c = b;
		Grid.IncreaseVisibility(location, VisionRange);
		for (; t < 1f; t += WorldTimeSystem.TimeClip * travelSpeed) {
			transform.localPosition = Bezier.GetPoint(a, b, c, t);
			Vector3 d = Bezier.GetDerivative(a, b, c, t);
			d.y = 0f;
			transform.localRotation = Quaternion.LookRotation(d);
			yield return null;
		}

		transform.localPosition = location.Position;
		orientation = transform.localRotation.eulerAngles.y;
		ListPool<HexCell>.Add(pathToTravel);
		pathToTravel = null;
	}

	IEnumerator LookAt (Vector3 point) {

	    if (HexMetrics.Wrapping)
	    {
	        float xDistance = point.x - transform.localPosition.x;
	        if (xDistance < -HexMetrics.innerRadius * HexMetrics.wrapSize)
	        {
	            point.x += HexMetrics.innerDiameter * HexMetrics.wrapSize;
	        }
	        else if (xDistance > HexMetrics.innerRadius * HexMetrics.wrapSize)
	        {
	            point.x -= HexMetrics.innerDiameter * HexMetrics.wrapSize;
	        }
	    }
        point.y = transform.localPosition.y;
		Quaternion fromRotation = transform.localRotation;
		Quaternion toRotation =
			Quaternion.LookRotation(point - transform.localPosition);
		float angle = Quaternion.Angle(fromRotation, toRotation);

		if (angle > 0f) {
			float speed = rotationSpeed / angle;
			for (
				float t = WorldTimeSystem.TimeClip * speed;
				t < 1f;
				t += WorldTimeSystem.TimeClip * speed
			) {
				transform.localRotation =
					Quaternion.Slerp(fromRotation, toRotation, t);
				yield return null;
			}
		}

		transform.LookAt(point);
		orientation = transform.localRotation.eulerAngles.y;
	}

	public int GetMoveCost (
		HexCell fromCell, HexCell toCell, HexDirection direction)
	{
		if (!IsValidDestination(toCell)) {
			return -1;
		}
		HexEdgeType edgeType = fromCell.GetEdgeType(toCell);
		if (edgeType == HexEdgeType.Cliff) {
			return -1;
		}
		int moveCost;
		if (fromCell.HasRoadThroughEdge(direction)) {
			moveCost = 1;
		}
		else if (fromCell.Walled != toCell.Walled) {
			return -1;
		}
		else {
			moveCost = edgeType == HexEdgeType.Flat ? 5 : 10;
			moveCost +=toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel;
		}
		return moveCost;
	}

	public void Die () {
		if (location) {
			Grid.DecreaseVisibility(location, VisionRange);
		}
		location.Unit = null;
		Destroy(gameObject);
	}

	public void Save (BinaryWriter writer) {
		location.coordinates.Save(writer);
		writer.Write(orientation);
	}

	public static void Load (BinaryReader reader, HexGrid grid) {
		HexCoordinates coordinates = HexCoordinates.Load(reader);
		float orientation = reader.ReadSingle();
		grid.AddUnit(Instantiate(StrategyAssetManager.GetHexUnitPrefabs(1)), grid.GetCell(coordinates), orientation);
	}

	void OnEnable () {
		if (location) {
			transform.localPosition = location.Position;
			if (currentTravelLocation) {
				Grid.IncreaseVisibility(location, VisionRange);
				Grid.DecreaseVisibility(currentTravelLocation, VisionRange);
				currentTravelLocation = null;
			}
		}
	}

//	void OnDrawGizmos () {
//		if (pathToTravel == null || pathToTravel.Count == 0) {
//			return;
//		}
//
//		Vector3 a, b, c = pathToTravel[0].Position;
//
//		for (int i = 1; i < pathToTravel.Count; i++) {
//			a = c;
//			b = pathToTravel[i - 1].Position;
//			c = (b + pathToTravel[i].Position) * 0.5f;
//			for (float t = 0f; t < 1f; t += 0.1f) {
//				Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
//			}
//		}
//
//		a = c;
//		b = pathToTravel[pathToTravel.Count - 1].Position;
//		c = b;
//		for (float t = 0f; t < 1f; t += 0.1f) {
//			Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
//		}
//	}
}