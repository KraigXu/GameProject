using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.EnumTypes;

public class ICEGridMovements : MonoBehaviour 
{

	private ICECreatureControl m_Controller = null;
	protected ICECreatureControl Controller{
		get{ return m_Controller = ( m_Controller == null ? GetComponent<ICECreatureControl>() : m_Controller ); }
	}

	/// <summary>
	/// The size of the grid.
	/// </summary>
	public int GridSize = 5;

	public bool ShowGrid = true;
	public Color GridColor = new Color( Color.red.r, Color.red.g, Color.red.b, 0.5f );

	/// <summary>
	/// Buffer to store the current grid move position.
	/// </summary>
	private Vector3 m_GridMovePosition = Vector3.zero;

	/// <summary>
	/// Buffer to store the current grid move rotation.
	/// </summary>
	private Quaternion m_GridMoveRotation = Quaternion.identity;

	void Awake ()
	{
		if( Controller == null )
			return;

		Controller.Creature.Move.OnUpdateMovePosition += DoUpdateMovePosition;
		Controller.Creature.Move.OnUpdateStepRotation += DoUpdateStepRotation;

		Vector3 _transform_grid_position = transform.position;
		_transform_grid_position.x = Mathf.RoundToInt( _transform_grid_position.x / GridSize ) * GridSize;
		_transform_grid_position.z = Mathf.RoundToInt( _transform_grid_position.z / GridSize ) * GridSize;

		transform.position = _transform_grid_position;
	}

	//private Quaternion m_MoveRotation;
	//private float m_MoveAngle = 0;

	/// <summary>
	/// DoUpdateMovePosition can be used to override the default move position of a creature. This delegated method will be called on each frame update
	/// </summary>
	/// <param name="_sender">Sender.</param>
	/// <param name="_origin_position">Origin position.</param>
	/// <param name="_new_position">New position.</param>
	private void DoUpdateMovePosition( GameObject _sender, Vector3 _transform_position, ref Vector3 _new_move_position )
	{
		// just to make sure that all required objects are available
		if( Controller == null || Controller.Creature.ActiveTarget == null || Controller.Creature.Move.TargetMovePositionReached )
			return;

		// the active target move position is the final destination the creature have to reach 
		Vector3 _target_move_position = Controller.Creature.ActiveTargetMovePosition;

		// this will adapt the target move position to the grid
		//Vector3 _grid_target_move_position = GetGridPosition( _target_move_position );

		// here we apapt the level of the new move position 
		m_GridMovePosition.y = transform.position.y;

		// if the creature is near to the given node point we have to generate the next one
		if( PositionTools.Distance( m_GridMovePosition, transform.position ) < Controller.Creature.Move.DesiredStoppingDistance )
		{
			// direction to the original target move position of the active target
			Vector3 _dir = ( _target_move_position - transform.position ).normalized;

			// here we get the next grid position according to the direction and the specified grid size
			Vector3 _next_grid_pos = GetGridPosition( transform.position + ( _dir * GridSize ) );

			// We do not want to allow diagonal movements, so we have to adjust all the paths that are longer than the grid size.
			if( PositionTools.Distance( m_GridMovePosition , _next_grid_pos ) > GridSize + Controller.Creature.Move.DesiredStoppingDistance )
			{
				// ... in this case the selection of the direction will be done by chance
				if( UnityEngine.Random.Range( 0, 1 ) == 0 )
					_next_grid_pos.x = m_GridMovePosition.x;
				else
					_next_grid_pos.z = m_GridMovePosition.z;
			}

			// we take the new position only if this is closer to the target than the current one
			//if( ( _next_grid_pos - _grid_target_move_position ).magnitude < ( m_GridMovePosition - _grid_target_move_position ).magnitude )
				
			m_GridMovePosition = _next_grid_pos;
			m_GridMoveRotation = Quaternion.LookRotation( ( m_GridMovePosition - transform.position ).normalized );
				
		}
		else
		{
			// this code block makes sure that a creature is always on the grid
			float _speed = ( Controller.Creature.Move.DesiredVelocity.z > 0 ? Controller.Creature.Move.DesiredVelocity.z : 1 ) * Time.deltaTime;
			Vector3 _move_direction = ( m_GridMovePosition - transform.position );	
			if( Mathf.Abs( _move_direction.x ) < Mathf.Abs( _move_direction.z ) )
				transform.position = Vector3.Lerp( transform.position, new Vector3( transform.position.x + _move_direction.x, transform.position.y, transform.position.z ), _speed );
			else if( Mathf.Abs( _move_direction.z ) < Mathf.Abs( _move_direction.x ) )
				transform.position = Vector3.Lerp( transform.position, new Vector3( transform.position.x, transform.position.y, transform.position.z + _move_direction.z ), _speed );
		}
			
		// here we finally override the default move position of the creature
		_new_move_position = m_GridMovePosition;

	}

	/// <summary>
	/// DoUpdateStepRotation can be used to override the default step rotation of a creature. This delegated method will be called on each frame update
	/// </summary>
	/// <param name="_sender">Sender.</param>
	/// <param name="_origin_rotation">Origin rotation.</param>
	/// <param name="_new_rotation">New rotation.</param>
	private void DoUpdateStepRotation( GameObject _sender, Quaternion _origin_rotation, ref Quaternion _new_rotation ){

		// here we adapt the rotation of the creature by using Quaternion.Lerp to turn the creature smoothly ...
		_new_rotation = Quaternion.Lerp( _new_rotation, m_GridMoveRotation, Controller.Creature.Move.DesiredAngularVelocity.y * Time.deltaTime );

		// ... but you could also adjust the rotation directly without smoothing
		//_new_rotation = Quaternion.LookRotation( ( m_GridMovePosition - transform.position ).normalized );
	}

	/// <summary>
	/// Calulates the grid position.
	/// </summary>
	/// <returns>The grid position.</returns>
	/// <param name="_position">Position.</param>
	private Vector3 GetGridPosition( Vector3 _position )
	{
		_position.x = Mathf.Round( _position.x / GridSize ) * GridSize;
		_position.z = Mathf.Round( _position.z / GridSize ) * GridSize;
		_position.y = 0;

		return _position;
	}

	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	public void OnDrawGizmos(){

		if( ! ShowGrid )
			return;

		Gizmos.color = GridColor;

		int _size = 5;
		for( int i = 0 ; i < 10 ; i++ )
		{
			Vector3 _pos_x1 = new Vector3( m_GridMovePosition.x - ( GridSize * _size ) , 1, m_GridMovePosition.z - ( GridSize * _size ) + ( GridSize * i ) );
			Vector3 _pos_x2 = new Vector3( m_GridMovePosition.x + ( GridSize * _size ) , 1, m_GridMovePosition.z - ( GridSize * _size ) + ( GridSize * i ) );
			Vector3 _pos_z1 = new Vector3( m_GridMovePosition.x - ( GridSize * _size ) + ( GridSize * i ) , 1, m_GridMovePosition.z - ( GridSize * _size ) );
			Vector3 _pos_z2 = new Vector3( m_GridMovePosition.x - ( GridSize * _size ) + ( GridSize * i ) , 1, m_GridMovePosition.z + ( GridSize * _size ) );

			Gizmos.DrawLine( _pos_x1, _pos_x2 );
			Gizmos.DrawLine( _pos_z1, _pos_z2 );

			for( int a = 1 ; a <= 10 ; a++ )
			{
				Vector3 _node_point = _pos_x1 + new Vector3( GridSize * a, 0, 0 ); 
				Vector3 _offset = new Vector3( - GridSize * 0.25f, 10, GridSize * 0.5f ); 

				CustomGizmos.Text( "" + _node_point.x + ":" + _node_point.z, _node_point + _offset, GridColor, 14, FontStyle.Normal );
			}

		}
	}
}

