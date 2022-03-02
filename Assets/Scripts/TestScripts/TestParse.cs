using UnityEngine;
using System.Collections.Generic;

public class TestParse : MonoBehaviour
{
    private string _testString = "stick:10:lumber:11:copper:12:silver:13:sage:14:rosemary:15:";

    private List<string> _name   = new List<string>();
    private List<int>    _amount = new List<int>();

    private int[] _strOne = new int[2];
    private int[] _strTwo = new int[2];

    public void Start()
    {
        _name.Add("lumber");
        _name.Add("stick");

        _amount.Add(11);
        _amount.Add(14);

        _strOne[0] = 12;
        _strOne[1] = 14;

        _strTwo[0] = 22;
        _strTwo[1] = 25;

        //		string str = CharacterSeperatedParser.MakeCharSeperatedString (':', name, amount);
        var str = CharacterSeperatedParser.Parse(':', _testString);

        foreach (var t in str)
            Debug.Log(t);
    }
}