using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;

public sealed class LevelProgression : MonoBehaviour
{
    private string                  _json;
    private Dictionary<string, int> _levelData;

    public void LoadLevelProgression(Action onComplete)
    {
        if (_json != null)
            throw new Exception("Level progression already loaded.");

        _json = LoadLevelProgressionJson();
        _levelData = new Dictionary<string, int>();
        SetLevelProgression(1, 1);
        
        onComplete?.Invoke();
    }

    public void SetLevelProgression(int level, int difficulty)
    {
        _levelData.Clear();
        var levelProgression = JSON.Parse(_json);
        var levelData = levelProgression["LevelProgression"][level + "," + difficulty];
        foreach (KeyValuePair<string, JSONNode> item in levelData.AsObject)
            _levelData.Add(item.Key, int.Parse(item.Value));
    }

    public int GetEntityLevel(string entityName)
    {
        return _levelData.TryGetValue(entityName, out var level) ? level : 0;
    }

    /*
     * Resources.
     */

    private string LoadLevelProgressionJson()
    {
        return Resources.Load<TextAsset>("LevelProgression").text;
    }
}