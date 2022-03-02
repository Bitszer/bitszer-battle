using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	public class CharacterSeperatedParser
{
	/// <summary>
	/// Makes the char seperated string.
	/// Param One -> List of string KEY -- 
	/// Param Two -> List of string VALUE
	/// </summary>
	public static string MakeCharSeperatedString(char _character, List<string> _strOne, List<string> _strTwo)
	{
		string _string = "";

		for (int i = 0; i < _strOne.Count; i++) 
		{
			_string += _strOne [i] + _character + _strTwo [i];
			_string += ":";
		}
		_string.Substring (0, _string.Length - 1);
		return _string.Substring (0, _string.Length - 1);
	}

	public static string MakeCharSeperatedString(char _character, List<string> _strOne, List<int> _strTwo)
	{
		string _string = "";

		for (int i = 0; i < _strOne.Count; i++) 
		{
			_string += _strOne [i] + _character + _strTwo [i].ToString();
			_string += ":";
		}

		return _string.Substring (0, _string.Length - 1);
	}

	public static string MakeCharSeperatedString(char _character, int[] _strOne, int[] _strTwo)
	{
		string _string = "";

		for (int i = 0; i < _strOne.Length; i++) 
		{
			_string += _strOne [i].ToString() + _character + _strTwo [i].ToString();
			_string += ":";
		}

		return _string.Substring (0, _string.Length - 1);
	}

	public static string[] Parse(char _character, string _str)
	{
		_str.Substring (0, _str.Length -1);
		string[] parsedSting = _str.Split (_character);
		for (int i = 0; i < parsedSting.Length; i++) 
		{
			parsedSting [i] = parsedSting[i].Trim ();
		}

		return parsedSting;
	}
}
