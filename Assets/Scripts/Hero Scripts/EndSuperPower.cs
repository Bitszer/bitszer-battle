using UnityEngine;
using System.Collections;

public class EndSuperPower : MonoBehaviour 
{
	public void DestroySuperPower()
	{
		this.GetComponentInParent<SuperPower> ().EndSuperPower (this.gameObject);
	}
}
