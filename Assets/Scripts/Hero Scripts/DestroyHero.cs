using UnityEngine;
using System.Collections;

public class DestroyHero : MonoBehaviour 
{
	public int side;

	void OnTriggerEnter2D(Collider2D _hit)
	{
		if (side == 0) {
			if (_hit.tag == "enemy") {
				Destroy (_hit.transform.parent.gameObject);
			}
		} else {
			if (_hit.tag == "Player") {
				Destroy (_hit.transform.parent.gameObject);
			}
		}

	}
}
