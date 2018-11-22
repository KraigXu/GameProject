
using UnityEngine;

namespace ICE.Creatures.Objects
{
	public interface INavigationLink
	{
		Vector3 GetStartPosition( GameObject _object, Vector3 _offset );
		Vector3 GetEndPosition( GameObject _object );
	}
}
