using UnityEngine;
using System.Collections;

public class ClampRange
{
	/*
	 * 		You have a range from        15 to 55
	 * 		The range you require is      0 to 1
	 * 		
	 * 		parameters will be as follows:		
	 * 
	 * 		INITIAL_VALUE = 15,    FINAL_VALUE = 55,    initial_value = 0,        final_value = 1
	 * 		
	 * 		valChanging = value in between 15 - 55 you want to MAP to value in between 0 - 1
	*/

	public static float Clamp(float INITIAL_VALUE, float FINAL_VALUE, float initial_value, float final_val, float valChanging)
	{
		return ( (valChanging - INITIAL_VALUE) / (FINAL_VALUE - INITIAL_VALUE) * (final_val - initial_value) + initial_value );
	}
}
