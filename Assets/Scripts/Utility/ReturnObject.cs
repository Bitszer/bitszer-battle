using UnityEngine;
using System.Collections;

public class ReturnObject
{
	public bool success;
	public int message;		// if message = 1 --> Insufficient Resource
							// else message = 2 --> Max Level

	public ReturnObject(){
		success = false;
		message = 1;
	}
}
