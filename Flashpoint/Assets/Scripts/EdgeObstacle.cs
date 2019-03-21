using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EdgeObstacle
{
	bool IsDestroyed();
	void Damage();
	bool IsPassable();
}
