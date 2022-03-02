using UnityEngine;
using System.Collections;

public class CameraBound : MonoBehaviour 
{
	public bool isTriggered = false;

	void OnTriggerEnter2D(Collider2D _hit)
	{
		if (_hit.tag == "bound") 
		{
			isTriggered = true;
		}
	}

	void OnTriggerExit2D(Collider2D _hit)
	{
		if (_hit.tag == "bound") 
		{
			isTriggered = false;
		}
	}
}
